import { AccountStatus } from './constants/authentication'

interface IUserRole{
    id: number;
    codeName: string;
    displayName: string;
}

interface IClaims{
    email: string;
    name: string;
    accountStatus: AccountStatus;
    userRoles: Array<IUserRole>;
}

interface IAuthentication{
    isAuthenticated: boolean;
    claims: IClaims;
}

interface IUserInfoDetail{
    id: number;
    name: string;
    email: string;
    accountStatus: AccountStatus;
    userRoles: Array<IUserRole>;
}

function GetAccountRemind(accountStatus: AccountStatus){
    switch (accountStatus) {
        case AccountStatus.Uncheck:
            return "Please connect admin to activate your account."  
        case AccountStatus.Approved:
            return ""
        case AccountStatus.Rejected:
            return "Your account is rejected."
        case AccountStatus.Suspend:
            return "Your account is suspended."
        default:
            return ""
    }
}

export {
    IUserRole,
    IClaims,
    IAuthentication,
    IUserInfoDetail,
    GetAccountRemind,
}