import { INotification } from '@/models/notification';
import { ToastWarning } from '@/util/notification';
import debounce from 'lodash.debounce';
import Vue from 'vue'
import ToastInterface from 'vue-toastification/dist/types/src/ts/interface';
import { Module } from "vuex";

const GetInitState = (): INotification => ({
        notificator: undefined,
        Notificate401: undefined, 
        Notificate403: undefined, 
    })

function SetNotificate401(state: INotification){
    state.Notificate401 = debounce( () => ToastWarning( `Sorry! You don't have sufficient permission. You will be signed out automatically.`, state.notificator ), 1000 )
}

function SetNotificate403(state: INotification){
    state.Notificate403 = debounce( () => ToastWarning( `Sorry! You don't have sufficient permission.`, state.notificator ), 1000 )
}

const module: Module<any, any> = {
    namespaced: true,
    state: GetInitState(),
    mutations: {
        Init: ( state: INotification ) => {
            const initState = GetInitState()
            Vue.set(state, "notificator", initState.notificator)
        },
        SetNotificator: ( state: INotification, notificator: ReturnType<typeof ToastInterface> ) => {
            state.notificator = notificator
            SetNotificate401( state )
            SetNotificate403( state )
        },
    },
    actions: {
    },
    modules: {
    }
}

export default module