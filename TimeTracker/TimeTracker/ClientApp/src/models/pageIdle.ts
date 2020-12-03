enum IdleDetermineStates{
    ByPass,
    PageIdle,
    UserComfirm,
    Logout,
}

interface IPageIdle{
    startIdleTimestamp?: Date;
    startUserConfirmTimestamp?: Date;
    isShowUserConfirm: boolean;
    isShowLogOutNotification: boolean;
    idleTimes: number;
    idleDetermineStates: IdleDetermineStates;
}

export {
    IdleDetermineStates,
    IPageIdle
}