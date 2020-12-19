<template>
    <div class="TaskEditor">
        <TwoColumn>
            <template v-slot:left>
                <MetaDisplayer :selectedUser.sync="targetUser" :selectedDates.sync="targetDates" :width="widthMetaDisplayer" />
                <TaskPeriodSimpleSummary :daysData="daysData" :selectedDates="targetDates" isReactiveMode ref="summary" />
            </template>
            <template v-slot:right>
                <TaskDayTimeline :user="targetUser" :dateRange="targetDates" @updateDaysData="HandleUpdateDaysData" />
            </template>
        </TwoColumn>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref, watch } from '@vue/composition-api'

    import TwoColumn from '@/views/layouts/TwoColumn.vue'
    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'
    import TaskPeriodSimpleSummary from '@/components/trackTask/charts/TaskPeriodSimpleSummary.vue'
    import TaskDayTimeline from '@/components/trackTask/taskForm/TaskDayTimeline.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange, IDayData } from '@/models/tasks'
    import { TaskEditorAPIHandler, TaskEditorWSHandler } from '@/api/taskEditor'
    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'

    interface ISummary{
        InitSummary: Function;
    }

    export default defineComponent({
        name: 'TaskEditor',
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
            TaskDayTimeline,
        },
        beforeRouteLeave(to, from, next) {
            const taskEditorWSHandler = new TaskEditorWSHandler( this.$store )
            taskEditorWSHandler.Unsubscribe()
            next();
        },
        setup( props, { refs, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )

            const targetUser = ref({} as IClaims)
            const targetDates = ref({} as IDateRange)
            const daysData = ref([] as Array<IDayData>)

            // console.log( targetUser )

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
            function InitTaskParameter(){
                if( store.state.taskParameters.taskSources.length == 0 ){
                    taskEditorAPIHandler.GetTaskSources(isLoading)
                }
                
                if( store.state.taskParameters.taskTypes.length == 0 ){
                    taskEditorAPIHandler.GetTaskTypes(isLoading)
                }

                if( !store.state.taskParameters.dayWorkLimitTime ){
                    taskEditorAPIHandler.GetDayWorkLimitTime(isLoading)
                }
            }
            InitTaskParameter()
            // ======================================================================

            function HandleUpdateDaysData( emitData: Array<IDayData> ){
                daysData.value = emitData as Array<IDayData>
                (refs.summary as ISummary).InitSummary()
            }

            return {
                targetUser,
                targetDates,
                daysData,
                HandleUpdateDaysData
            }
        }        
    })
</script>

<style lang="scss">
    .TaskEditor{
        width: 100%;
        min-width: 1400px;
        padding-top: 30px;
    }
</style>