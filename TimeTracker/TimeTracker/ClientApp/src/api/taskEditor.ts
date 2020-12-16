import { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'

import { APIHandler } from '@/util/apiHandler.ts'
import { Store } from 'vuex/types/index'
import VueRouter from 'vue-router'
import { IStore } from '@/models/store'
import { ICreateTask, IDayData, IDeleteTasks, IQueryTasks, ITask, IUpdateIsLeave, IUpdateTaskCol, IUpdateTaskRowOrder } from '@/models/tasks'
import { WSHandler } from './webSocket'
import { WSMapCode } from '@/models/constants/webSocket'
import { FormatDate } from '@/util/taskDate'
import { GetOptionIdByGuid, GetOptionIdByOption } from '@/util/taskParameters'

class TaskEditorAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetAccounts(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/TaskEditor/GetAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
    }

    GetPeriods(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/TaskEditor/GetPeriods', isLoading, undefined, SuccessFunc, ErrorFunc)
    }
    
    GetTaskTypes(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/TaskEditor/GetTaskTypes', isLoading, undefined, (response) => {
            this._store.commit("taskParameters/SetTaskTypes", response.data)
            
            if(SuccessFunc){
                SuccessFunc(response)
            }           
        }, ErrorFunc, false)
    }

    GetTaskSources(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/TaskEditor/GetTaskSources', isLoading, undefined, (response) => {
            this._store.commit("taskParameters/SetTaskSources", response.data)
            
            if(SuccessFunc){
                SuccessFunc(response)
            }           
        }, ErrorFunc, false)
    }

    GetDaysData(data: IQueryTasks, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
            this.HttpPost('api/TaskEditor/GetDaysData', isLoading, data, SuccessFunc, ErrorFunc)
        }

    CreateTask(data: ICreateTask, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
            this.HttpPost('api/TaskEditor/CreateTask', isLoading, data, SuccessFunc, ErrorFunc)
        }
}

class TaskEditorWSHandler{
    
    protected _store: Store<IStore>
    protected _wsHandler: WSHandler | undefined

    constructor( store: Store<IStore> ) {
        this._store = store
        this._wsHandler = store.state.webSocket.wsHandler
    }

    public Subscribe( queryTasks: IQueryTasks ){
        this._wsHandler?.connection.invoke("SubscribeTaskEditor", queryTasks)
        this._store.commit( "webSocket/SetTaskEditorGroup", queryTasks )
    }

    public Unsubscribe(){
        if( this._store.state.webSocket.taskEditorQueryTasks ){
            this._wsHandler?.connection.invoke("UnsubscribeTaskEditor", this._store.state.webSocket.taskEditorQueryTasks)
        }
    }

    public UpdateIsLeave( updateIsLeave: IUpdateIsLeave ){
        this._wsHandler?.connection.invoke("UpdateIsLeave", updateIsLeave)
    }

    public CreateTask( createTask: ICreateTask ){
        this._wsHandler?.connection.invoke("CreateTask", createTask)
    }

    public DeleteTasks( deleteTasks: IDeleteTasks ){
        this._wsHandler?.connection.invoke("DeleteTasks", deleteTasks)
    }

    public UpdateTaskRowOrder( taskRows: Array<IUpdateTaskRowOrder> ){
        this._wsHandler?.connection.invoke("UpdateTaskRowOrder", taskRows)
    }    

    public UpdateTaskCol( tasks: Array<IUpdateTaskCol> ){
        this._wsHandler?.connection.invoke("UpdateTaskCol", tasks)
    }    
}

class TaskEditorWSListener{
    public connection: signalR.HubConnection | undefined
    protected _store: Store<IStore>
    
    constructor( store: Store<IStore> ) {
        this._store = store
        this.connection = store.state.webSocket.wsHandler?.connection
    }
    
    public InitListener( daysData: Ref<Array<IDayData>> ) {
        this.ListenIsLeave(daysData)
        this.ListenAddRows(daysData)
        this.ListenRemoveRows(daysData)
        this.ListenMoveRows(daysData)
        this.ListenUpdateTaskCol(daysData)
    }

    public ListenIsLeave( daysData: Ref<Array<IDayData>> ){
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorUpdateIsLeave], (message) => {
            // console.log("Click IsLeave", message)

            const updateIsLeave = message as IUpdateIsLeave

            const date = FormatDate(updateIsLeave.date)
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }

            targetDayData.isLeave = updateIsLeave.isLeave
        })
    }

    public ListenAddRows( daysData: Ref<Array<IDayData>> ){
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorCreateTask], (message) => {
            // console.log("CreateTasks", message)
    
            const createTask = message as ICreateTask
            const date = FormatDate(createTask.tasks[0].date)
            const task = createTask.tasks.map( updateTask => ({
                guid: updateTask.guid,
                displayOrder: updateTask.displayOrder,
                date: FormatDate(updateTask.date),
                consumeTime: updateTask.consumeTime,
                taskType: GetOptionIdByOption(this._store.state.taskParameters.taskTypes, updateTask.taskType),
                taskSource: GetOptionIdByOption(this._store.state.taskParameters.taskSources, updateTask.taskSource),
                taskName: updateTask.taskName,
                taskContent: updateTask.taskContent,
            } as ITask ))
    
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }
    
            task.forEach( newTask => {                    
                targetDayData.formData.splice(newTask.displayOrder, 0, newTask)
            })
        })
    }

    public ListenRemoveRows( daysData: Ref<Array<IDayData>> ){
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorDeleteTasks], (message) => {
            // console.log("DeleteTasks", message)
    
            const deleteTasks = message as IDeleteTasks

            const date = FormatDate(deleteTasks.date)    
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }
    
            targetDayData.formData = targetDayData.formData.filter( task => !deleteTasks.taskGuids.includes(task.guid) )
        })
    }

    public ListenMoveRows( daysData: Ref<Array<IDayData>> ){
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorUpdateTaskRowOrder], (message) => {
            // console.log("UpdateTaskRowOrder", message)
    
            const updateTaskRowOrder = message as Array<IUpdateTaskRowOrder>

            const date = FormatDate(updateTaskRowOrder[0].date)    
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }
    
            updateTaskRowOrder.forEach( newTask => {
                const targetTask = targetDayData.formData.find( task => task.guid == newTask.guid )
                if( !targetTask ){
                    return
                }
    
                targetTask.displayOrder = newTask.displayOrder
            })
            targetDayData.formData.sort( ( x, y ) => { 
                if (x.displayOrder < y.displayOrder) {
                    return -1
                }
                if (x.displayOrder > y.displayOrder) {
                    return 1
                }
                return 0
            })
        })
    }
    
    public ListenUpdateTaskCol( daysData: Ref<Array<IDayData>> ){
        // HandleWSEmptyCells
        // HandleWSModifyCells
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorUpdateTaskCol], (message) => {
            // console.log("UpdateTaskCol", message)
    
            const updateTaskCol = message as Array<IUpdateTaskCol>

            const date = FormatDate(updateTaskCol[0].date)    
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }
    
            updateTaskCol.forEach( newTask => {
                const targetTask = targetDayData.formData.find( task => task.guid == newTask.guid )
                if( !targetTask ){
                    return
                }
    
                let value = newTask.value
                switch (newTask.relatedKey) {
                    case "taskType":
                        value = GetOptionIdByGuid(this._store.state.taskParameters.taskTypes, newTask.value)
                        break;
    
                    case "taskSource":
                        value = GetOptionIdByGuid(this._store.state.taskParameters.taskSources, newTask.value)
                        break;
                
                    default:
                        break;
                }
                Reflect.set(targetTask, newTask.relatedKey, value)                    
            })
        })
    }
}

export {
    TaskEditorAPIHandler,
    TaskEditorWSHandler,
    TaskEditorWSListener,
}