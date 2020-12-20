import { INotification } from '@/models/notification';
import { ToastSuccess, ToastWarning } from '@/util/notification';
import debounce from 'lodash.debounce';
import Vue from 'vue'
import ToastInterface from 'vue-toastification/dist/types/src/ts/interface';
import { Module } from "vuex";

const GetInitState = (): INotification => ({
        notificator: undefined,
        Notificate401: undefined, 
        Notificate403: undefined, 
        NotificateWSReconnected: undefined,
        NotificateWSReconnecting: undefined,
        NotificateWSClose: undefined,
    })

const GetDebounceNoter = ( state: INotification, message: string, debouncTime = 1000 ) => 
    debounce( () => ToastWarning( message, state.notificator ), debouncTime )

const SetNotificate401 = (state: INotification) =>
    state.Notificate401 = GetDebounceNoter( state, `Sorry! You don't have sufficient permission. You will be signed out automatically.`)

const SetNotificate403 = (state: INotification) =>
    state.Notificate403 = GetDebounceNoter( state, `Sorry! You don't have sufficient permission.`)

const SetNotificateWSReconnected = (state: INotification) =>
    state.NotificateWSReconnected = debounce( () => ToastSuccess( `The connection is restored successfully.`, state.notificator ), 3000 )

const SetNotificateWSReconnecting = (state: INotification) =>
    state.NotificateWSReconnecting = GetDebounceNoter( state, `Sorry! The connection is unstable now. Please wait for the connection.`, 3000)

const SetNotificateWSClose = (state: INotification) =>
    state.NotificateWSClose = GetDebounceNoter( state, `Sorry! The connection is break unpredictably. Please refresh the browser to continue.`, 3000)

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
            SetNotificateWSReconnected( state )
            SetNotificateWSReconnecting( state )
            SetNotificateWSClose( state )
        },
    },
    actions: {
    },
    modules: {
    }
}

export default module