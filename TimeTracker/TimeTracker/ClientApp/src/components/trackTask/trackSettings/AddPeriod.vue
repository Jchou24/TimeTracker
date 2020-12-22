<template>
    <div class="AddPeriod">
        <v-btn class="pl-5 mx-3" color="primary" rounded elevation="4" large
            @click="isOpenModal = true"
            >
            <v-icon left>mdi-plus-circle-outline</v-icon> Add Period
        </v-btn>

        <v-dialog :width="modalWidth"
            v-model="isOpenModal"
            >
            <GeneralCard title="Add Period" titleIcon="mdi-plus-circle" :width="modalWidth" v-if="isOpenModal">
                <template v-slot:body>
                    <v-container fluid class="px-6">
                        <v-row class="justify-center">
                            <v-text-field class="AddPeriod-field"
                                v-model="name"
                                label="Period Name"
                                counter
                                maxlength="256"
                                append-icon="mdi-calendar-edit"
                                outlined
                                dense
                            />
                        </v-row>
                        <v-row class="justify-center">
                            <DatePicker class="AddPeriod-field" label="Start Date" :date.sync="startDate" />
                        </v-row>
                        <v-row class="justify-center mb-n6">
                            <DatePicker class="AddPeriod-field" label="End Date" :date.sync="endDate" />
                        </v-row>
                    </v-container>
                </template>

                <template v-slot:action>
                    <v-btn class="pa-6 px-8 text-center"
                        color="primary"
                        :loading="isLoading"
                        elevation="2"
                        large
                        rounded                        
                        @click="HandleSubmit" :disabled="isDisabledSubmit"
                    >
                        <v-icon left>mdi-plus-circle-outline</v-icon>
                        Add
                    </v-btn>
                </template>
            </GeneralCard>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'
    import DatePicker from '@/components/trackTask/toolbar/DatePicker.vue'

    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { IPeriod } from '@/models/period'
    import { DataTableHeader } from 'vuetify'
    import { PeriodAPIHandler } from '@/api/period'
    import moment from 'moment'
    import { ToastSuccess, ToastError } from '@/util/notification'
    import { useToast } from "vue-toastification/composition"
    import { IsDisabledSubmit } from '@/util/period'

    export default defineComponent({
        name: 'AddPeriod',
        props:{
            modalWidth:{
                type: Number,
                default: 500
            }
        },
        components:{
            GeneralCard,
            DatePicker,
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const periodAPIHandler = new PeriodAPIHandler( store, router )
            const toast = useToast()

            const isLoading = ref(false)
            const isOpenModal = ref(false)
            
            const name = ref("")
            const startDate = ref("")
            const endDate = ref("")
            const isDisabledSubmit = computed( () => IsDisabledSubmit(name.value, startDate.value, endDate.value, isLoading.value) )

            function InitFormValue() {
                name.value = ""
                startDate.value = ""
                endDate.value = ""
            }
            InitFormValue()
            // =================================================================
            function HandleSubmit() {
                const periods = [{
                    name: name.value,
                    startDate: startDate.value,
                    endDate: endDate.value,
                }] as Array<IPeriod>

                periodAPIHandler.CreatePeriods(periods, isLoading, (response) => {
                    isOpenModal.value = false
                    periods[0].guid = response.data[0]
                    emit("addPeriod", periods[0])
                    ToastSuccess( `Add Period ${name.value}`, toast)
                    InitFormValue()
                }, () => {
                    // TODO handle error case
                })
            }
            

            return {
                isLoading,
                isOpenModal,
                isDisabledSubmit,
                name,
                startDate,
                endDate,
                HandleSubmit,
            }
        }        
    })
</script>

<style lang="scss" >
    .AddPeriod-field{
        width: 350px;
        max-width: 350px;

        .v-input__prepend-outer{
            width: 0;
            margin: 0;
        }
    }
</style>