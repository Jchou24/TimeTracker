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

const accountStatusIcon = {
    0: "mdi-account",
    1: "mdi-account",
    2: "mdi-account",
    3: "mdi-account",
}

const accountStatusColor = {
    0: "warning",
    1: "primary",
    2: "error",
    3: "error",
}

function GetAccountStatusKey(){
    return Object.keys(AccountStatus).map(Number).filter( x => !Number.isNaN(x))
}

function GetUserRolesKey(){
    return Object.keys(UserRoles).map(Number).filter( x => !Number.isNaN(x))
}

export {
    accountStatusIcon,
    accountStatusColor,
    AccountStatus,
    UserRoles,
    GetAccountStatusKey,
    GetUserRolesKey,
}