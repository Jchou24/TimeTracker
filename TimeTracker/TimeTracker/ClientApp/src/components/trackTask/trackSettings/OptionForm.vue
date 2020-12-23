<template>
    <div class="OptionForm">
        <GeneralCard :title="title" :titleIcon="titleIcon"
            :width="width" :loading="isLoadingOptions">
            <template v-slot:body>
                <v-card-title class="px-10 pb-2">
                    #Total: {{taskOptions.length}}
                    
                    <v-spacer/>
                    <v-text-field class="mb-4"
                        v-model="search"
                        append-icon="mdi-magnify"
                        label="Search"
                        single-line
                        hide-details
                        />
                </v-card-title>

                <v-data-table class="px-6" color="primary"
                    v-model="selected"
                    :headers="headers"
                    :items="taskOptions"
                    :search="search"
                    :items-per-page="10"
                    show-select
                    item-key="guid"
                    fixed-header
                    filterable
                    multi-sort
                    no-data-text="No Periods found"
                >
                    <template v-slot:item.isActive="{ item }">
                        <v-chip class="pl-4" outlined ripple :class="{'pr-7': item.isActive}"
                            :color="item.isActive ? 'primary' : 'warning' "
                            >
                            <v-icon left>
                                {{ item.isActive ? 'mdi-check-circle-outline' : 'mdi-cancel' }}
                            </v-icon>
                            <span :class="{'ml-1': item.isActive}">
                                {{ item.isActive ? 'Active' : 'Disactive' }}
                            </span>
                        </v-chip>
                    </template>

                    <template v-slot:item.actions="{ item }">
                        <v-icon small class="mr-2"
                            @click="HandleStartUpdateOption(item)"
                            >
                            mdi-pencil
                        </v-icon>
                        <v-icon small
                            @click="HandleDisActiveOption(item)"
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
                                :loading="isDisActiveOptions"
                                :disabled="isDisabledDisActiveOptions"
                                @click="HandleDisActiveSelectedOptions"
                                >
                                <v-icon left>mdi-delete</v-icon> Disactive Selected Options
                            </v-btn>
                        </v-col>
                        <v-col>
                            <v-row class="justify-end">
                                <OptionFormAdd :modalWidth="modalWidth" :taskOptions="taskOptions" :OptionAPIHandler="OptionAPIHandler"  @addOption="HandleAddOption"/>
                                <v-btn class="pl-5 pr-6 mr-3" color="primary" rounded elevation="4" large
                                    :loading="isLoadingOptions"
                                    :disabled="isLoadingOptions"
                                    @click="InitOptions"
                                    >
                                    <v-icon left>mdi-reload</v-icon> Reload
                                </v-btn>
                            </v-row>
                        </v-col>
                    </v-row>
                </v-container>                
            </template>
        </GeneralCard>

        <OptionFormUpdate 
            :modalWidth="modalWidth"
            :isOpenModal.sync="isOpenUpdateModal"
            :taskOption.sync="updateOption"

            :taskOptions="taskOptions"
            :OptionAPIHandler="OptionAPIHandler"
            @updateOption="HandleUpdateOption"
        />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'
    import OptionFormAdd from './OptionFormAdd.vue'
    import OptionFormUpdate from './OptionFormUpdate.vue'

    import { DataTableHeader } from 'vuetify'
    import { ToastSuccess, ToastError } from '@/util/notification'
    import { useToast } from "vue-toastification/composition"
    import { IOptionMethods, ITaskEntityOption } from '@/models/parameter'

    export default defineComponent({
        name: 'OptionForm',
        props:{
            width:{
                type: Number,
            },
            modalWidth:{
                type: Number,
                default: 500
            },
            title:{
                type: String,
            },
            titleIcon:{
                type: String,
            },
            OptionAPIHandler:{
                type: Object as () => IOptionMethods,
                required: true
            },
        },
        components:{
            GeneralCard,
            OptionFormAdd,
            OptionFormUpdate,
        },
        setup( props, { emit, root } ){
            const toast = useToast()

            const selected = ref([] as Array<ITaskEntityOption>)
            const search = ref("")
            const headers = [
                { text: "Display Text", value: "displayName" },
                { text: "Code", value: "codeName" },
                { text: "Status", value: "isActive" },
                { text: 'Actions', value: 'actions', sortable: false },
            ] as Array<DataTableHeader>
            const isLoadingOptions = ref(false)
            const isDisActiveOptions = ref(false)
            const isOpenUpdateModal = ref(false)
            const isDisabledDisActiveOptions = computed( () => !(isDisActiveOptions.value || selected.value.length > 0) )
            // =================================================================
            // Init

            const taskOptions = ref([] as Array<ITaskEntityOption>)

            function InitOptions(){
                props.OptionAPIHandler.GetOptions(isLoadingOptions, (response) => {
                    taskOptions.value = response.data as Array<ITaskEntityOption>
                })
            }
            InitOptions()
            
            function HandleAddOption(option: ITaskEntityOption){
                taskOptions.value = [ option, ...taskOptions.value ]
            }
            // =================================================================
            // Handle Delete Period
            function DisActiveOptions(targetTaskOptions: Array<ITaskEntityOption>, isClearSelect: boolean) {
                targetTaskOptions.forEach( option => option.isActive = false )
                props.OptionAPIHandler.UpdateOptions(targetTaskOptions, isDisActiveOptions, () => {
                    ToastSuccess( `Disactive ${targetTaskOptions.length} option(s)`, toast )
                    if (isClearSelect) {
                        selected.value = []
                    }
                }, () => {
                    // TODO Handle delete fail
                })
            }

            const HandleDisActiveSelectedOptions = () => DisActiveOptions(selected.value, true)
            const HandleDisActiveOption = (targetTaskOption: ITaskEntityOption) => DisActiveOptions([targetTaskOption], false)
            // =================================================================

            const updateOption = ref({} as ITaskEntityOption)
            function HandleStartUpdateOption(targetTaskOption: ITaskEntityOption){
                updateOption.value = targetTaskOption
                isOpenUpdateModal.value = true
            }

            function HandleUpdateOption(targetTaskOption: ITaskEntityOption) {
                const updatedOption = taskOptions.value.find( x => x.guid == targetTaskOption.guid )

                if(!updatedOption){
                    return
                }

                updatedOption.codeName = targetTaskOption.codeName
                updatedOption.displayName = targetTaskOption.displayName
                updatedOption.isActive = targetTaskOption.isActive
            }

            return {
                isLoadingOptions,
                isDisActiveOptions,
                isDisabledDisActiveOptions,
                isOpenUpdateModal,
                selected,
                taskOptions,
                search,
                headers,
                updateOption,
                InitOptions,
                HandleAddOption,
                HandleDisActiveSelectedOptions,
                HandleStartUpdateOption,
                HandleUpdateOption,
                HandleDisActiveOption,
            }
        }        
    })
</script>

<style lang="scss">
    .OptionForm .theme--light.v-data-table tbody tr.v-data-table__selected{
        background: rgba(243, 229, 200, 0.6);
    }
</style>