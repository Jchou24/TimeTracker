<template>
    <div class="DateRangeSelector">
        <DatePicker label="Start Date" prependIcon="mdi-arrow-up-bold-outline" :date.sync="startDate" />
        <DatePicker label="End Date" prependIcon="mdi-arrow-down-bold-outline" :date.sync="endDate" />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import DatePicker from "./DatePicker.vue"

    import { GetToday, ValidateDate } from '@/util/taskDate'
    import { IDateRange } from '@/models/tasks'

    export default defineComponent({
        name: 'DateRangeSelector',
        props:{
            defaultStartDate:{
                type: String
            },
            defaultEndDate:{
                type: String
            },
        },
        components:{
            DatePicker
        },
        setup( props, { emit, root } ){
            

            function GetDefaultDate(date: string | undefined){
                return date && ValidateDate(date) === true ? date : GetToday()
            }

            const startDate = ref( GetDefaultDate(props.defaultStartDate) )
            const endDate = ref( GetDefaultDate(props.defaultEndDate) )
            const dateRange = computed( () => ({
                    periodName: "",
                    isUsedPeriod: false,
                    startDate: startDate.value,
                    endDate: endDate.value,
                } as IDateRange))

            watch( dateRange, () => {
                emit( "updateDateRange", dateRange.value )
            })

            return {
                startDate,
                endDate,
            }
        }        
    })
</script>

<style lang="scss">

</style>