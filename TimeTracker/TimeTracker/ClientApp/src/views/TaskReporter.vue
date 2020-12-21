<template>
    <div class="TaskReporter">
        <TwoColumn>
            <template v-slot:left>
                <MetaDisplayer 
                    :singleSelectTarget="singleSelectTarget" 
                    :selectedUsers.sync="targetUsers" 
                    :selectedDates.sync="targetDates" 
                    :width="widthMetaDisplayer" />
                <TaskPeriodSimpleSummary 
                    :selectedUsers="targetUsers"
                    :sourceSummary="simpleSummary" 
                    :selectedDates="targetDates" 
                    :isReactiveMode="false" />
            </template>
            <template v-slot:right>
                <div class="charts margin-center">
                    <EchartsPie class="chart" title="工作類型" :isShowLoading="isLoadingTaskType" :pieData="taskTypeSummary" />
                    <EchartsPie class="chart" title="工作來源" :isShowLoading="isLoadingTaskSource" :pieData="taskSourceSummary" />
                    <EchartsPie class="chart" title="工時" :isShowLoading="isLoadingTaskTime" :pieData="taskTimeSummary" />
                </div>
            </template>
        </TwoColumn>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref, watch } from '@vue/composition-api'

    import TwoColumn from '@/views/layouts/TwoColumn.vue'
    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'
    import TaskPeriodSimpleSummary from '@/components/trackTask/charts/TaskPeriodSimpleSummary.vue'
    import EchartsPie from '@/components/trackTask/charts/EchartsPie.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange, IDayData, IQueryPeopleTasks } from '@/models/tasks'
    import { TaskReporterAPIHandler } from '@/api/taskReporter'
    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { IDayCount, IEchartsPieRow } from '@/models/charts'
    import { ValidateDate } from '@/util/taskDate'
    import moment from 'moment'

    export default defineComponent({
        name: 'TaskReporter',
        props:{
            widthMetaDisplayer:{
                type: Number,
                default: 300
            },
            singleSelectTarget:{
                type: Boolean,
                default: true
            },
        },
        components:{
            TwoColumn,
            MetaDisplayer,
            TaskPeriodSimpleSummary,
            EchartsPie,
        },
        setup( props, { refs, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskReporterAPIHandler = new TaskReporterAPIHandler( store, router )

            const targetUsers = ref([] as Array<IClaims>)
            const targetDates = ref({} as IDateRange)
            const queryPeopleTasks = computed( () => ({
                    ownerGuids: targetUsers.value.map( targetUser => targetUser.guid ),
                    startDate: targetDates.value.startDate,
                    endDate: targetDates.value.endDate,
                } as IQueryPeopleTasks))

            const simpleSummary = ref([] as Array<IDayCount>)
            const taskTypeSummary = ref([] as Array<IEchartsPieRow>)
            const taskSourceSummary = ref([] as Array<IEchartsPieRow>)
            const taskTimeSummary = ref([] as Array<IEchartsPieRow>)

            const isLoadingTaskType = ref(false)
            const isLoadingTaskSource = ref(false)
            const isLoadingTaskTime = ref(false)
            // ======================================================================
            const isLoading = ref(false)
            watch( isLoading, () => {
                if(isLoading.value){
                    store.commit("TurnOnLoading")
                }else{
                    store.commit("TurnOffLoading")
                }
            })       
            // ======================================================================
            watch( queryPeopleTasks, () => {
                if( ValidateDate(queryPeopleTasks.value.startDate) !== true || ValidateDate(queryPeopleTasks.value.endDate) !== true){
                    return
                }

                if( !(moment(queryPeopleTasks.value.startDate) <= moment(queryPeopleTasks.value.endDate)) ){
                    return
                }

                if ( queryPeopleTasks.value.ownerGuids.length === 0 ){
                    return
                }                

                const SetDefaultName = ( rows: Array<IEchartsPieRow> ) => rows.forEach( row => {
                        if (!row.name) {
                           row.name = "None" 
                        }
                    })
                    
                taskReporterAPIHandler.GetSimpleSummary( queryPeopleTasks.value, isLoading, (response) => {
                    simpleSummary.value = response.data
                })
                
                taskReporterAPIHandler.GetTaskTypeSummary( queryPeopleTasks.value, isLoadingTaskType, (response) => {
                    taskTypeSummary.value = response.data
                    SetDefaultName(taskTypeSummary.value)
                })

                taskReporterAPIHandler.GetTaskSourceSummary( queryPeopleTasks.value, isLoadingTaskSource, (response) => {
                    taskSourceSummary.value = response.data
                    SetDefaultName(taskSourceSummary.value)
                })

                taskReporterAPIHandler.GetTaskTimeSummary( queryPeopleTasks.value, isLoadingTaskTime, (response) => {
                    taskTimeSummary.value = response.data
                    SetDefaultName(taskTimeSummary.value)
                })
            })
            // ======================================================================

            return {
                targetUsers,
                targetDates,
                simpleSummary,
                taskTypeSummary,
                taskSourceSummary,
                taskTimeSummary,

                isLoadingTaskType,
                isLoadingTaskSource,
                isLoadingTaskTime,
            }
        }        
    })
</script>

<style lang="scss" scoped>
    .TaskReporter{
        width: 100%;
        .charts{
            width: 100%;
            display: flex;
            flex-direction: row;

            .chart{
                flex: 1;
            }
        }
    }
</style>