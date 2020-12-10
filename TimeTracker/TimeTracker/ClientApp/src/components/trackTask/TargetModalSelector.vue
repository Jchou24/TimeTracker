<template>
    <div class="TargetModalSelector">
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
                    #Total: {{users.length}}
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
                    :items="users"
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
        name: 'TargetModalSelector',
        props:{
            isOpenModal:{
                type: Boolean,
            },
            selectedUser:{
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

            
            const selected = ref([] as Array<IClaims>)

            watch( props.selectedUser as IClaims, () => selected.value = [props.selectedUser as IClaims] )
            watch( selected, () => emit("update:selectedUser", selected.value[0] ) )
            // =================================================================
            // Init
            const isLoading = ref(false)
            const users = ref([] as Array<IClaims>)

            function GetSelectedTarget( users: Array<IClaims> ){
                const queryString = $route.query[selectTargetQueryKey]
                if( queryString && users.some( user => user.guid == queryString ) ){
                    return users.find( user => user.guid == queryString )
                }else{
                    return users.find( user => user.guid == store.state.authentication.claims.guid )
                }
            }

            function InitUsers(){
                taskEditorAPIHandler.GetAccounts(isLoading, (response) => {
                    users.value = response.data as Array<IClaims>
                    selected.value = [GetSelectedTarget( users.value ) as IClaims]
                })
            }
            InitUsers()
            watch( isOpenModalRef, () => {
                if( isOpenModalRef.value == true ){
                    InitUsers()
                }
            })
            // =================================================================
            // handle previous Selected
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
            // =================================================================
            // update selected
            watch( selected, () => {
                if( selected.value.length == 0 ){
                    selected.value.push( previousSelected )
                }

                const query = { ...$route.query } as Dictionary<string>
                if( selected.value.length > 0 && selected.value[0].guid ){
                    query[selectTargetQueryKey] = selected.value[0].guid
                    $router.replace({ query }).catch(()=>{query})
                }
            })
            // =================================================================
            const search = ref("")
            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Email", value: "email", align:"center" },
            ] as Array<DataTableHeader>


            return {
                isLoading,
                isOpenModalRef,
                search,
                selected,
                headers,
                users,

                HandleItemSelected,
            }
        }        
    })
</script>

<style lang="scss"> 
</style>