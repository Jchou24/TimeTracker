import axios from 'axios'
import { Ref } from '@vue/composition-api'

import { AccountStatus } from '@/models/constants/authentication.ts'


axios.defaults.withCredentials = true

interface IUpdateAccounts{
    Id: number;
    Name: string;
    IsUpdateName: boolean;

    AccountStatus: AccountStatus;
    IsUpdateAccountStatus: boolean;

    UserRoles: Array<number>;
    IsUpdateUserRoles: boolean;
}

function GetUncheckAccounts(isLoading: Ref<boolean>){
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/admin/getuncheckaccounts')
        .finally(()=>{
            isLoading.value = false                    
        })
}

function GetAccounts(isLoading: Ref<boolean>){
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/admin/getaccounts')
        .finally(()=>{
            isLoading.value = false                    
        })
}

function UpdateAccounts(data: Array<IUpdateAccounts>, isLoading: Ref<boolean>){
    isLoading.value = true
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/admin/updateaccounts', data)
        .finally(()=>{
            isLoading.value = false                    
        })
}

export {
    IUpdateAccounts,
    GetUncheckAccounts,
    GetAccounts,
    UpdateAccounts,
}