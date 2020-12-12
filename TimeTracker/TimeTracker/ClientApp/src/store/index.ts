import Vue from 'vue'
import Vuex, { Store } from 'vuex'

import createMultiTabState from 'vuex-multi-tab-state'

import { IStore, IStoreRoot } from '@/models/store'
import authentication from './authentication'
import pageIdle from './pageIdle'
import taskParameters from './taskParameters'
import notification from './notification'
import { WSHandler } from '@/api/webSocket'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        isLoading: false,
        wsHandler: undefined,
    },
    mutations: {
        TurnOnLoading: ( state: IStoreRoot ) => {
            state.isLoading = true 
        },
        TurnOffLoading: ( state: IStoreRoot ) => {
            state.isLoading = false 
        },
        SetWSHandler: ( state: IStoreRoot, wsHandler: WSHandler ) => {
            state.wsHandler = wsHandler
        },
    },
    actions: {
    },
    modules: {
        taskParameters,
        authentication,
        pageIdle,
        notification,
    },
    plugins: [
        createMultiTabState({
            statesPaths: ['authentication', 'pageIdle']
        }),
    ],
}) as Store<IStore>
