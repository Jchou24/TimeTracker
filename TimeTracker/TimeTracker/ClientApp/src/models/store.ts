import { IAuthentication } from './authentication'
import { INotification } from './notification'
import { IPageIdle } from './pageIdle'
import { ITaskParameters } from './tasks'
import { ITwoColumn } from './twoColumn'
import { IWebSocket } from './webSocket'


interface IStoreRoot{
    isLoading: boolean,
    tabGuid: string,
    userImage: string,
}

interface IStore extends IStoreRoot{
    authentication: IAuthentication,
    pageIdle: IPageIdle,
    taskParameters: ITaskParameters,
    notification: INotification,
    webSocket: IWebSocket,
    twoColumn: ITwoColumn,
}

export {
    IStoreRoot,
    IStore,
}