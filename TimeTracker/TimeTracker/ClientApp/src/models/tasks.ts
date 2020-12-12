
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


export {
    IDateRange,
    IPeriod,
    ITaskOption,
    ITaskParameters,
}