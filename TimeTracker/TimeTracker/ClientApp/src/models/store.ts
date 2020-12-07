import ToastInterface from 'vue-toastification/dist/types/src/ts/interface'
import { IAuthentication } from './authentication'
import { IPageIdle } from './pageIdle'


interface IStoreRoot{
    isLoading: boolean,
    notificator: ReturnType<typeof ToastInterface> | undefined,
}

interface IStore extends IStoreRoot{
    authentication: IAuthentication,
    pageIdle: IPageIdle,
}

export {
    IStoreRoot,
    IStore,
}