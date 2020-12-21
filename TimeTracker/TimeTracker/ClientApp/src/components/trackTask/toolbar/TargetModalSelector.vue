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
                    :single-select="singleSelectTarget"
                    show-select
                    item-key="guid"
                    fixed-header
                    filterable
                    multi-sort
                    no-data-text="No Accounts"
                />
            </v-card>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import { DataTableHeader } from 'vuetify/types'
    import { Store } from 'vuex/types/index'
    import { Dictionary } from 'vue-router/types/router'
    import { TaskEditorAPIHandler } from '@/api/taskEditor.ts'
    import { IClaims } from '@/models/authentication.ts'
    import { IStore } from '@/models/store'
    import { GetGuids, IsEqualUsers } from '@/util/authentication'

    interface ISelected{
        item: IClaims;
        value: boolean;
    }

    export default defineComponent({
        name: 'TargetModalSelector',
        props:{
            isOpenModal:{
                type: Boolean,
            },
            selectedUsers:{
                type: Array as () => Array<IClaims>,
                default: () => [],
            },
            width:{
                type: Number,
                default: 500,
            },
            singleSelectTarget:{
                type: Boolean,
                default: true
            },
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorAPIHandler = new TaskEditorAPIHandler( store, router )
            // =================================================================
            const selectTargetQueryKey = "st"
            const search = ref("")
            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Email", value: "email", align:"center" },
            ] as Array<DataTableHeader>
            
            const selected = ref([] as Array<IClaims>)
            // =================================================================
            // Handle emit event
            const isOpenModalRef = computed({
                get: () => props.isOpenModal,
                set: (value) => emit("update:isOpenModal", value)
            })

            const EmitSelectedUsers = () => emit("update:selectedUsers", selected.value )
            watch( () => props.selectedUsers, () => selected.value = props.selectedUsers )
            watch( selected, ( newValue, oldValue ) => {
                // Not allow selection list empty
                if( newValue.length == 0 ){
                    selected.value = JSON.parse(JSON.stringify(oldValue))
                    return
                }

                if( newValue.length > 0 && oldValue.length > 0 && !IsEqualUsers( GetGuids(newValue), GetGuids(oldValue) )){
                    EmitSelectedUsers()
                }    
            })
            // =================================================================
            // Init
            const users = ref([] as Array<IClaims>)

            function GetSelectedTarget( users: Array<IClaims> ){
                const queryString = $route.query[selectTargetQueryKey]
                if( queryString && users.some( user => user.guid == queryString ) ){
                    return users.find( user => user.guid == queryString )
                }else{
                    return users.find( user => user.guid == store.state.authentication.claims.guid )
                }
            }

            const isLoading = ref(false)
            function InitUsers(){
                taskEditorAPIHandler.GetAccounts(isLoading, (response) => {
                    users.value = response.data as Array<IClaims>

                    if(selected.value.length > 0){
                        return
                    }

                    if(props.singleSelectTarget) {
                        selected.value = [GetSelectedTarget( users.value ) as IClaims]
                    }else{
                        selected.value = users.value
                    }
                    EmitSelectedUsers()
                })
            }
            InitUsers()
            watch( isOpenModalRef, () => {
                if( isOpenModalRef.value == true ){
                    InitUsers()
                }
            })
            // =================================================================
            // Sync Query String Parameter if in singleSelectTarget mode
            watch( selected, () => {
                if( !props.singleSelectTarget ){
                    return
                }

                const query = { ...$route.query } as Dictionary<string>
                if( selected.value.length > 0 && selected.value[0].guid ){
                    query[selectTargetQueryKey] = selected.value[0].guid
                    $router.replace({ query }).catch(()=>{query})
                }
            })
            // =================================================================

            return {
                isLoading,
                isOpenModalRef,
                search,
                selected,
                headers,
                users,
            }
        }        
    })
</script>

<style lang="scss"> 
</style>