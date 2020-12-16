import Vue from 'vue'
import Vuex, { Store } from 'vuex'

import createMultiTabState from 'vuex-multi-tab-state'

import { IStore, IStoreRoot } from '@/models/store'
import authentication from './authentication'
import pageIdle from './pageIdle'
import taskParameters from './taskParameters'
import notification from './notification'
import webSocket from './webSocket'
import { GetGuid } from '@/util/authentication'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        isLoading: false,
        tabGuid: GetGuid(),
    },
    mutations: {
        TurnOnLoading: ( state: IStoreRoot ) => {
            state.isLoading = true 
        },
        TurnOffLoading: ( state: IStoreRoot ) => {
            state.isLoading = false 
        },
    },
    actions: {
    },
    modules: {
        taskParameters,
        authentication,
        pageIdle,
        notification,
        webSocket,
    },
    plugins: [
        createMultiTabState({
            statesPaths: ['authentication', 'pageIdle']
        }),
    ],
}) as Store<IStore>
