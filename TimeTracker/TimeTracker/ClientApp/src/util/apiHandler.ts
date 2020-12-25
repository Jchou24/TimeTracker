import axios, { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'
import { Store } from 'vuex/types/index'
import { IStore } from '@/models/store'
import VueRouter from 'vue-router'
import { GetRedirectPath } from '@/router/routeConfigs'
import { ValidationResults } from '@/models/authentication'

axios.defaults.withCredentials = true

class APIHandler{
    protected _store: Store<IStore>
    protected _rootRouter: VueRouter
    
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
            ErrorFunc?: (error: any) => void,
            isHandle4XX = true){
        if (isLoading) {
            isLoading.value = true
        }
        
        axios.post(`${process.env.VUE_APP_SERVER_URL}${url}`.toLowerCase(), data )
            .then( response => this.Then( response, SuccessFunc ))
            .catch( error => this.Catch( error, ErrorFunc, isHandle4XX ))
            .finally( () => this.Finally( isLoading ))
    }

    LogoutProcess(){
        this._store.commit("authentication/Init")
        this._store.commit("pageIdle/Init")
        this._store.commit("SetUserImage", "")
        this.HttpPost('api/Account/Logout')
    }

    private Then( response: AxiosResponse<any>, SuccessFunc?: (response: AxiosResponse<any>) => void ){
        if( SuccessFunc ){
            SuccessFunc(response)
        }
    }

    private Catch( error: any, ErrorFunc?: (error: any) => void, isHandle4XX = true ){
        if( ErrorFunc ){
            ErrorFunc(error)
        }

        if( !isHandle4XX ){
            return
        }

        const errorStatusCode = error?.response?.status || undefined
        const toast = this._store.state.notification
        switch (errorStatusCode) {
            case 401:
                this.LogoutProcess()
                toast.Notificate401()                    
                break;

            case 403:
                this._rootRouter.push(GetRedirectPath(ValidationResults.invalidRole))
                toast.Notificate403()
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