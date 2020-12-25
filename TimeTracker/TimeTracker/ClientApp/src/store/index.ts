import Vue from 'vue'
import Vuex, { Store } from 'vuex'

import createMultiTabState from 'vuex-multi-tab-state'

import { IStore, IStoreRoot } from '@/models/store'
import authentication from './authentication'
import pageIdle from './pageIdle'
import taskParameters from './taskParameters'
import notification from './notification'
import webSocket from './webSocket'
import twoColumn from './twoColumn'
import { GetGuid } from '@/util/authentication'

Vue.use(Vuex)

export default new Vuex.Store<IStoreRoot>({
    state: {
        isLoading: false,
        tabGuid: GetGuid(),
        userImage: "",
    },
    mutations: {
        TurnOnLoading: ( state ) => {
            state.isLoading = true 
        },
        TurnOffLoading: ( state ) => {
            state.isLoading = false 
        },
        SetUserImage: ( state, userImage: string ) => state.userImage = userImage
    },
    actions: {
    },
    modules: {
        taskParameters,
        authentication,
        pageIdle,
        notification,
        webSocket,
        twoColumn,
    },
    plugins: [
        createMultiTabState({
            statesPaths: ['authentication', 'pageIdle', 'userImage']
        }),
    ],
}) as Store<IStore>
