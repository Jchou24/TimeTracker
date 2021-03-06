<template>
    <div class="TaskDayForm">
        <TableForm class="table-form"
            :class="GetFormClass(dayData)"
            v-model="dayData.formData"
            :isReadonly="!dayData.isFormClicked || dayData.isLeave"
            :options="options"

            v-on-clickaway="() => dayData.isFormClicked = false"
            @click.native="HandleClickTableForm(dayData)"
            @addRows="HandleAddRows(dayData, $event)"
            @removeRows="HandleRemoveRows(dayData, $event)"
            @moveRows="HandleMoveRows(dayData, $event)"
            @emptyCells="HandleEmptyCells(dayData, $event)"
            @modifyCells="HandleModifyCells(dayData, $event)"
            @input="$emit('input')"

            v-if="isOptionsReady"
            />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import { directive as onClickaway } from 'vue-clickaway2'
    import { Store } from 'vuex/types/index'
    import { TaskEditorAPIHandler, TaskEditorWSHandler } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IClaims } from '@/models/authentication'
    import { ICreateTask, IDateRange, IDayData, IDeleteTasks, ITaskOption, 
        IUpdateTask, IUpdateTaskCol, IUpdateTaskRowOrder } from '@/models/tasks'
    import { GetOptions } from '@/models/constants/task'
    import { GetGuid } from '@/util/authentication'
    import { GetOptionById, GetOptionGuidById } from '@/util/taskParameters'

    import { IAddRow, IMoveRow, IRemoveRow, IUpdateCell } from './taskform'

    export default defineComponent({
        name: 'TaskDayForm',
        props:{
            user:{
                type: Object as () => IClaims,
            },
            dayData:{
                type: Object as () => IDayData,
            }
        },
        directives: {
            onClickaway: onClickaway,
        },
        components:{
            
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )
            const taskEditorWSHandler = new TaskEditorWSHandler( store )
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
            function GetFormClass( dayData: IDayData ){

                const CheckIsLightForm = ( dayData: IDayData ) => {
                    if (dayData.isLeave) {
                        return true
                    }

                    if (dayData.formData.length == 0 && dayData.isFormClicked == false ) {
                        return true
                    }

                    return false
                }

                return { 
                    'light-form': CheckIsLightForm(dayData),
                }
            }
            // ======================================================================
            function HandleClickTableForm(){
                emit('handleClickTableForm', props.dayData)
            }
            // ======================================================================
            function HandleAddRows(dayData: IDayData, emitData: Array<IAddRow>){
                // console.log("Handle Add Rows")
                // console.table(emitData)

                const tasks = emitData.map( x => ({
                        guid: GetGuid(),
                        displayOrder: x.newIndex,
                        date: dayData.date,
                        taskType: GetOptionById( store.state.taskParameters.taskTypes, dayData.formData[x.newIndex].taskType ),
                        taskSource: GetOptionById( store.state.taskParameters.taskSources, dayData.formData[x.newIndex].taskSource ),
                        taskName: dayData.formData[x.newIndex].taskName,
                        taskContent: dayData.formData[x.newIndex].taskContent,
                    } as IUpdateTask ))

                const createTask = {
                    ownerGuid: props.user?.guid,
                    tasks,
                } as ICreateTask
                
                taskEditorAPIHandler.CreateTask(createTask, isLoading, 
                    ( response ) => {
                        const guids = response.data as Array<string>
                        emitData.forEach( ( row, idx ) => {
                            dayData.formData[row.newIndex].guid = guids[idx]
                        })

                        createTask.tasks.forEach( (task, idx) => task.guid = guids[idx] )
                        taskEditorWSHandler.CreateTask(createTask)
                    })
            }

            function HandleRemoveRows(dayData: IDayData, emitData: Array<IRemoveRow>){
                // console.log("Handle Remove Rows")
                // console.table(emitData)

                taskEditorWSHandler.DeleteTasks({
                    ownerGuid: props.user?.guid,
                    date: dayData.date,
                    taskGuids: emitData.map( x => x.value.guid ),
                } as IDeleteTasks)
            }

            function HandleMoveRows(dayData: IDayData, emitData: Array<IMoveRow>){
                // console.log("Handle Move Rows")
                // console.table(emitData)

                taskEditorWSHandler.UpdateTaskRowOrder(emitData.map( moveRow => ({ 
                        ownerGuid: props.user?.guid,
                        guid: dayData.formData[moveRow.newIndex].guid,
                        date: dayData.date,
                        displayOrder: moveRow.newIndex
                    } as IUpdateTaskRowOrder ))
                )
            }

            function HandleModifyCells(dayData: IDayData, emitData: Array<IUpdateCell>){
                // console.log("Handle Modify Cells")
                // console.table(emitData)

                function GetValue(relatedKey: string, value: any){
                    if( relatedKey == "taskType" ){
                        return GetOptionGuidById(store.state.taskParameters.taskTypes, value)
                    }

                    if( relatedKey == "taskSource" ){
                        return GetOptionGuidById(store.state.taskParameters.taskSources, value)
                    }

                    return value
                }

                const updateTaskCols = [] as Array<IUpdateTaskCol>
                emitData.forEach( cell => {
                    if( cell.newValue !== null ){
                        updateTaskCols.push({
                            ownerGuid: props.user?.guid,
                            guid: dayData.formData[cell.rowIndex].guid,
                            date: dayData.date,
                            relatedKey: cell.relatedKey,
                            value: GetValue(cell.relatedKey, cell.newValue),
                        } as IUpdateTaskCol)
                    }
                })

                if (updateTaskCols.length > 0) {
                    taskEditorWSHandler.UpdateTaskCol(updateTaskCols)    
                }
            }

            function HandleEmptyCells(dayData: IDayData, emitData: Array<IUpdateCell>){
                // console.log("Handle Empty Cells")
                // console.table(emitData)
                HandleModifyCells( dayData, emitData )
            }
            // ======================================================================

            return {
                options: computed( () => GetOptions(store.state.taskParameters) ),
                isOptionsReady: computed( () => store.state.taskParameters.taskTypes.length > 0 && store.state.taskParameters.taskSources.length > 0),

                GetFormClass,
                HandleClickTableForm,
                HandleAddRows,
                HandleRemoveRows,
                HandleMoveRows,
                HandleEmptyCells,
                HandleModifyCells,
            }
        }        
    })
</script>

<style lang="scss">
    .TaskDayForm .CellTypeTextArea .counter{
        margin-top: -29px;
        margin-left: -2px;
    }

    .TaskDayForm .table-form{
        &.light-form{
            opacity: 0.2;
        }
    }

    .TaskDayForm .multiselect__content{
        padding-left: 0px;
    }
</style>