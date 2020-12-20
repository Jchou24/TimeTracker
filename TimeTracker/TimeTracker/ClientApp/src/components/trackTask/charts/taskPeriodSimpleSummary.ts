import { computed, Ref, ref, watch } from "@vue/composition-api";

import { Store } from "vuex/types/index";
import { IStore } from "@/models/store";
import { IDateRange, IDayData } from "@/models/tasks";
import { GetDateRange } from "@/util/taskDate";
import { IDayCount } from "@/models/charts";

const GetTotal = ( summary: Ref<Array<IDayCount>> ) => computed( () => ({
    date: "",
    consumeTime: summary.value.map( x => x.consumeTime ).reduce((a, b) => a + b, 0),
    overtime: summary.value.map( x => x.overtime ).reduce((a, b) => a + b, 0),
} as IDayCount))

const GetClass = (dayCount: IDayCount) => ({
    inactive: dayCount && !dayCount.isLeave && dayCount.consumeTime == 0,
    overTime: dayCount && !dayCount.isLeave && dayCount.consumeTime > 0 && dayCount.overtime > 0,
    leave: dayCount && dayCount.isLeave,
})

const GetOverTime = (store: Store<IStore>) => computed( () => store.state.taskParameters.dayWorkLimitTime || 7.5 )

function Summary(store: Store<IStore>, selectedDates: Ref<IDateRange>, Executor: () => Array<IDayCount>, WatchData: () => any ) {
    const summary = ref([] as Array<IDayCount>)
    const total = GetTotal(summary)    
    
    const isShowContent = ref(false)

    let currentStartDate = selectedDates.value.startDate
    let currentEndDate = selectedDates.value.endDate
    function InitSummary() {
        if (currentStartDate !== selectedDates.value.startDate || currentEndDate !== selectedDates.value.endDate) {
            isShowContent.value = false
            currentStartDate = selectedDates.value.startDate
            currentEndDate = selectedDates.value.endDate
        }

        summary.value = Executor()

        setTimeout( () => isShowContent.value = true, 150)
    }
    InitSummary()

    watch( WatchData, () => {
        InitSummary()
    })

    return {
        summary,
        total,
        isShowContent,
        InitSummary,
        GetClass,
    }
}

function UseReactiveSummary( store: Store<IStore>, daysData: Ref<Array<IDayData>>, selectedDates: Ref<IDateRange> ){
    return Summary(store, selectedDates, () => {
        const overTime = GetOverTime(store)
        const tmpSummary = daysData.value.map( dayData => {
            const dayCount = {
                date: dayData.date,
                consumeTime: dayData.isLeave ? overTime.value : 0,
                overtime: 0,
                isLeave: dayData.isLeave,
            } as IDayCount

            if (!dayData.isLeave) {
                dayData.formData.forEach( task => dayCount.consumeTime += task.consumeTime )
                dayCount.overtime = dayCount.consumeTime - overTime.value
                dayCount.overtime = dayCount.overtime < 0 ? 0 : dayCount.overtime
            }
            
            return dayCount
        })
        return tmpSummary
    }, () => daysData.value)
}

function UseDirectiveSummary( store: Store<IStore>, sourceSummary: Ref<Array<IDayCount>>, selectedDates: Ref<IDateRange> ){
    return Summary(store, selectedDates, () => {
        const orderedMapSummary = new Map(GetDateRange(selectedDates.value.startDate, selectedDates.value.endDate).map( date => [ date, {
            date: date,
            consumeTime: 0,
            overtime: 0,
            isLeave: false,
        } as IDayCount ]))
        sourceSummary.value.forEach( sourceData => orderedMapSummary.set(sourceData.date, sourceData) )
        return Array.from(orderedMapSummary.values())
    }, () => [ sourceSummary.value, selectedDates.value ])
}

export {
    UseReactiveSummary,
    UseDirectiveSummary,
}