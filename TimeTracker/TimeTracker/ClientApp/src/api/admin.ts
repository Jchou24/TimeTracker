import { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'

import { APIHandler } from '@/util/apiHandler.ts'
import { Store } from 'vuex/types/index'
import VueRouter from 'vue-router'
import { IStore } from '@/models/store'
import { IUpdateAccounts } from '@/models/authentication'

class AdminAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetUncheckAccounts(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Admin/GetUncheckAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
    }

    GetAccounts(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Admin/GetAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
    }

    UpdateAccounts(data: Array<IUpdateAccounts>, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Admin/UpdateAccounts', isLoading, data, SuccessFunc, ErrorFunc)
    }
}

export {
    AdminAPIHandler
}