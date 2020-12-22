import moment from "moment"
import { ValidateDate } from "./taskDate"

function IsDisabledSubmit(name: string, startDate: string, endDate: string, isLoading: boolean) {
    if( name.length == 0 || startDate.length == 0 || endDate.length == 0) {
        return true
    }

    if( ValidateDate(startDate) !== true || ValidateDate(endDate) !== true ) {
        return true
    }

    if( moment(startDate) > moment(endDate) ){
        return true
    }

    if( isLoading ){
        return true
    }

    return false
}

export {
    IsDisabledSubmit
}