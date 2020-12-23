<template>
    <div class="OptionFormUpdate">
        <v-dialog :width="modalWidth"
            v-model="isOpenModalRef"
            >
            <GeneralCard title="Update Option" titleIcon="mdi-upload" :width="modalWidth">
                <template v-slot:body>
                    <v-container fluid class="px-6">
                        <v-row class="justify-center">
                            <v-text-field class="OptionFormAdd-field"
                                v-model="codeName"
                                :error-messages="codeNameError"
                                label="Code"
                                counter
                                maxlength="256"
                                append-icon="mdi-pencil"
                                outlined
                                dense
                                @input="isInitCodeName = false"
                            />
                        </v-row>
                        <v-row class="justify-center">
                            <v-text-field class="OptionFormAdd-field"
                                v-model="displayName"
                                label="Display Text"
                                counter
                                maxlength="256"
                                append-icon="mdi-pencil"
                                outlined
                                dense
                            />
                        </v-row>                        
                        <v-row class="justify-center mb-n6">
                            <v-switch inset color="primary"
                                v-model="isActive"                            
                                :label=" isActive ? 'Activate Option' : 'Disactivate Option' "
                            />
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
    import { IsDisabledSubmit } from '@/util/parameter'
    import { IOptionMethods, ITaskEntityOption } from '@/models/parameter'

    export default defineComponent({
        name: 'OptionFormUpdate',
        props:{
            modalWidth:{
                type: Number,
                default: 500
            },
            isOpenModal:{
                type: Boolean,
            },
            taskOption:{
                type:  Object as () => ITaskEntityOption,
            },
            taskOptions:{
                type: Array as () => Array<ITaskEntityOption>,
                required: true,
            },
            OptionAPIHandler:{
                type: Object as () => IOptionMethods,
                required: true
            },
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
            
            const codeName = ref("")
            const isInitCodeName = ref(true)
            const codeNameError = computed( () => {
                if (isInitCodeName.value) {
                    return ''
                }

                if (codeName.value == props.taskOption?.codeName) {
                    return ''
                }
                
                if (codeName.value.length == 0) {
                    return 'Code is required'
                }
                
                return props.taskOptions.some( option => option.codeName == codeName.value ) ? 'Code should be unique.' : ''
            })
            const displayName = ref("")
            const isActive = ref(false)

            const IsOrigin = () =>
                isActive.value == props.taskOption?.isActive && 
                codeName.value == props.taskOption?.codeName && 
                displayName.value == props.taskOption?.displayName
            const isDisabledSubmit = computed( () => codeNameError.value.length > 0 || IsOrigin() )

            function InitFormValue() {
                codeName.value = props.taskOption?.codeName || ""
                displayName.value = props.taskOption?.displayName || ""
                isActive.value = props.taskOption?.isActive || false
            }
            watch( () => props.taskOption, () => InitFormValue() )            
            // =================================================================
            function HandleSubmit() {
                const taskOptions = {
                    guid: props.taskOption?.guid,
                    codeName: codeName.value,
                    displayName: displayName.value,
                    isActive: isActive.value,
                } as ITaskEntityOption

                props.OptionAPIHandler.UpdateOptions([taskOptions], isLoading, (response) => {
                    isOpenModalRef.value = false
                    emit("update:taskOption", taskOptions)
                    emit("updateOption", taskOptions)
                    ToastSuccess( `Option Updated`, toast)
                    InitFormValue()
                }, () => {
                    // TODO handle error case
                })
            }
            

            return {
                isLoading,
                isOpenModalRef,
                isDisabledSubmit,
                isInitCodeName,
                isActive,
                codeName,
                codeNameError,
                displayName,
                HandleSubmit,
            }
        }        
    })
</script>

<style lang="scss" >
    .OptionFormUpdate-field{
        width: 350px;
        max-width: 350px;

        .v-input__prepend-outer{
            width: 0;
            margin: 0;
        }
    }
</style>