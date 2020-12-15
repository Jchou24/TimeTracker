import FormSettings from 'jctk-table-form/src/components/ShareVar'

import { ITaskOption, ITaskParameters } from "../tasks";

const TransformedOptions = ( taskOptions: Array<ITaskOption> ) => 
    taskOptions.map( taskOption => ({ name: taskOption.displayName, value: taskOption.id }) ) || []

const GetOptions = ( taskParameters: ITaskParameters ) =>({
    head:[{
            title: "時數",
            relatedKey: "consumeTime",
            cellType: FormSettings.cellTypes.number /*as string*/,
            style:{
                "width": "80px",
                "min-width": "80px",
            },
            options:{
                step: 0.5
            },
        },{
            title: "工作類型",
            relatedKey: "taskType",
            cellType: FormSettings.cellTypes.singleSelect  /*as string*/,
            style:{
                width: "160px",
                "min-width": "160px",
            },
            options: TransformedOptions(taskParameters.taskTypes),
        },{
            title: "工作來源",
            relatedKey: "taskSource",
            cellType: FormSettings.cellTypes.singleSelect  /*as string*/,
            style:{
                width: "160px",
                "min-width": "160px",
            },
            options: TransformedOptions(taskParameters.taskSources),
        },{
            title: "工作名稱",
            relatedKey: "taskName",
            cellType: FormSettings.cellTypes.textarea  /*as string*/,
            options:{
                maxLength: 256,
                isSuggestions: true,
                throttle: 250,
            },
        },{
            title: "工作內容",
            relatedKey: "taskContent",
            cellType: FormSettings.cellTypes.textarea  /*as string*/,
            options:{
                maxLength: 256,
                isSuggestions: true,
                throttle: 250,
            },
        }]
})

export {
    GetOptions
}