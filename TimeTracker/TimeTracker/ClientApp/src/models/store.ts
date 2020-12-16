import { IAuthentication } from './authentication'
import { INotification } from './notification'
import { IPageIdle } from './pageIdle'
import { ITaskParameters } from './tasks'
import { IWebSocket } from './webSocket'


interface IStoreRoot{
    isLoading: boolean,
    tabGuid: string,
}

interface IStore extends IStoreRoot{
    authentication: IAuthentication,
    pageIdle: IPageIdle,
    taskParameters: ITaskParameters,
    notification: INotification,
    webSocket: IWebSocket,
}

export {
    IStoreRoot,
    IStore,
}