<template>
    <div class="DatePeriodSelector">
        <v-dialog :width="width"
            v-model="isOpenModalRef"
            >
            <v-card
                elevation="10"
                :width="width"
                :loading="isLoading"
                outlined
                shaped
                >
                <v-card-title class="px-10 pb-2">
                    #Total: {{periods.length}}
                    <v-spacer/>
                    <v-text-field 
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
                    :loading="isLoading"
                    :single-select="true"
                    show-select
                    item-key="guid"
                    fixed-header
                    filterable
                    multi-sort
                    no-data-text="No User found"
                    @item-selected="HandleItemSelected"
                />
            </v-card>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import { TaskEditorAPIHandler } from '@/api/taskEditor'
    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { IPeriod, IDateRange } from '@/models/tasks'
    import { DataTableHeader } from 'vuetify'
    import { FormatDate } from '@/util/taskDate'

    export default defineComponent({
        name: 'DatePeriodSelector',
        props:{
            isOpenModal:{
                type: Boolean,
            },
            width:{
                type: Number,
            }
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )


            const isOpenModalRef = computed({
                get: () => props.isOpenModal,
                set: (value) => emit("update:isOpenModal", value)
            })
            
            const selected = ref([] as Array<IPeriod>)            
            // =================================================================
            // Init
            const isLoading = ref(false)
            const periodsData = ref([] as Array<IPeriod>)

            function FormatPeriodsDate(periods: Array<IPeriod>){
                periods.forEach( periods => {
                    periods.startDate = FormatDate(periods.startDate)
                    periods.endDate = FormatDate(periods.endDate)
                })
            }

            const periods = computed({
                get: () => periodsData.value,
                set: (value) => {
                    FormatPeriodsDate(value)
                    periodsData.value = value
                }
            })            

            function InitPeriods(){
                taskEditorAPIHandler.GetPeriods(isLoading, (response) => {
                    periods.value = response.data as Array<IPeriod>
                })
            }
            
            watch( isOpenModalRef, () => {
                if( isOpenModalRef.value == true ){
                    InitPeriods()
                }
            })
            // =================================================================
            // handle previous Selected
            interface ISelected{
                item: IPeriod;
                value: boolean;
            }

            let previousSelected = selected.value[0]
            function HandleItemSelected( item: ISelected ){
                if( (item.value == false) ){
                    previousSelected = item.item
                }                
            }
            // =================================================================
            // update selected
            watch( selected, () => {
                if( selected.value.length == 0 ){
                    selected.value.push( previousSelected )
                }

                const selectedPeriod = selected.value[0]
                if( selectedPeriod ){
                    const emitValue = {
                        periodName: selectedPeriod.name,
                        isUsedPeriod: true,
                        startDate: selectedPeriod.startDate,
                        endDate: selectedPeriod.endDate,
                    } as IDateRange

                    emit( "updateDateRange", emitValue )
                }
            })
            // =================================================================

            const search = ref("")
            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Start Date", value: "startDate", align:"center" },
                { text: "End Date", value: "endDate", align:"center" },
            ] as Array<DataTableHeader>

            return {
                isOpenModalRef,
                isLoading,
                periods,
                selected,
                search,
                headers,
                HandleItemSelected,
            }
        }        
    })
</script>

<style lang="scss">

</style>