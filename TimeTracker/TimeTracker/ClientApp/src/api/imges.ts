import { IStore } from "@/models/store"
import { APIHandler } from "@/util/apiHandler"
import { Ref } from "@vue/composition-api"
import { AxiosResponse } from "axios"
import VueRouter from "vue-router"
import { Store } from "vuex/types/index"


class ImageAPIHandler extends APIHandler{
    
    constructor( store: Store<IStore>, router: VueRouter ) {
        super(store, router)
    }

    GetImage(isLoading?: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Image/GetImage', isLoading, undefined, (response) => {
            this._store.commit("SetUserImage", response.data)
            if (SuccessFunc) {
                SuccessFunc(response)
            }
        }, ErrorFunc)
    }

    CreateOrUpdateImage(data: string, isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Image/CreateOrUpdateImage', isLoading, { image: data }, (response) => {
            this._store.commit("SetUserImage", data)
            if (SuccessFunc) {
                SuccessFunc(response)
            }
        }, ErrorFunc)
    }

    DeleteImage(isLoading: Ref<boolean>, 
            SuccessFunc?: (response: AxiosResponse<any>) => void, 
            ErrorFunc?: (error: any) => void){
        this.HttpPost('api/Image/DeleteImage', isLoading, undefined, (response) => {
            this._store.commit("SetUserImage", "")
            if (SuccessFunc) {
                SuccessFunc(response)
            }
        }, ErrorFunc)
    }
}

export {
    ImageAPIHandler
}