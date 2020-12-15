
interface IAddRow{
    newIndex: number;
}

interface IRemoveRow{
    oldIndex: number;
    value: any;
}

interface IMoveRow{
    newIndex: number;
    oldIndex: number;
}

interface IUpdateCell{
    rowIndex: number;
    relatedKey: string;
    oldValue: any;
    newValue: any;
}

export {
    IAddRow,
    IRemoveRow,
    IMoveRow,
    IUpdateCell,
}