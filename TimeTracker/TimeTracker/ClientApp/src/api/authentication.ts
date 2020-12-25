import { AxiosResponse } from 'axios'
import { Store } from 'vuex'
import { Ref } from '@vue/composition-api'
import { APIHandler } from '@/util/apiHandler.ts'
import { IClaims, ILogin, IUpdatePassword } from '@/models/authentication'
import { IStore } from '@/models/store'
import VueRouter from 'vue-router'
import { ParameterAPIHandler } from './parameter'
import { ImageAPIHandler } from './imges'

class AuthenticationAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    protected Init(isSetAuthenticated: boolean, isLoading: Ref<boolean>){
        this.GetUserInfo( isLoading, () => {
            if (isSetAuthenticated) {
                this._store.commit("authentication/SetIsAuthenticated", true)    
            }            
        })

        const parameterAPIHandler = new ParameterAPIHandler( this._store, this._rootRouter )
        parameterAPIHandler.GetTaskSources(isLoading)
        parameterAPIHandler.GetTaskTypes(isLoading)

        const imageAPIHandler = new ImageAPIHandler( this._store, this._rootRouter )
        imageAPIHandler.GetImage()
    }

    GetUserInfo(isLoading?: Ref<boolean>,
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpGet('api/Account/GetUserInfo', isLoading, (response) =>{
            if(SuccessFunc){
                SuccessFunc(response)
            }
            this._store.commit("authentication/SetClaims", response.data as IClaims )
        }, ErrorFunc)
    }

    IsLogin(isLoading: Ref<boolean>,
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){    
        this.HttpGet('api/Account/IsLogin', isLoading, (response) =>{
            if(SuccessFunc){
                SuccessFunc(response)
            }
            this._store.commit("authentication/SetIsAuthenticated", Boolean(response.data))
            if(this._store.state.authentication.isAuthenticated){
                this.Init(false, isLoading)
            }
        }, ErrorFunc)
    }

    Login(data: ILogin, isLoading: Ref<boolean>,
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Account/Login', isLoading, data, (response) =>{
            if(SuccessFunc){
                SuccessFunc(response)
            }

            this.Init(true, isLoading)
        }, ErrorFunc)
    }

    Logout(isLoading: Ref<boolean>,
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.LogoutProcess()
        this.HttpPost('api/Account/Logout', isLoading, undefined, SuccessFunc, ErrorFunc)
    }

    Regist(data: ILogin, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Account/Regist', isLoading, data, (response) =>{
            if(SuccessFunc){
                SuccessFunc(response)
            }
            this.Login(data, isLoading)
        }, ErrorFunc)
    }

    KeepAlive(){
        this.HttpPost('api/Account/KeepAlive')
    }

    UpdateName(data: ILogin, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Account/UpdateName', isLoading, data, (response) =>{
            if(SuccessFunc){
                SuccessFunc(response)
            }
            this._store.commit("authentication/SetName", data.name)
        }, ErrorFunc)
    }

    UpdatePassword(data: IUpdatePassword, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Account/UpdatePassword', isLoading, data, SuccessFunc, ErrorFunc)
    }
}

export {
    AuthenticationAPIHandler,
}