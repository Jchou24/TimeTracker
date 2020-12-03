import Vue from 'vue'
import Vuex from 'vuex'

import createMultiTabState from 'vuex-multi-tab-state'

import authentication from './authentication'
import pageIdle from './pageIdle'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
    },
    mutations: {
    },
    actions: {
    },
    modules: {
        authentication,
        pageIdle,
    },
    plugins: [
        createMultiTabState(),
    ],
})
