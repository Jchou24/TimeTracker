import { IStore } from "@/models/store"
import { IQueryPeopleTasks } from "@/models/tasks"
import { APIHandler } from "@/util/apiHandler"
import { Ref } from "@vue/composition-api"
import { AxiosResponse } from "axios"
import VueRouter from "vue-router"
import { Store } from "vuex/types/index"


class TaskReporterAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetSimpleSummary(data: IQueryPeopleTasks, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
            this.HttpPost('api/TaskReporter/GetSimpleSummary', isLoading, data, SuccessFunc, ErrorFunc)
        }

    GetTaskTypeSummary(data: IQueryPeopleTasks, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
            this.HttpPost('api/TaskReporter/GetTaskTypeSummary', isLoading, data, SuccessFunc, ErrorFunc)
        }

    GetTaskSourceSummary(data: IQueryPeopleTasks, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
            this.HttpPost('api/TaskReporter/GetTaskSourceSummary', isLoading, data, SuccessFunc, ErrorFunc)
        }

    GetTaskTimeSummary(data: IQueryPeopleTasks, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
            this.HttpPost('api/TaskReporter/GetTaskTimeSummary', isLoading, data, SuccessFunc, ErrorFunc)
        }

}


export {
    TaskReporterAPIHandler
}