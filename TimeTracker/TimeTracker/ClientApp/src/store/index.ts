import Vue from 'vue'
import Vuex, { Store } from 'vuex'

import createMultiTabState from 'vuex-multi-tab-state'

import { IStore, IStoreRoot } from '@/models/store'
import authentication from './authentication'
import pageIdle from './pageIdle'
import ToastInterface from 'vue-toastification/dist/types/src/ts/interface'
import { WSHandler } from '@/api/webSocket'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        isLoading: false,
        notificator: undefined,
        wsHandler: undefined,
    },
    mutations: {
        TurnOnLoading: ( state: IStoreRoot ) => {
            state.isLoading = true 
        },
        TurnOffLoading: ( state: IStoreRoot ) => {
            state.isLoading = false 
        },
        SetNotificator: ( state: IStoreRoot, notificator: ReturnType<typeof ToastInterface> ) => {
            state.notificator = notificator
        },
        SetWSHandler: ( state: IStoreRoot, wsHandler: WSHandler ) => {
            state.wsHandler = wsHandler
        },
    },
    actions: {
    },
    modules: {
        authentication,
        pageIdle,
    },
    plugins: [
        createMultiTabState({
            statesPaths: ['authentication', 'pageIdle']
        }),
    ],
}) as Store<IStore>
