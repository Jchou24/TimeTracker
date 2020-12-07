import axios, { AxiosResponse } from 'axios'
import { Ref } from '@vue/composition-api'

axios.defaults.withCredentials = true

function HttpGet(url: string, isLoading?: Ref<boolean>, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void ){
    if (isLoading) {
        isLoading.value = true
    }

    axios.get(`${process.env.VUE_APP_SERVER_URL}${url}`.toLowerCase() )
        .then( response =>{
            if( SuccessFunc ){
                SuccessFunc(response)
            }
        })
        .catch( error => {
            if( ErrorFunc ){
                ErrorFunc(error)
            }
        })
        .finally( ()=>{
            if (isLoading) {
                isLoading.value = false
            }
        })
}

function HttpPost(url: string, isLoading?: Ref<boolean>, data?: any, 
        SuccessFunc?: (response: AxiosResponse<any>) => void, 
        ErrorFunc?: (error: any) => void ){
    if (isLoading) {
        isLoading.value = true
    }
    
    axios.post(`${process.env.VUE_APP_SERVER_URL}${url}`.toLowerCase(), data )
        .then( response =>{
            if( SuccessFunc ){
                SuccessFunc(response)
            }
        })
        .catch( error => {
            if( ErrorFunc ){
                ErrorFunc(error)
            }
        })
        .finally( ()=>{
            if (isLoading) {
                isLoading.value = false
            }
        })
}

export {
    HttpGet,
    HttpPost
}