import { WSHandler } from '@/api/webSocket'
import { IQueryTasks } from '@/models/tasks'
import { IWebSocket } from '@/models/webSocket'
import Vue from 'vue'
import { Module } from 'vuex/types/index'

const GetInitState = (): IWebSocket => ({
        wsHandler: undefined,
        taskEditorQueryTasks: undefined,
    })

const module: Module<IWebSocket, any> = {
    namespaced: true,
    state: GetInitState(),
    mutations: {
        Init: ( state ) => {
            const initState = GetInitState()
            Vue.set(state, "wsHandler", initState.wsHandler)
        },
        SetWSHandler: ( state, wsHandler: WSHandler ) => {
            state.wsHandler = wsHandler
        },
        SetTaskEditorGroup: ( state, queryTasks: IQueryTasks ) => {
            Vue.set(state, "taskEditorQueryTasks", queryTasks)
        },
    },
    actions: {
    },
    modules: {
    }
}

export default module