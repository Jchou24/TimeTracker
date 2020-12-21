interface IDayCount{
    date: string;
    consumeTime: number;
    overtime: number;
    isLeave: boolean;
}

interface IEchartsPieRow{
    name: string;
    value: number;
}

interface ITaskTimeSummaryDetail{
    date: string;
    totalConsumeTime: number;
    totalOvertime: number;
    totalLeave: number;
    avgConsumeTime: number;
    avgOvertime: number;
    medianConsumeTime: number;
    medianOvertime: number;
    minConsumeTime: number;
    minOvertime: number;
    maxConsumeTime: number;
    maxOvertime: number;
}

export {
    IDayCount,
    IEchartsPieRow,
    ITaskTimeSummaryDetail,
}