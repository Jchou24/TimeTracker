<template>
    <div class="TaskDayTimeline">
        <v-timeline align-top v-if="daysData.length > 0">
            <RippleTransitionFlip>
                <v-timeline-item
                    v-for="(dayData, idx) in daysData"
                    :key="idx"
                    :data-index="idx"
                    fill-dot
                    
                    right
                    >
                    <template v-slot:icon>
                        <v-btn
                            class="ma-2"
                            
                            fab
                            color="primary"
                            :loading="false"
                            @click="HandleIconClick"
                            >
                            <v-icon>mdi-pencil</v-icon>
                        </v-btn>
                    </template>
                    <template v-slot:opposite>
                        <v-card class="opposite-body" color="primary">
                            <v-card-title class="title">
                                {{FormatDate(dayData.date, "MM/DD")}}
                            </v-card-title>
                        </v-card>
                    </template>

                    <TaskDayForm 
                        :dayData="dayData"
                        :user="user"
                        @handleClickTableForm="HandleClickTableForm"
                        />
                </v-timeline-item>
            </RippleTransitionFlip>
        </v-timeline>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import TaskDayForm from './TaskDayForm.vue'
    import RippleTransitionFlip from '@/util/components/transition/RippleTransitionFlip.vue'

    import debounce from 'lodash.debounce'
    import { Store } from 'vuex/types/index'
    import { TaskEditorAPIHandler, TaskEditorWSHandler } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IClaims } from '@/models/authentication'
    import { IDateRange, IDayData, IGetDaysDataResponse, IQueryTasks, ITaskOption } from '@/models/tasks'
    import { FormatDate, GetDateRange } from '@/util/taskDate'

    export default defineComponent({
        name: 'TaskDayTimeline',
        props:{
            user:{
                type: Object as () => IClaims,
            },
            dateRange:{
                type: Object as () => IDateRange,
            },
        },
        components:{
            TaskDayForm,
            RippleTransitionFlip,
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )
            const taskEditorWSHandler = new TaskEditorWSHandler( store )

            const daysData = ref([] as Array<IDayData>)
            // ======================================================================
            function HandleIconClick() {
                // click to scroll top
                console.log("click icon")
            }

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
            }
            InitTaskParameter()
            // ======================================================================

            function HandleClickTableForm(dayData: IDayData){
                daysData.value.forEach( d => d.isFormClicked = false)
                dayData.isFormClicked = true
            }

            // ======================================================================


            
            
            const GetDaysData = ( dateRange: Array<string>, tasksDays: Array<IDayData> ) => {
                    const result = [] as Array<IDayData>
                    const mapper = {} as Record<string, IDayData>
                    tasksDays.forEach( tasksDay => mapper[tasksDay.date] = tasksDay )

                    dateRange.forEach( date => {
                        const defaultDayData = {
                            guid: "",
                            date: date,
                            isLeave: false,
                            isFormClicked: false,
                            formData: [],
                        } as IDayData

                        result.push({
                            ...defaultDayData,
                            ...mapper[date],
                        })
                    })

                    // console.log( result )

                    return result
                }

            const GetOptionIdByOption = ( storeTasksOptions: Array<ITaskOption>, value: ITaskOption ) => storeTasksOptions.find( x => x.guid === value.guid )?.id

            function SetDaysData() {
                const dateRange = GetDateRange(props.dateRange?.startDate, props.dateRange?.endDate)
                if( dateRange.length == 0 ){
                    return
                }

                taskEditorAPIHandler.GetDaysData({
                        ownerGuid: props.user?.guid,
                        startDate: props.dateRange?.startDate,
                        endDate: props.dateRange?.endDate,
                    } as IQueryTasks, isLoading, (response) => {
                        const tasksDays = response.data as Array<IGetDaysDataResponse>
                        tasksDays.forEach( tasksDay => tasksDay.formData.forEach( task => {
                            task.date = tasksDay.date
                            task.taskSource = GetOptionIdByOption( store.state.taskParameters.taskSources, task.taskSource as ITaskOption ) as number
                            task.taskType = GetOptionIdByOption( store.state.taskParameters.taskTypes, task.taskType as ITaskOption ) as number
                        }))
                        // console.log(tasksDays)
                        daysData.value = GetDaysData(dateRange, tasksDays as Array<IDayData>)
                    })
            }

            watch( () => [ props.user, props.dateRange ], debounce( () => {
                SetDaysData()
                // TODO
                // 要做差異讀取
            }, 500 ))
            
            // ======================================================================
            // TODO 
            //     要在這邊設置 ws on 接收後端派送的更新訊息
            // HandleWSAddRows
            // HandleWSRemoveRows
            // HandleWSMoveRows
            // HandleWSEmptyCells
            // HandleWSModifyCells
            

            return {
                isLoading,
                daysData,

                FormatDate,
                HandleIconClick,
                HandleClickTableForm,
            }
        }        
    })
</script>

<style lang="scss">
    $sticky-top: 100px; // TODO 要用js控制這個值，或訂好這個值，或許可以用 css variable 來控制

    
    $form-width: 900px;
    // $timeline-date-width: 135px;
    $timeline-date-width: 85px;
    $form-margin-left: calc( ( ( ( #{$form-width} - 96px ) / 2 ) - #{$timeline-date-width} ) * -1 );

    .TaskDayTimeline{
        width: $form-width;
        margin-left: $form-margin-left;
    }

    .TaskDayTimeline .v-timeline-item__opposite{
        align-self: unset;
        flex: initial;
        width: $timeline-date-width;
        max-width: $timeline-date-width;

        .opposite-body{
            position: sticky;
            top: $sticky-top;
        }
    }

    .TaskDayTimeline .v-timeline-item__body{
        flex: initial;
    }

    .TaskDayTimeline .v-timeline-item__dot{
        position: sticky;
        // margin-left: -30px;
        left: 0px;
        top: calc( #{$sticky-top} + 15px );
        // top: $sticky-top;
        margin-top: 15px;
    }
</style>