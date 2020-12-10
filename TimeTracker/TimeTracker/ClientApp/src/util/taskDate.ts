import moment from 'moment'

const dateFormat = "YYYY-MM-DD"

function GetYesterday() {
    return moment().subtract(1, 'days').format(dateFormat)
    
}

function GetToday(){
    return moment().format(dateFormat);
}

function ValidateDate(value: string){
    return moment(value, dateFormat, true).isValid() || `Please input ${dateFormat}`
}

function FormatDate(value: string) {
    return moment(value).isValid() ? moment(value).format(dateFormat) : ""
}

export {
    dateFormat,
    GetYesterday,
    GetToday,
    ValidateDate,
    FormatDate,
}
