import { IdleDetermineStates, IPageIdle } from '@/models/pageIdle'

function GetInitPageIdle(): IPageIdle{
    return {
        startIdleTimestamp: undefined,
        startUserConfirmTimestamp: undefined,
        isShowUserConfirm: false,
        isShowLogOutNotification: false,
        idleTimes: 0,
        idleDetermineStates: IdleDetermineStates.ByPass,
    }
}

export default{
    namespaced: true,
    state: GetInitPageIdle(),
    getters:{
    },
    mutations: {
        Init: (state: IPageIdle) => {
            const initPageIdle = GetInitPageIdle()
            Object.keys(initPageIdle).forEach( key => Reflect.set( state, key, Reflect.get(initPageIdle, key) ) )
        },
        SetStartIdleTimestamp: (state: IPageIdle, timeStamp: Date) => state.startIdleTimestamp = timeStamp,
        SetStartUserConfirmTimestamp: (state: IPageIdle, timeStamp: Date) => state.startUserConfirmTimestamp = timeStamp,
        SetIsShowUserConfirm: (state: IPageIdle, isShowUserConfirm: boolean) => state.isShowUserConfirm = isShowUserConfirm,
        SetIsShowLogOutNotification: (state: IPageIdle, isShowLogOutNotification: boolean) => state.isShowLogOutNotification = isShowLogOutNotification,
        SetIdleTimes: (state: IPageIdle, idleTimes: number) => state.idleTimes = idleTimes,
        SetIdleDetermineStates: ( state: IPageIdle, idleDetermineStates: IdleDetermineStates ) => state.idleDetermineStates = idleDetermineStates,
    },
    actions: {
    },
    modules: {
    }
}