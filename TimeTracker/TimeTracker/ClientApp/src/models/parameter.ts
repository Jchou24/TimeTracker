import { Ref } from "@vue/composition-api"
import { AxiosResponse } from "axios"

interface ITaskEntityOption{
    guid: string;
    codeName: string; // unique
    displayName: string;
    isActive: boolean;
}

interface IOptionMethods{
    GetOptions: (
            isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void
        ) => void;
    CreateOptions: (
            data: Array<ITaskEntityOption>, 
            isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void
        ) => void
    UpdateOptions: (
        data: Array<ITaskEntityOption>, 
        isLoading: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void
    ) => void
}

export {
    ITaskEntityOption,
    IOptionMethods,
}
