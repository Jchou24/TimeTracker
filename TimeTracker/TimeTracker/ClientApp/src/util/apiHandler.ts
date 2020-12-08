import axios, { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'
import { Store } from 'vuex/types/index'
import { IStore } from '@/models/store'
import VueRouter from 'vue-router'
import { GetRedirectPath } from '@/router/routeConfigs'
import { ValidationResults } from '@/models/authentication'
import { ToastWarning } from './notification'

axios.defaults.withCredentials = true

class APIHandler{
    protected _store: Store<IStore>
    private _rootRouter: VueRouter
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        this._store = store
        this._rootRouter = router
    }

    HttpGet(url: string, isLoading?: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void ){
        if (isLoading) {
            isLoading.value = true
        }

        axios.get(`${process.env.VUE_APP_SERVER_URL}${url}`.toLowerCase() )
            .then( response => this.Then( response, SuccessFunc ))
            .catch( error => this.Catch( error, ErrorFunc ))
            .finally( () => this.Finally( isLoading ))
    }

    HttpPost(url: string, isLoading?: Ref<boolean>, data?: any, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void ){
        if (isLoading) {
            isLoading.value = true
        }
        
        axios.post(`${process.env.VUE_APP_SERVER_URL}${url}`.toLowerCase(), data )
            .then( response => this.Then( response, SuccessFunc ))
            .catch( error => this.Catch( error, ErrorFunc ))
            .finally( () => this.Finally( isLoading ))
    }

    LogoutProcess(){
        this._store.commit("authentication/Init")
        this._store.commit("pageIdle/Init")
    }

    private Then( response: AxiosResponse<any>, SuccessFunc?: (response: AxiosResponse<any>) => void ){
        if( SuccessFunc ){
            SuccessFunc(response)
        }
    }

    private Catch( error: any, ErrorFunc?: (error: any) => void ){
        if( ErrorFunc ){
            ErrorFunc(error)
        }

        const errorStatusCode = error?.response?.status || undefined
        const toast = this._store.state.notificator
        switch (errorStatusCode) {
            case 401:
                this.LogoutProcess()
                ToastWarning(toast, `Sorry! You don't have sufficient permission. You will be signed out automatically.` )
                break;

            case 403:
                this._rootRouter.push(GetRedirectPath(ValidationResults.invalidRole))
                ToastWarning(toast, `Sorry! You don't have sufficient permission.` )
                break;
        
            default:
                break;
        }        
    }

    private Finally( isLoading?: Ref<boolean> ){
        if (isLoading) {
            isLoading.value = false
        }
    }
}

export {
    APIHandler
}