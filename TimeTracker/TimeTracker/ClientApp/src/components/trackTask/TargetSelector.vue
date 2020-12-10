<template>
    <div class="TargetSelector">
        <v-dialog :width="props.width"
            v-model="isOpenModalRef"   
            >
            <v-card
                elevation="10"
                :width="props.width"
                :loading="isLoading"
                outlined
                shaped
                >
                <v-card-title class="px-10 pb-2">
                    #Total: {{accounts.length}}
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
                    :items="accounts"
                    :search="search"
                    :items-per-page="10"
                    :loading="isLoading"
                    :single-select="true"
                    show-select
                    item-key="guid"
                    fixed-header
                    filterable
                    multi-sort
                    no-data-text="No Accounts"
                    @item-selected="HandleItemSelected"
                />
            </v-card>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'

    import { DataTableHeader } from 'vuetify/types'
    import { TaskEditorAPIHandler } from '@/api/taskEditor.ts'
    import { IClaims } from '@/models/authentication.ts'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'
    import { Dictionary } from 'vue-router/types/router'

    export default defineComponent({
        name: 'TargetSelector',
        props:{
            isOpenModal:{
                type: Boolean,
            },
            selectedAccount:{
                type: Object as () => IClaims,
            },
            width:{
                type: Number,
                default: 500,
            }
        },
        components:{
            GeneralCard
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )

            const selectTargetQueryKey = "st"

            const isOpenModalRef = computed({
                get: () => props.isOpenModal,
                set: (value) => emit("update:isOpenModal", value)
            })

            const search = ref("")
            const selected = ref([] as Array<IClaims>)

            watch( props.selectedAccount as IClaims, () => selected.value = [props.selectedAccount as IClaims] )
            watch( selected, () => emit("update:selectedAccount", selected.value[0] ) )

            function GetSelectedTargetGuid(){
                const querySt = $route.query[selectTargetQueryKey]
                return querySt ? querySt : store.state.authentication.claims.guid
            }
            const selectedTargetGuid = GetSelectedTargetGuid()
            

            const isLoading = ref(false)
            const accounts = ref([] as Array<IClaims>)
            taskEditorAPIHandler.GetAccounts(isLoading, (response) => {
                accounts.value = response.data as Array<IClaims>
                selected.value = accounts.value.filter( account => account.guid == selectedTargetGuid )
            })

            interface ISelected{
                item: IClaims;
                value: boolean;
            }
            let previousSelected = selected.value[0]
            function HandleItemSelected( item: ISelected ){
                if( (item.value == false) ){
                    previousSelected = item.item
                }                
            }
            watch( selected, () => {
                if( selected.value.length == 0 ){
                    selected.value.push( previousSelected )
                }

                const query = {} as Dictionary<string>
                query[selectTargetQueryKey] = selected.value[0].guid
                $router.replace({ query }).catch(()=>{query})
            })
                        

            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Email", value: "email", align:"center" },
            ] as Array<DataTableHeader>


            return {
                props,

                isLoading,
                isOpenModalRef,
                search,
                selected,
                headers,
                accounts,

                HandleItemSelected,
            }
        }        
    })
</script>

<style lang="scss"> 
</style>