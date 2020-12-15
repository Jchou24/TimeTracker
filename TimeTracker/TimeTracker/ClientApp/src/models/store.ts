import { WSHandler } from '@/api/webSocket'
import { IAuthentication } from './authentication'
import { INotification } from './notification'
import { IPageIdle } from './pageIdle'
import { ITaskParameters } from './tasks'


interface IStoreRoot{
    isLoading: boolean,
    wsHandler: WSHandler | undefined,
    tabGuid: string,
}

interface IStore extends IStoreRoot{
    authentication: IAuthentication,
    pageIdle: IPageIdle,
    taskParameters: ITaskParameters,
    notification: INotification,
}

export {
    IStoreRoot,
    IStore,
}