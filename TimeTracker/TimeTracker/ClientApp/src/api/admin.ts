import axios from 'axios'
import { Store } from 'vuex'

import { AccountStatus } from '@/models/authentication.ts'

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

function GetUncheckAccounts(){
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/admin/getuncheckaccounts')
}

function GetAccounts(){
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/admin/getaccounts')
}

function UpdateAccounts(data: Array<IUpdateAccounts>){
    return axios.post(process.env.VUE_APP_SERVER_URL + 'api/admin/updateaccounts', data)
}

export {
    IUpdateAccounts,
    GetUncheckAccounts,
    GetAccounts,
    UpdateAccounts,
}