import { WSHandler } from '@/api/webSocket';
import { IQueryTasks } from './tasks';

interface IWebSocket{
    wsHandler: WSHandler | undefined;
    taskEditorQueryTasks: IQueryTasks | undefined;
}

export {
    IWebSocket
}