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

export {
    IDayCount,
    IEchartsPieRow,
}