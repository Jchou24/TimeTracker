<template>
    <div class="TaskEditor">
        <TwoColumn>
            <template v-slot:left>
                <MetaDisplayer 
                    singleSelectTarget 
                    :selectedUsers.sync="targetUsers" 
                    :selectedDates.sync="targetDates" 
                    :width="widthMetaDisplayer" />
                <TaskPeriodSimpleSummary isReactiveMode
                    :selectedUsers="targetUsers" 
                    :daysData="daysData" 
                    :selectedDates="targetDates" 
                    ref="summary" />
            </template>
            <template v-slot:right>
                <TaskDayTimeline class="margin-center" :user="targetUser" :dateRange="targetDates" @updateDaysData="HandleUpdateDaysData" />
            </template>
        </TwoColumn>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, onUnmounted, reactive, ref, watch } from '@vue/composition-api'

    import TwoColumn from '@/views/layouts/TwoColumn.vue'
    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'
    import TaskPeriodSimpleSummary from '@/components/trackTask/charts/TaskPeriodSimpleSummary.vue'
    import TaskDayTimeline from '@/components/trackTask/taskForm/TaskDayTimeline.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange, IDayData } from '@/models/tasks'
    import { TaskEditorAPIHandler, TaskEditorWSHandler } from '@/api/taskEditor'
    import { ParameterAPIHandler } from '@/api/parameter'
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
        setup( props, { refs, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )
            const taskEditorWSHandler = new TaskEditorWSHandler( store )
            const parameterAPIHandler = new ParameterAPIHandler( store, router )

            const targetUsers = ref([] as Array<IClaims>)
            const targetUser = computed( () => targetUsers.value[0] )
            const targetDates = ref({} as IDateRange)
            const daysData = ref([] as Array<IDayData>)
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
                // if( store.state.taskParameters.taskSources.length == 0 ){
                    parameterAPIHandler.GetTaskSources(isLoading)
                // }
                
                // if( store.state.taskParameters.taskTypes.length == 0 ){
                    parameterAPIHandler.GetTaskTypes(isLoading)
                // }

                // if( !store.state.taskParameters.dayWorkLimitTime ){
                    parameterAPIHandler.GetDayWorkLimitTime(isLoading)
                // }
            }
            InitTaskParameter()
            // ======================================================================

            function HandleUpdateDaysData( emitData: Array<IDayData> ){
                daysData.value = emitData as Array<IDayData>
                (refs.summary as ISummary).InitSummary()
            }

            onUnmounted( () => {
                taskEditorWSHandler.Unsubscribe()
            })

            return {
                targetUser,
                targetUsers,
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
        padding-top: 30px;
    }
</style>