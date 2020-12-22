<template>
    <div class="UpdatePeriod">
        <v-dialog :width="modalWidth"
            v-model="isOpenModalRef"
            >
            <GeneralCard title="Update Period" titleIcon="mdi-calendar-import" :width="modalWidth">
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
                        <v-icon left>mdi-upload</v-icon>
                        Update
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
    import { ToastSuccess, ToastError } from '@/util/notification'
    import { useToast } from "vue-toastification/composition"
    import { IsDisabledSubmit } from '@/util/period'

    export default defineComponent({
        name: 'UpdatePeriod',
        props:{
            modalWidth:{
                type: Number,
                default: 500
            },
            isOpenModal:{
                type: Boolean,
            },
            period:{
                type:  Object as () => IPeriod,
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
            const isOpenModalRef = computed({
                get: () => props.isOpenModal,
                set: (value) => emit("update:isOpenModal", value)
            })
            
            const name = ref("")
            const startDate = ref("")
            const endDate = ref("")
            const isDisabledSubmit = computed( () => IsDisabledSubmit(name.value, startDate.value, endDate.value, isLoading.value) )

            function InitFormValue() {
                name.value = props.period?.name || ""
                startDate.value = props.period?.startDate || ""
                endDate.value = props.period?.endDate || ""
            }
            watch( () => props.period, () => InitFormValue() )            
            // =================================================================
            function HandleSubmit() {
                const period = {
                    guid: props.period?.guid,
                    name: name.value,
                    startDate: startDate.value,
                    endDate: endDate.value,
                } as IPeriod

                periodAPIHandler.UpdatePeriods([period], isLoading, (response) => {
                    isOpenModalRef.value = false
                    emit("update:period", period)
                    emit("updatePeriod", period)
                    ToastSuccess( `Period Updated`, toast)
                    InitFormValue()
                }, () => {
                    // TODO handle error case
                })
            }
            

            return {
                isLoading,
                isOpenModalRef,
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