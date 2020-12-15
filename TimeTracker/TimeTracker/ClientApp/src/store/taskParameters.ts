import Vue from 'vue'
import { ITaskOption, ITaskParameters } from '@/models/tasks';
import { Module } from "vuex";

const TASKTYPES = "taskTypes"
const TASKSOURCES = "taskSources"

const GetInitState = (): ITaskParameters => ({
        taskTypes: [],
        taskSources: [],
    })

const SetOptions = ( state: ITaskParameters, tasktype: string, taskOptions: Array<ITaskOption> ) => {
    taskOptions.sort( ( x, y ) => { 
        if (x.displayName < y.displayName) {
            return -1
        }
        if (x.displayName > y.displayName) {
            return 1
        }
        return 0
    }).forEach( (option, idx) => option.id = idx )
    Vue.set(state, tasktype, taskOptions)
}

const module: Module<any, any> = {
    namespaced: true,
    state: GetInitState(),
    mutations: {
        Init: ( state: ITaskParameters ) => {
            const initState = GetInitState()
            Vue.set(state, TASKTYPES, initState.taskTypes)
            Vue.set(state, TASKSOURCES, initState.taskSources)
        },
        SetTaskTypes: ( state: ITaskParameters, taskTypes: Array<ITaskOption> ) => SetOptions(state, TASKTYPES, taskTypes),
        SetTaskSources: ( state: ITaskParameters, taskSources: Array<ITaskOption> ) => SetOptions(state, TASKSOURCES, taskSources),
    },
    actions: {
    },
    modules: {
    }
}

export default module