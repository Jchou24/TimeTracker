<template>
    <div class="OptionFormAdd">
        <v-btn class="pl-5 mx-3" color="primary" rounded elevation="4" large
            @click="isOpenModal = true"
            >
            <v-icon left>mdi-plus-circle-outline</v-icon>{{buttonText}}
        </v-btn>

        <v-dialog :width="modalWidth"
            v-model="isOpenModal"
            >
            <GeneralCard :title="buttonText" titleIcon="mdi-plus-circle" :width="modalWidth" v-if="isOpenModal">
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
                        <v-row class="justify-center mb-n6">
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
                        <v-icon left>mdi-send</v-icon>
                        Submit
                    </v-btn>
                </template>
            </GeneralCard>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'

    import { DataTableHeader } from 'vuetify'
    import { ToastSuccess, ToastError } from '@/util/notification'
    import { useToast } from "vue-toastification/composition"
    import { IOptionMethods, ITaskEntityOption } from '@/models/parameter'

    export default defineComponent({
        name: 'OptionFormAdd',
        props:{
            buttonText:{
                type: String,
                default: "New Option"
            },
            modalWidth:{
                type: Number,
                default: 500
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
        },
        setup( props, { emit, root } ){
            const toast = useToast()

            const isLoading = ref(false)
            const isOpenModal = ref(false)
            
            const codeName = ref("")
            const isInitCodeName = ref(true)
            const codeNameError = computed( () => {
                if (isInitCodeName.value) {
                    return ''
                }

                if (codeName.value.length == 0) {
                    return 'Code is required'
                }
                
                return props.taskOptions.some( option => option.codeName == codeName.value ) ? 'Code should be unique.' : ''
            })
            const displayName = ref("")

            const isDisabledSubmit = computed( () => isInitCodeName.value || codeNameError.value.length > 0 )

            function InitFormValue() {
                codeName.value = ""
                displayName.value = ""
                isInitCodeName.value = true
            }
            InitFormValue()

            watch( isOpenModal, () => {
                InitFormValue()
            })
            // =================================================================
            function HandleSubmit() {
                const taskOptions = {
                    codeName: codeName.value,
                    displayName: displayName.value,
                    isActive: true,
                } as ITaskEntityOption

                props.OptionAPIHandler.CreateOptions([taskOptions], isLoading, (response) => {
                    isOpenModal.value = false
                    taskOptions.guid = response.data[0]
                    emit("addOption", taskOptions)
                    ToastSuccess( `Add Option ${displayName.value}`, toast)
                    InitFormValue()
                }, () => {
                    // TODO handle error case
                })
            }
            

            return {
                isLoading,
                isOpenModal,
                isDisabledSubmit,
                isInitCodeName,
                codeName,
                codeNameError,
                displayName,
                HandleSubmit,
            }
        }        
    })
</script>

<style lang="scss" >
    .OptionFormAdd-field{
        width: 350px;
        max-width: 350px;
    }
</style>