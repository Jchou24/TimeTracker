import { PluginOptions, ToastOptions } from "vue-toastification/dist/types/src/types"
import ToastInterface from "vue-toastification/dist/types/src/ts/interface"
import { TYPE, POSITION } from "vue-toastification/dist/types/src/ts/constants";

function ToastSuccess(message: string, toast?: ReturnType<typeof ToastInterface>, option?: (ToastOptions & {
    type?: TYPE.SUCCESS | undefined;
}) | undefined){
    const mergedOption = {
        icon: {
            iconClass: 'material-icons',  
            iconChildren: 'check_circle_outline', 
            iconTag: 'span'   
        },
        timeout: 7 * 1000,
        ...option
    } as PluginOptions
    return toast?.success(message, mergedOption)
}

function ToastWarning(message: string, toast?: ReturnType<typeof ToastInterface>, option?: (ToastOptions & {
    type?: TYPE.WARNING | undefined;
}) | undefined){
    const mergedOption = {
        icon: {
            iconClass: 'material-icons',  
            iconChildren: 'warning', 
            iconTag: 'span'   
        },
        timeout: 7 * 1000,
        ...option
    } as PluginOptions
    return toast?.warning(message, mergedOption)
}

function ToastError(message: string, toast?: ReturnType<typeof ToastInterface>, option?: (ToastOptions & {
    type?: TYPE.WARNING | undefined;
}) | undefined){
    const mergedOption = {
        icon: {
            iconClass: 'material-icons',  
            iconChildren: 'error', 
            iconTag: 'span'   
        },
        timeout: 7 * 1000,
        ...option
    } as PluginOptions
    return toast?.error(message, mergedOption)
}

export {
    ToastSuccess,
    ToastWarning,
    ToastError,
}
