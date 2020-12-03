enum AccountStatus{
    Uncheck = 0, // 未審核用戶
    Approved = 1, // 合法用戶
    Rejected = 2, // 拒絕用戶
    Suspend = 3, // 停權用戶
}

enum UserRoles
{
    Admin = 1,
    User = 2,        
}

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

export {
    AccountStatus,
    UserRoles,
    IUserRole,
    IClaims,
    IAuthentication,
    IUserInfoDetail,
}