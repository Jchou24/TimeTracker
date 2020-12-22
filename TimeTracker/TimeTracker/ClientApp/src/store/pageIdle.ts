import { IdleDetermineStates, IPageIdle } from '@/models/pageIdle'
import { Module } from 'vuex/types/index'

const GetInitPageIdle = (): IPageIdle => ({
        startIdleTimestamp: undefined,
        startUserConfirmTimestamp: undefined,
        isShowUserConfirm: false,
        isShowLogOutNotification: false,
        idleTimes: 0,
        idleDetermineStates: IdleDetermineStates.ByPass,
    })

const module: Module<IPageIdle, any> = {
    namespaced: true,
    state: GetInitPageIdle(),
    getters:{
    },
    mutations: {
        Init: (state) => {
            const initPageIdle = GetInitPageIdle()
            Object.keys(initPageIdle).forEach( key => Reflect.set( state, key, Reflect.get(initPageIdle, key) ) )
        },
        SetStartIdleTimestamp: (state, timeStamp: Date) => state.startIdleTimestamp = timeStamp,
        SetStartUserConfirmTimestamp: (state, timeStamp: Date) => state.startUserConfirmTimestamp = timeStamp,
        SetIsShowUserConfirm: (state, isShowUserConfirm: boolean) => state.isShowUserConfirm = isShowUserConfirm,
        SetIsShowLogOutNotification: (state, isShowLogOutNotification: boolean) => state.isShowLogOutNotification = isShowLogOutNotification,
        SetIdleTimes: (state, idleTimes: number) => state.idleTimes = idleTimes,
        SetIdleDetermineStates: ( state, idleDetermineStates: IdleDetermineStates ) => state.idleDetermineStates = idleDetermineStates,
    },
    actions: {
    },
    modules: {
    }
}

export default module