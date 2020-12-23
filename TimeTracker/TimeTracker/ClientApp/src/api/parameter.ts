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
}

export {
    ParameterAPIHandler
}