<template>
    <div class="DateMenuSelector">

        <DatePeriodSelector :isOpenModal.sync="isOpenDateRangeSelector" :width="width" 
            @updateDateRange="HandleUpdateDateRange" />
            
        <v-menu class="pl-4" v-model="isOpenMenu"
            :nudge-right="20"
            rounded="xl"
            offset-x
            disable-keys
            transition="slide-x-transition"
            :close-on-content-click="false">

            <template v-slot:activator="activator">
                <slot name="activator" v-bind:activator="activator"></slot>
            </template>

            <v-list>
                <v-list-item>
                    <v-list-item-title>
                        <v-btn  class="mt-3 mb-4 text-capitalize" color=""
                            rounded block 
                            @click="isOpenDateRangeSelector = true; isOpenMenu = false">
                            <v-icon left>mdi-magnify</v-icon> Search Period
                        </v-btn>
                    </v-list-item-title>
                </v-list-item>

                <v-list-item>
                    <v-list-item-title>
                        <DateRangeSelector class="mr-1 mt-3 mb-n3" 
                            :defaultStartDate="defaultStartDate" 
                            :defaultEndDate="defaultEndDate" 
                            @updateDateRange="HandleUpdateDateRange" />
                    </v-list-item-title>
                </v-list-item>
            </v-list>
        </v-menu>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import DatePeriodSelector from './DatePeriodSelector.vue'
    import DateRangeSelector from './DateRangeSelector.vue'

    import { Store } from 'vuex/types/index'
    import { Dictionary } from 'vue-router/types/router'
    import { IStore } from '@/models/store'
    import { IDateRange } from '@/models/tasks'
    import { TaskEditorAPIHandler } from '@/api/taskEditor'
    import { GetToday, ValidateDate } from '@/util/taskDate'

    export default defineComponent({
        name: 'DateMenuSelector',
        props:{
            selectedDates:{
                type: Object as () => IDateRange,
            },
            width:{
                type: Number,
                default: 500,
            }
        },
        components:{
            DatePeriodSelector,
            DateRangeSelector,
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )

            const queryStringKey = {
                startDate: "std",
                endDate: "ed"
            }

            const isLoading = ref(false)
            const isOpenMenu = ref(false)
            const isOpenDateRangeSelector = ref(false)

            // =================================================================
            // HandleUpdateDateRange
            function UpdateQueryString(dateRange: IDateRange){
                
                let query = {} as Dictionary<string>
                if( ValidateDate(dateRange.startDate) ){
                    query[queryStringKey.startDate] = dateRange.startDate
                }

                if( ValidateDate(dateRange.endDate) ){
                    query[queryStringKey.endDate] = dateRange.endDate
                }

                if( Object.keys(query).length > 0 ){
                    query = {
                        ...$route.query,
                        ...query
                    } as Dictionary<string>
                    $router.replace({ query }).catch(()=>{query})
                }
            }

            function HandleUpdateDateRange(dateRange: IDateRange){
                emit("update:selectedDates", dateRange)
                UpdateQueryString(dateRange)
            }
            // =================================================================
            // Handle init
            const defaultStartDate = ref("")            
            const defaultEndDate = ref("")
            function InitDefaultDate(){
                const ValidateQueryString = (key: string) => $route.query[key] && ValidateDate($route.query[key] as string)

                defaultStartDate.value = ValidateQueryString(queryStringKey.startDate) ?
                    $route.query[queryStringKey.startDate] as string : GetToday()

                defaultEndDate.value = ValidateQueryString(queryStringKey.endDate) ?
                    $route.query[queryStringKey.endDate] as string : GetToday()

                HandleUpdateDateRange({
                    periodName: "",
                    isUsedPeriod: false,
                    startDate: defaultStartDate.value,
                    endDate: defaultEndDate.value,
                } as IDateRange)
            }
            InitDefaultDate()
            // =================================================================
            

            return {
                isLoading,
                isOpenMenu,
                isOpenDateRangeSelector,

                defaultStartDate,
                defaultEndDate,

                HandleUpdateDateRange,
            }
        }        
    })
</script>

<style lang="scss">

</style>