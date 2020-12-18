
interface IDateRange{
    periodName: string;
    isUsedPeriod: boolean;
    startDate: string;
    endDate: string;
}

interface IPeriod{
    guid: string;
    startDate: string;
    endDate: string;
    name: string;
}

interface ITaskOption{
    id: number;
    guid: string;
    codeName: string;
    displayName: string;
}

interface ITaskParameters{
    taskTypes: Array<ITaskOption>;
    taskSources: Array<ITaskOption>;
    dayWorkLimitTime: number | undefined;
}

interface ITask{
    guid: string;
    displayOrder: number;
    date: string;
    consumeTime: number;
    taskType: number;
    taskSource: number;
    taskName: string;
    taskContent: string;
}

interface IDeleteTasks{
    ownerGuid: string;
    date: string;
    taskGuids: Array<string>;
}

interface IUpdateIsLeave{
    ownerGuid: string;
    date: string;
    isLeave: boolean;
}

interface IUpdateTask{
    guid: string;
    displayOrder: number;
    date: string;
    consumeTime: number;
    taskType: ITaskOption;
    taskSource: ITaskOption;
    taskName: string;
    taskContent: string;
}

interface IUpdateTaskRowOrder{
    ownerGuid: string;
    guid: string; // task guid
    date: string;
    displayOrder: number;
}

interface IUpdateTaskCol{
    ownerGuid: string;
    guid: string; // task guid
    date: string;
    relatedKey: string;
    value: any;
}

interface IGetDaysDataResponseTask{
    guid: string;
    displayOrder: number;
    date: string;
    consumeTime: number;
    taskType: ITaskOption | number;
    taskSource: ITaskOption | number;
    taskName: string;
    taskContent: string;
}

interface IDayData{
    guid: string;
    date: string;
    isLeave: boolean;
    isFormClicked: boolean;
    formData: Array<ITask>;
}

interface IGetDaysDataResponse{
    guid: string;
    date: string;
    isLeave: boolean;
    isFormClicked: boolean;
    formData: Array<IGetDaysDataResponseTask>;
}

interface IQueryTasks{
    ownerGuid: string;
    startDate: string;
    endDate: string;
}

interface ICreateTask{
    tasks: Array<IUpdateTask>;
    ownerGuid: string;
}

export {
    IDateRange,
    IPeriod,
    ITaskOption,
    ITaskParameters,
    ITask,
    IDeleteTasks,
    IUpdateIsLeave,
    IUpdateTask,
    IUpdateTaskRowOrder,
    IUpdateTaskCol,
    IDayData,
    IGetDaysDataResponse,
    IQueryTasks,
    ICreateTask,
}