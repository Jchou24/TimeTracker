import moment from 'moment'

const dateFormat = "YYYY-MM-DD"

function GetYesterday() {
    return moment().subtract(1, 'days').format(dateFormat)   
}

function GetToday(){
    return moment().format(dateFormat);
}

function ValidateDate(value: string | undefined){
    return moment(value, dateFormat, true).isValid() || `Please input ${dateFormat}`
}

function FormatDate(value: string, format = dateFormat) {
    return moment(value).isValid() ? moment(value).format(format) : ""
}

function GetDateRange(startDate: string | undefined, endDate: string | undefined): Array<string> {
    if( ValidateDate(startDate) !== true || ValidateDate(endDate) !== true ){
        return []
    }

    const dates = []
    const currentDate = moment(startDate).add(-1, 'days').startOf('day')
    const lastDate = moment(endDate).add(1, 'days').startOf('day')

    while(currentDate.add(1, 'days').diff(lastDate) < 0) {
        dates.push(currentDate.clone().format(dateFormat))
    }

    return dates
}

export {
    dateFormat,
    GetYesterday,
    GetToday,
    ValidateDate,
    FormatDate,
    GetDateRange,
}
