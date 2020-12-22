import { IPeriod } from "@/models/period"
import { IStore } from "@/models/store"
import { APIHandler } from "@/util/apiHandler"
import { Ref } from "@vue/composition-api"
import { AxiosResponse } from "axios"
import VueRouter from "vue-router"
import { Store } from "vuex/types/index"


class PeriodAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetPeriods(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Period/GetPeriods', isLoading, undefined, SuccessFunc, ErrorFunc)
    }

    CreatePeriods(data: Array<IPeriod>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Period/CreatePeriods', isLoading, data, SuccessFunc, ErrorFunc)
    }

    UpdatePeriods(data: Array<IPeriod>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Period/UpdatePeriods', isLoading, data, SuccessFunc, ErrorFunc)
    }

    DeletePeriods(data: Array<IPeriod>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Period/DeletePeriods', isLoading, data, SuccessFunc, ErrorFunc)
    }
}

export {
    PeriodAPIHandler
}