import { AccountStatus } from './constants/authentication'

interface IUserRole{
    id: number;
    codeName: string;
    displayName: string;
}

interface IClaims{
    guid: string;
    name: string;
    email: string;
    accountStatus: AccountStatus;
    userRoles: Array<IUserRole>;
}

interface IAuthentication{
    isAuthenticated: boolean;
    claims: IClaims;
}

interface ILogin{
    email: string;
    name: string;
    password: string;
} 

interface IUpdatePassword{
    currentPassword: string;
    password: string;
}

interface IUpdateAccounts{
    Guid: string;
    Name: string;
    IsUpdateName: boolean;

    AccountStatus: AccountStatus;
    IsUpdateAccountStatus: boolean;

    UserRoles: Array<number>;
    IsUpdateUserRoles: boolean;
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
    ILogin,
    IUpdatePassword,
    IUpdateAccounts,
    GetAccountRemind,
}