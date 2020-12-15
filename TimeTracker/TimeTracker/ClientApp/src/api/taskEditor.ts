import { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'

import { APIHandler } from '@/util/apiHandler.ts'
import { Store } from 'vuex/types/index'
import VueRouter from 'vue-router'
import { IStore } from '@/models/store'
import { ICreateTask, IQueryTasks, IUpdateTaskCol, IUpdateTaskRowOrder } from '@/models/tasks'
import { WSHandler } from './webSocket'

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
        this._wsHandler = store.state.wsHandler
    }

    public DeleteTasks( taskGuids: Array<string> ) {
        this._wsHandler?.connection.invoke("DeleteTasks", taskGuids)
    }

    public UpdateTaskRowOrder( taskRows: Array<IUpdateTaskRowOrder> ) {
        this._wsHandler?.connection.invoke("UpdateTaskRowOrder", taskRows)
    }    

    public UpdateTaskCol( tasks: Array<IUpdateTaskCol> ) {
        this._wsHandler?.connection.invoke("UpdateTaskCol", tasks)
    }    
}

export {
    TaskEditorAPIHandler,
    TaskEditorWSHandler,
}