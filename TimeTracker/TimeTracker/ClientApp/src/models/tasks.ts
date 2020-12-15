
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
    guid: string;
    displayOrder: number;
}

interface IUpdateTaskCol{
    guid: string;
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
    IUpdateTask,
    IUpdateTaskRowOrder,
    IUpdateTaskCol,
    IDayData,
    IGetDaysDataResponse,
    IQueryTasks,
    ICreateTask,
}