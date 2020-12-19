<template>
    <div class="TaskReporter">
        <TwoColumn>
            <template v-slot:left>
                <MetaDisplayer :selectedUser.sync="targetUser" :selectedDates.sync="targetDates" :width="widthMetaDisplayer" />
                <TaskPeriodSimpleSummary :sourceSummary="summary" :selectedDates="targetDates" :isReactiveMode="false" />
            </template>
            <template v-slot:right>
                
            </template>
        </TwoColumn>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref, watch } from '@vue/composition-api'

    import TwoColumn from '@/views/layouts/TwoColumn.vue'
    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'
    import TaskPeriodSimpleSummary from '@/components/trackTask/charts/TaskPeriodSimpleSummary.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange, IDayData, IQueryTasks } from '@/models/tasks'
    import { TaskReporterAPIHandler } from '@/api/taskReporter'
    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { IDayCount } from '@/components/trackTask/charts/taskPeriodSimpleSummary'

    export default defineComponent({
        name: 'TaskReporter',
        props:{
            widthMetaDisplayer:{
                type: Number,
                default: 300
            }
        },
        components:{
            TwoColumn,
            MetaDisplayer,
            TaskPeriodSimpleSummary,
        },
        setup( props, { refs, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskReporterAPIHandler = new TaskReporterAPIHandler( store, router )

            const targetUser = ref({} as IClaims)
            const targetDates = ref({} as IDateRange)
            const queryTasks = computed( () => ({
                    ownerGuid: targetUser.value.guid,
                    startDate: targetDates.value.startDate,
                    endDate: targetDates.value.endDate,
                } as IQueryTasks))

            // const queryTasks = computed( () => ({
            //         ownerGuid: "3ebe55e4-236e-4782-8df0-5d32f01582f2",
            //         startDate: "2020-12-10",
            //         endDate: "2020-12-12",
            //     } as IQueryTasks))

            const summary = ref([] as Array<IDayCount>)

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
            watch( () => queryTasks.value, () => {
                taskReporterAPIHandler.GetSimpleSummary( queryTasks.value, isLoading, (response) => {
                    summary.value = response.data
                })
            })
            
            // ======================================================================
            // function HandleUpdateDaysData( emitData: Array<IDayData> ){
            //     daysData.value = emitData as Array<IDayData>
            //     (refs.summary as ISummary).InitSummary()
            // }

            return {
                targetUser,
                targetDates,
                summary,
            }
        }        
    })
</script>

<style lang="scss">
    .TaskReporter{
        width: 100%;
        min-width: 1400px;
        padding-top: 30px;
    }
</style>