<template>
    <div class="TaskDayTimeline" align-top v-if="daysData.length > 0">
        <div class="task-timeline-line" />
        <div class="task-timeline-body" >
            <RippleTransitionFlip v-if="isShowContent">
                <div class="timeline-row" :class="{ 'focus-row': dayData.isFormClicked }"
                    v-for="(dayData, idx) in daysData"
                    :key="idx"
                    :data-index="idx"
                    >
                    
                    <TaskDayTimelineTitle class="timeline-row-title" :dayData="dayData" :user="user" />

                    <TaskDayForm class="timeline-row-form"
                        :dayData="dayData"
                        :user="user"
                        @handleClickTableForm="HandleClickTableForm"
                        @input="EmitUpdateDaysData"
                        />
                </div>
            </RippleTransitionFlip>
        </div>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import TaskDayForm from './TaskDayForm.vue'
    import TaskDayTimelineTitle from './TaskDayTimelineTitle.vue'
    // import TaskDayLeaveIcon from './TaskDayLeaveIcon.vue'
    // import TaskDayLeaveCheckbox from './TaskDayLeaveCheckbox.vue'
    import RippleTransitionFlip from '@/util/components/transition/RippleTransitionFlip.vue'

    import debounce from 'lodash.debounce'
    import { Store } from 'vuex/types/index'
    import { TaskEditorAPIHandler, TaskEditorWSHandler, TaskEditorWSListener } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IClaims } from '@/models/authentication'
    import { IDateRange, IDayData, IGetDaysDataResponse, 
        IQueryTasks, ITask, ITaskOption } from '@/models/tasks'
    import { FormatDate, GetDateRange } from '@/util/taskDate'
    import { GetOptionIdByOption } from '@/util/taskParameters'

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
            TaskDayTimelineTitle,
            // TaskDayLeaveIcon,
            // TaskDayLeaveCheckbox,
            RippleTransitionFlip,
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )
            const taskEditorWSHandler = new TaskEditorWSHandler( store )
            const taskEditorWSListener = new TaskEditorWSListener( store )

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

            const isShowContent = ref(false)
            function SetDaysData() {
                const dateRange = GetDateRange(props.dateRange?.startDate, props.dateRange?.endDate)
                if( dateRange.length == 0 ){
                    return
                }

                const queryTasks = {
                    ownerGuid: props.user?.guid,
                    startDate: props.dateRange?.startDate,
                    endDate: props.dateRange?.endDate,
                } as IQueryTasks

                taskEditorAPIHandler.GetDaysData(queryTasks, isLoading, (response) => {
                        const tasksDays = response.data as Array<IGetDaysDataResponse>
                        tasksDays.forEach( tasksDay => tasksDay.formData.forEach( task => {
                            task.date = tasksDay.date
                            task.taskSource = GetOptionIdByOption( store.state.taskParameters.taskSources, task.taskSource as ITaskOption ) as number
                            task.taskType = GetOptionIdByOption( store.state.taskParameters.taskTypes, task.taskType as ITaskOption ) as number
                        }))
                        // console.log(tasksDays)
                        daysData.value = GetDaysData(dateRange, tasksDays as Array<IDayData>)

                        setTimeout( () => isShowContent.value = true, 150)
                        // emit("updateDaysData", daysData.value)
                    })

                taskEditorWSHandler.Unsubscribe()
                taskEditorWSHandler.Subscribe(queryTasks)
            }

            watch( () => [ props.user, props.dateRange ], debounce( () => {
                isShowContent.value = false
                SetDaysData()

                // TODO
                // 要做差異讀取
                
            }, 500 ))

            function EmitUpdateDaysData() {
                emit("updateDaysData", daysData.value)
                // console.log("updateDaysData")
            }
            watch( () => [daysData.value], () => EmitUpdateDaysData() )
            
            // ======================================================================
            // Hadnler ws message
            
            taskEditorWSListener.InitListener(daysData, EmitUpdateDaysData) 

            return {
                isLoading,
                isShowContent,
                daysData,

                HandleClickTableForm,
                EmitUpdateDaysData,
            }
        }        
    })
</script>

<style lang="scss" scoped>
    .TaskDayTimeline{
        display: flex;
        flex-direction: row;
        padding-left: 80px;
        min-width: 1100px;
    }

    $ilne-margin-right: 80px;

    .TaskDayTimeline .task-timeline-line{
        width: 10px;
        margin-right: $ilne-margin-right;
        background: #e0e0e0;
        border-radius: 15px;
    }

    .TaskDayTimeline .task-timeline-body .timeline-row{
        margin-bottom: 25px;
        position: relative;

        &.focus-row{
            z-index: 1;
        }
    }

    .TaskDayTimeline .task-timeline-body .timeline-row .timeline-row-title{
        position: absolute;
        left: calc( ( #{$ilne-margin-right} + 67px ) * -1 );
        top: 0px;
        border-radius: 15px;
    }

    .TaskDayTimeline .task-timeline-body .timeline-row .timeline-row-form{
        @include vm-drop-shadow-1(5px, 5px, 5px, #c5c3c3);
    }
</style>

<style lang="scss" scoped>
</style>