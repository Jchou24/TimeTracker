<template>
    <div class="DatePeriodForm">
        <GeneralCard title="Periods" titleIcon="mdi-calendar-multiple"
            :width="width" :loading="isLoadingPeriods">
            <template v-slot:body>
                <v-card-title class="px-10 pb-2">
                    #Total: {{periods.length}}
                    
                    <v-spacer/>
                    <v-text-field class="mb-4"
                        v-model="search"
                        append-icon="mdi-magnify"
                        label="Search"
                        single-line
                        hide-details
                        />
                </v-card-title>

                <v-data-table class="theme-table px-6" color="primary"
                    v-model="selected"
                    :headers="headers"
                    :items="periods"
                    :search="search"
                    :items-per-page="10"
                    show-select
                    item-key="guid"
                    fixed-header
                    filterable
                    multi-sort
                    no-data-text="No Periods found"
                >
                    <template v-slot:item.actions="{ item }">
                        <v-icon small class="mr-2"
                            @click="HandleStartUpdatePeriod(item)"
                            >
                            mdi-pencil
                        </v-icon>
                        <v-icon small
                            @click="HandleDeletePeriod(item)"
                            >
                            mdi-delete
                        </v-icon>
                    </template>
                </v-data-table>
            </template>
            <template v-slot:action>
                <v-container>
                    <v-row no-gutters>
                        <v-col>
                            <v-btn class="pl-5 pr-6" color="warning" rounded elevation="4" large
                                :loading="isDeletingPeriods"
                                :disabled="isDisabledDeletingPeriods"
                                @click="HandleDeleteSelectedPeriod"
                                >
                                <v-icon left>mdi-delete</v-icon> Delete Selected Periods
                            </v-btn>
                        </v-col>
                        <v-col>
                            <v-row class="justify-end">
                                <DatePeriodFormAdd :modalWidth="modalWidth" @addPeriod="HandleAddPeriod" />
                                <v-btn class="pl-5 pr-6 mr-3" color="primary" rounded elevation="4" large
                                    :loading="isLoadingPeriods"
                                    :disabled="isLoadingPeriods"
                                    @click="InitPeriods"
                                    >
                                    <v-icon left>mdi-reload</v-icon> Reload
                                </v-btn>
                            </v-row>
                        </v-col>
                    </v-row>
                </v-container>                
            </template>
        </GeneralCard>

        <DatePeriodFormUpdate 
            :modalWidth="modalWidth"
            :isOpenModal.sync="isOpenUpdateModal"
            :period.sync="updatePeriod"
            @updatePeriod="HandleUpdatePeriod"
        />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'
    import DatePeriodFormAdd from './DatePeriodFormAdd.vue'
    import DatePeriodFormUpdate from './DatePeriodFormUpdate.vue'

    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { IPeriod } from '@/models/period'
    import { DataTableHeader } from 'vuetify'
    import { FormatDate } from '@/util/taskDate'
    import { PeriodAPIHandler } from '@/api/period'
    import { ToastSuccess, ToastError } from '@/util/notification'
    import { useToast } from "vue-toastification/composition"

    export default defineComponent({
        name: 'DatePeriodForm',
        props:{
            width:{
                type: Number,
            },
            modalWidth:{
                type: Number,
            },
        },
        components:{
            GeneralCard,
            DatePeriodFormAdd,
            DatePeriodFormUpdate,
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const periodAPIHandler = new PeriodAPIHandler( store, router )
            const toast = useToast()

            const selected = ref([] as Array<IPeriod>)
            const search = ref("")
            const headers = [
                { text: "Name", value: "name" },
                { text: "Start Date", value: "startDate" },
                { text: "End Date", value: "endDate" },
                { text: 'Actions', value: 'actions', sortable: false },
            ] as Array<DataTableHeader>
            const isLoadingPeriods = ref(false)
            const isDeletingPeriods = ref(false)
            const isOpenUpdateModal = ref(false)
            const isDisabledDeletingPeriods = computed( () => !(isDeletingPeriods.value || selected.value.length > 0) )
            // =================================================================
            // Init
            function FormatPeriodsDate(periods: Array<IPeriod>){
                periods.forEach( periods => {
                    periods.startDate = FormatDate(periods.startDate)
                    periods.endDate = FormatDate(periods.endDate)
                })
            }

            const periodsData = ref([] as Array<IPeriod>)
            const periods = computed({
                get: () => periodsData.value,
                set: (value) => {
                    FormatPeriodsDate(value)
                    periodsData.value = value
                }
            })                     

            function InitPeriods(){
                periodAPIHandler.GetPeriods(isLoadingPeriods, (response) => {
                    periods.value = response.data as Array<IPeriod>
                })
            }
            InitPeriods()
            
            function HandleAddPeriod(period: IPeriod){
                periods.value = [ period, ...periods.value ]
            }
            // =================================================================
            // Handle Delete Period
            function DeletePeriods(targetPeriods: Array<IPeriod>, isClearSelect: boolean) {
                periodAPIHandler.DeletePeriods(targetPeriods, isDeletingPeriods, () => {
                    const selectedGuids = targetPeriods.map( targetPeriod => targetPeriod.guid )
                    periods.value = periods.value.filter( period => !selectedGuids.includes(period.guid) )
                    ToastSuccess( `Delete ${selectedGuids.length} period(s)`, toast )
                    if (isClearSelect) {
                        selected.value = []
                    }
                }, () => {
                    // TODO Handle delete fail
                })
            }

            const HandleDeleteSelectedPeriod = () => DeletePeriods(selected.value, true)
            const HandleDeletePeriod = (period: IPeriod) => DeletePeriods([period], false)
            // =================================================================

            const updatePeriod = ref({} as IPeriod)
            function HandleStartUpdatePeriod(period: IPeriod){
                updatePeriod.value = period
                isOpenUpdateModal.value = true
            }

            function HandleUpdatePeriod(period: IPeriod) {
                const updatedPeriod = periods.value.find( x => x.guid == period.guid )

                if(!updatedPeriod){
                    return
                }

                updatedPeriod.name = period.name
                updatedPeriod.startDate = period.startDate
                updatedPeriod.endDate = period.endDate                
            }

            return {
                isLoadingPeriods,
                isDeletingPeriods,
                isDisabledDeletingPeriods,
                isOpenUpdateModal,
                selected,
                periods,
                search,
                headers,
                updatePeriod,
                InitPeriods,
                HandleAddPeriod,
                HandleDeleteSelectedPeriod,
                HandleStartUpdatePeriod,
                HandleUpdatePeriod,
                HandleDeletePeriod,
            }
        }        
    })
</script>

<style lang="scss">

</style>