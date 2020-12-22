import { ITwoColumn } from '@/models/twoColumn'
import { Module } from 'vuex/types/index'

const GetInitState = (): ITwoColumn => ({
    isShowSidebar: false,
    isActiveTwoColumn: false,
})

const module: Module<ITwoColumn, any> = {
    namespaced: true,
    state: GetInitState(),
    mutations: {
        Init: ( state ) => {
            const initState = GetInitState()
            Object.keys(initState).forEach( key => Reflect.set( state, key, Reflect.get(initState, key) ) )
        },
        ActiveSidebar: ( state ) => state.isShowSidebar = true,
        DisActiveSidebar: ( state ) => state.isShowSidebar = false,
        ToggleSidebar: ( state ) => state.isShowSidebar = !state.isShowSidebar,
        ActiveTwoColumn: ( state ) => state.isActiveTwoColumn = true,
        DisActiveTwoColumn: ( state ) => state.isActiveTwoColumn = false,
    },
    actions: {
    },
    modules: {
    }
}

export default module