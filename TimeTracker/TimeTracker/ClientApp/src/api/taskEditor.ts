import { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'

import { APIHandler } from '@/util/apiHandler.ts'
import { Store } from 'vuex/types/index'
import VueRouter from 'vue-router'
import { IStore } from '@/models/store'

class TaskEditorAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetAccounts(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/TaskEditor/GetAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
    }
}

export {
    TaskEditorAPIHandler,
}