import { ITaskOption } from "@/models/tasks"

const GetOptionIdByOption = ( storeTasksOptions: Array<ITaskOption>, value: ITaskOption ) => storeTasksOptions.find( x => x.guid === value.guid )?.id
const GetOptionIdByGuid = ( storeTasksOptions: Array<ITaskOption>, value: string ) => storeTasksOptions.find( x => x.guid === value )?.id
const GetOptionById = ( storeTasksOptions: Array<ITaskOption>, value: number ) => storeTasksOptions.find( x => x.id === value )
const GetOptionGuidById = ( storeTasksOptions: Array<ITaskOption>, value: number ) => storeTasksOptions.find( x => x.id === value )?.guid

export {
    GetOptionIdByOption,
    GetOptionIdByGuid,
    GetOptionById,
    GetOptionGuidById,
}