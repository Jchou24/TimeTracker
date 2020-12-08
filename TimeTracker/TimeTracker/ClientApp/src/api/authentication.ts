import { AxiosResponse } from 'axios'
import { Store } from 'vuex'
import { Ref } from '@vue/composition-api'
import { HttpGet, HttpPost } from '@/util/apiHandler.ts'
import { IClaims } from '@/models/authentication'

interface ILogin{
    email: string;
    name: string;
    password: string;
} 

interface IUpdatePassword{
    currentPassword: string;
    password: string;
}

function GetUserInfo(store: Store<any>, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){    
    HttpGet('api/Account/GetUserInfo', isLoading, (response) =>{
        if(SuccessFunc){
            SuccessFunc(response)
        }
        store.commit("authentication/SetClaims", response.data as IClaims )
    }, ErrorFunc)
}

function IsLogin(store: Store<any>, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){    
    HttpGet('api/Account/IsLogin', isLoading, (response) =>{
        if(SuccessFunc){
            SuccessFunc(response)
        }
        store.commit("authentication/SetIsAuthenticated", Boolean(response.data))
        if(store.state.authentication.isAuthenticated){
            GetUserInfo(store, isLoading)
        }
    }, ErrorFunc)
}

function Login(data: ILogin, store: any, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Account/Login', isLoading, data, (response) =>{
        if(SuccessFunc){
            SuccessFunc(response)
        }
        GetUserInfo(store, isLoading, () => {
            store.commit("authentication/SetIsAuthenticated", true)
        })
    }, ErrorFunc)
}

function Logout(store: any, isLoading: Ref<boolean>,
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    store.commit("authentication/Init")
    store.commit("pageIdle/Init")
    HttpPost('api/Account/Logout', isLoading, undefined, SuccessFunc, ErrorFunc)
}

function Regist(data: ILogin, store: any, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Account/Regist', isLoading, data, (response) =>{
        if(SuccessFunc){
            SuccessFunc(response)
        }
        Login(data, store, isLoading)
    }, ErrorFunc)
}

function KeepAlive(){
    HttpPost('api/Account/KeepAlive')
}

function UpdateName(data: ILogin, store: any, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Account/UpdateName', isLoading, data, (response) =>{
        if(SuccessFunc){
            SuccessFunc(response)
        }
        store.commit("authentication/SetName", data.name)
    }, ErrorFunc)
}

function UpdatePassword(data: IUpdatePassword, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Account/UpdatePassword', isLoading, data, SuccessFunc, ErrorFunc)
}

export {
    ILogin,
    IUpdatePassword,
    IsLogin,
    Login,
    Logout,
    GetUserInfo,
    Regist,
    KeepAlive,
    UpdateName,
    UpdatePassword,
}