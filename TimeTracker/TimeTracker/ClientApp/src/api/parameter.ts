import { IOptionMethods, ITaskEntityOption } from "@/models/parameter"
import { IStore } from "@/models/store"
import { APIHandler } from "@/util/apiHandler"
import { Ref } from "@vue/composition-api"
import { AxiosResponse } from "axios"
import VueRouter from "vue-router"
import { Store } from "vuex/types/index"

class ParameterAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetDayWorkLimitTime(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/GetDayWorkLimitTime', isLoading, undefined, (response) => {
            this._store.commit("taskParameters/SetDayWorkLimitTime", response.data)
            
            if(SuccessFunc){
                SuccessFunc(response)
            }           
        }, ErrorFunc, false)
    }
    
    // ======================================================================
    // TaskTypes
    GetTaskTypes(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/GetTaskTypes', isLoading, undefined, (response) => {
            this._store.commit("taskParameters/SetTaskTypes", response.data)
            
            if(SuccessFunc){
                SuccessFunc(response)
            }           
        }, ErrorFunc, false)
    }    

    GetAllTaskTypes(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/GetAllTaskTypes', isLoading, undefined, SuccessFunc, ErrorFunc)
    }    

    CreateTaskTypes(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/CreateTaskTypes', isLoading, data, SuccessFunc, ErrorFunc)
    }

    UpdateTaskTypes(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/UpdateTaskTypes', isLoading, data, SuccessFunc, ErrorFunc)
    }

    // ======================================================================
    // TaskSources
    GetTaskSources(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/GetTaskSources', isLoading, undefined, (response) => {
            this._store.commit("taskParameters/SetTaskSources", response.data)
            
            if(SuccessFunc){
                SuccessFunc(response)
            }           
        }, ErrorFunc, false)
    }

    GetAllTaskSources(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/GetAllTaskSources', isLoading, undefined, SuccessFunc, ErrorFunc)
    }    

    CreateTaskSources(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/CreateTaskSources', isLoading, data, SuccessFunc, ErrorFunc)
    }

    UpdateTaskSources(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Parameter/UpdateTaskSources', isLoading, data, SuccessFunc, ErrorFunc)
    }
}

class TaskTypesOptionAPIHandler extends ParameterAPIHandler implements IOptionMethods{

    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    public GetOptions(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.GetAllTaskTypes(isLoading, SuccessFunc, ErrorFunc)
    }
    public CreateOptions(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.CreateTaskTypes(data, isLoading, SuccessFunc, ErrorFunc)
    }
    public UpdateOptions(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.UpdateTaskTypes(data, isLoading, SuccessFunc, ErrorFunc)
    }
}

class TaskSourcesOptionAPIHandler extends ParameterAPIHandler implements IOptionMethods{

    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    public GetOptions(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.GetAllTaskSources(isLoading, SuccessFunc, ErrorFunc)
    }
    public CreateOptions(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.CreateTaskSources(data, isLoading, SuccessFunc, ErrorFunc)
    }
    public UpdateOptions(data: Array<ITaskEntityOption>, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
        this.UpdateTaskSources(data, isLoading, SuccessFunc, ErrorFunc)
    }
}

export {
    ParameterAPIHandler,
    TaskTypesOptionAPIHandler,
    TaskSourcesOptionAPIHandler,
}