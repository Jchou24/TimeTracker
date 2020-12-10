
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


export {
    IDateRange,
    IPeriod,
}