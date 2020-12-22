declare module 'jctk-table-form/src/components/ShareVar'{
    interface ICellTypes{
        number: string;
        singleSelect: string;
        textarea: string;
    }

    interface IFormSettings{
        cellTypes: ICellTypes;
    }

    declare let FormSettings:IFormSettings

    export default FormSettings
}