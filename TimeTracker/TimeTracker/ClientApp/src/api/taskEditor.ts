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
import { HubConnectionState } from '@microsoft/signalr/dist/esm/HubConnection'

class TaskEditorAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetAccounts(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/TaskEditor/GetAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
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
        if( !this.ValidateState() ){
            return
        }
        this._wsHandler?.connection.invoke("SubscribeTaskEditor", queryTasks)
        this._store.commit( "webSocket/SetTaskEditorGroup", queryTasks )
    }

    public Unsubscribe(){
        if( !this.ValidateState() ){
            return
        }

        if( !this._store.state.webSocket.taskEditorQueryTasks ){
            return
        }
        this._wsHandler?.connection.invoke("UnsubscribeTaskEditor", this._store.state.webSocket.taskEditorQueryTasks)
    }

    public UpdateIsLeave( updateIsLeave: IUpdateIsLeave ){
        if( !this.ValidateState() ){
            return
        }
        this._wsHandler?.connection.invoke("UpdateIsLeave", updateIsLeave)
    }

    public CreateTask( createTask: ICreateTask ){
        if( !this.ValidateState() ){
            return
        }
        this._wsHandler?.connection.invoke("CreateTask", createTask)
    }

    public DeleteTasks( deleteTasks: IDeleteTasks ){
        if( !this.ValidateState() ){
            return
        }
        this._wsHandler?.connection.invoke("DeleteTasks", deleteTasks)
    }

    public UpdateTaskRowOrder( taskRows: Array<IUpdateTaskRowOrder> ){
        if( !this.ValidateState() ){
            return
        }
        this._wsHandler?.connection.invoke("UpdateTaskRowOrder", taskRows)
    }    

    public UpdateTaskCol( tasks: Array<IUpdateTaskCol> ){
        if( !this.ValidateState() ){
            return
        }
        this._wsHandler?.connection.invoke("UpdateTaskCol", tasks)
    }

    protected TryReconnect(){
        const isReconnecting = this._wsHandler?.TryReconnect(() => {
            this._store.state.notification.NotificateWSReconnected()
            // this.Subscribe()
        }, () => {
            this._store.state.notification.NotificateWSClose()
        })

        if (isReconnecting) {
            this._store.state.notification.NotificateWSReconnecting()
        }
    }

    protected ValidateState(){
        if( !this._wsHandler ){
            this._store.state.notification.NotificateWSClose()
            return false
        }

        const connection = this._wsHandler.connection

        if(connection.state === HubConnectionState.Connected) {
            return true
        }

        if( !this._store.state.authentication.isAuthenticated ){
            return false
        }

        switch (connection.state) {
            case HubConnectionState.Disconnected:
                this._store.state.notification.NotificateWSClose()
                return false
            case HubConnectionState.Connecting:
                this._store.state.notification.NotificateWSReconnecting()
                return false
            case HubConnectionState.Disconnecting:
                this._store.state.notification.NotificateWSReconnecting()
                return false
            case HubConnectionState.Reconnecting:
                this._store.state.notification.NotificateWSReconnecting()
                return false        
            default:
                this._store.state.notification.NotificateWSClose()
                return false
        }
    }
}

class TaskEditorWSListener{
    public connection: signalR.HubConnection | undefined
    protected _store: Store<IStore>
    
    constructor( store: Store<IStore> ) {
        this._store = store
        this.connection = store.state.webSocket.wsHandler?.connection
    }
    
    public InitListener( daysData: Ref<Array<IDayData>>, EmitUpdateDaysData: Function ) {
        this.ListenIsLeave(daysData, EmitUpdateDaysData)
        this.ListenAddRows(daysData, EmitUpdateDaysData)
        this.ListenRemoveRows(daysData, EmitUpdateDaysData)
        this.ListenMoveRows(daysData, EmitUpdateDaysData)
        this.ListenUpdateTaskCol(daysData, EmitUpdateDaysData)
    }

    public ListenIsLeave( daysData: Ref<Array<IDayData>>, EmitUpdateDaysData: Function ){
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorUpdateIsLeave], (message) => {
            // console.log("Click IsLeave", message)

            const updateIsLeave = message as IUpdateIsLeave

            const date = FormatDate(updateIsLeave.date)
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }

            targetDayData.isLeave = updateIsLeave.isLeave
            EmitUpdateDaysData()
        })
    }

    public ListenAddRows( daysData: Ref<Array<IDayData>>, EmitUpdateDaysData: Function ){
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
            EmitUpdateDaysData()
        })
    }

    public ListenRemoveRows( daysData: Ref<Array<IDayData>>, EmitUpdateDaysData: Function ){
        this.connection?.on(WSMapCode[WSMapCode.TaskEditorDeleteTasks], (message) => {
            // console.log("DeleteTasks", message)
    
            const deleteTasks = message as IDeleteTasks

            const date = FormatDate(deleteTasks.date)    
            const targetDayData = daysData.value.find( dayData => dayData.date == date )
            if (!targetDayData) {
                return
            }
    
            targetDayData.formData = targetDayData.formData.filter( task => !deleteTasks.taskGuids.includes(task.guid) )
            EmitUpdateDaysData()
        })
    }

    public ListenMoveRows( daysData: Ref<Array<IDayData>>, EmitUpdateDaysData: Function ){
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
            EmitUpdateDaysData()
        })
    }
    
    public ListenUpdateTaskCol( daysData: Ref<Array<IDayData>>, EmitUpdateDaysData: Function ){
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
            EmitUpdateDaysData()
        })
    }
}

export {
    TaskEditorAPIHandler,
    TaskEditorWSHandler,
    TaskEditorWSListener,
}