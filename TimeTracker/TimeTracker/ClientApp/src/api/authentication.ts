import axios from 'axios'
import { Store } from 'vuex'

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

function GetUserInfo(store: Store<any>){
    return axios.get(process.env.VUE_APP_SERVER_URL + 'api/account/getuserinfo')
        .then( response =>{
            // console.log(response.data)
            store.commit("authentication/SetClaims", response.data)
        })
}

function IsLogin(store: Store<any>){
    // console.log("Run IsLogin API")
    return axios.get(process.env.VUE_APP_SERVER_URL + 'api/account/islogin')
        .then( response => {
            store.commit("authentication/SetIsAuthenticated", Boolean(response.data))
            if(store.state.authentication.isAuthenticated){
                GetUserInfo(store)
            }
        })
}

function Login(data: ILogin, store: any){
    // console.log("Run Login API")
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/login', data)
        .then(()=>{
            GetUserInfo(store).then( () => {
                store.commit("authentication/SetIsAuthenticated", true)
            })
        })
}

function Logout(store: any){
    // console.log("run log out")
    store.commit("authentication/Init")
    store.commit("pageIdle/Init")
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/logout')
}

function Regist(data: ILogin, store: any){
    // console.log("Run Login API")
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/regist', data)
        .then(()=>{
            Login(data, store)
        })
}

function KeepAlive(){
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/keepalive')
}

function UpdateName(data: ILogin, store: any){
    // console.log("Run Login API")
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/updateName', data)
        .then(()=>{
            store.commit("authentication/SetName", data.name)
        })
}

function UpdatePassword(data: IUpdatePassword){
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/account/updatePassword', data)
}

export {
    ILogin,
    IsLogin,
    IUpdatePassword,
    Login,
    Logout,
    GetUserInfo,
    Regist,
    KeepAlive,
    UpdateName,
    UpdatePassword,
}