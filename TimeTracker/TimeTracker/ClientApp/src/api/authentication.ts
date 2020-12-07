import axios from 'axios'
import { Store } from 'vuex'
import { Ref } from '@vue/composition-api'

axios.defaults.withCredentials = true

interface ILogin{
    email: string;
    name: string;
    password: string;
} 

interface IUpdatePassword{
    currentPassword: string;
    password: string;
}

function GetUserInfo(store: Store<any>, isLoading: Ref<boolean>){
    isLoading.value = true
    return axios.get(process.env.VUE_APP_SERVER_URL + 'api/account/getuserinfo')
        .then( response =>{
            // console.log(response.data)
            store.commit("authentication/SetClaims", response.data)
        })
        .finally(()=>{
            isLoading.value = false                    
        })
}

function IsLogin(store: Store<any>, isLoading: Ref<boolean>){
    // console.log("Run IsLogin API")
    isLoading.value = true
    return axios.get(process.env.VUE_APP_SERVER_URL + 'api/account/islogin')
        .then( response => {
            store.commit("authentication/SetIsAuthenticated", Boolean(response.data))
            if(store.state.authentication.isAuthenticated){
                GetUserInfo(store, isLoading)
            }
        })
        .finally(()=>{
            isLoading.value = false                    
        })
}

function Login(data: ILogin, store: any, isLoading: Ref<boolean>){
    // console.log("Run Login API")
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/login', data)
        .then(()=>{
            GetUserInfo(store, isLoading).then( () => {
                store.commit("authentication/SetIsAuthenticated", true)
            })
        })
        .finally(()=>{
            isLoading.value = false                    
        })
}

function Logout(store: any, isLoading: Ref<boolean>){
    // console.log("run log out")
    isLoading.value = true
    store.commit("authentication/Init")
    store.commit("pageIdle/Init")
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/logout')
        .finally(()=>{
            isLoading.value = false                
        })
}

function Regist(data: ILogin, store: any, isLoading: Ref<boolean>){
    // console.log("Run Login API")
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/regist', data)
        .then(()=>{
            Login(data, store, isLoading)
        })
        .finally(()=>{
            isLoading.value = false                    
        })
}

function KeepAlive(){
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/keepalive')
}

function UpdateName(data: ILogin, store: any, isLoading: Ref<boolean>){
    // console.log("Run Login API")
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/updateName', data)
        .then(()=>{
            store.commit("authentication/SetName", data.name)
        })
        .finally(()=>{
            isLoading.value = false                    
        })
}

function UpdatePassword(data: IUpdatePassword, isLoading: Ref<boolean>){
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/updatePassword', data)
        .finally(()=>{
            isLoading.value = false                    
        })
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