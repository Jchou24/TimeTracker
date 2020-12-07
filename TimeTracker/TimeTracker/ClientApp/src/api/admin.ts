import { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'

import { HttpGet, HttpPost } from '@/util/apiHandler.ts'
import { AccountStatus } from '@/models/constants/authentication.ts'

interface IUpdateAccounts{
    Id: number;
    Name: string;
    IsUpdateName: boolean;

    AccountStatus: AccountStatus;
    IsUpdateAccountStatus: boolean;

    UserRoles: Array<number>;
    IsUpdateUserRoles: boolean;
}

function GetUncheckAccounts(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Admin/GetUncheckAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
}

function GetAccounts(isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Admin/GetAccounts', isLoading, undefined, SuccessFunc, ErrorFunc)
}

function UpdateAccounts(data: Array<IUpdateAccounts>, isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void){
    HttpPost('api/Admin/UpdateAccounts', isLoading, data, SuccessFunc, ErrorFunc)
}

export {
    IUpdateAccounts,
    GetUncheckAccounts,
    GetAccounts,
    UpdateAccounts,
}