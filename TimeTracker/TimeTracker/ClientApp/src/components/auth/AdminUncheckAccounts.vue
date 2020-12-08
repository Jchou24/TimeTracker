<template>
    <GeneralCard class="AdminUncheckAccounts my-10" title="Uncheck Accounts" titleIcon="mdi-account-alert" :width="800">
        <template v-slot:body>
            <v-card-title class="px-10 pb-2">
                #Unchecks: {{uncheckAccounts.length}}
                <v-spacer></v-spacer>
                <v-text-field 
                    v-model="search"
                    append-icon="mdi-magnify"
                    label="Search"
                    single-line
                    hide-details
                    />
            </v-card-title>

            <v-data-table class="px-6"
                :headers="headers"
                :items="uncheckAccounts"
                :search="search"
                :items-per-page="10"
                :loading="isLoading"
                fixed-header
                filterable
                multi-sort
                no-data-text="No Uncheck Accounts"
            >
                <template v-slot:item.accountStatus="{ item }">
                    <v-btn color="primary" class="text-capitalize mr-2"
                        elevation="4" rounded
                        @click="ApproveUncheckAccounts(item)">
                        Approve
                    </v-btn>
                    <v-btn color="warning" class="text-capitalize"
                        elevation="4" rounded
                        @click="DenyUncheckAccounts(item)">
                        Deny
                    </v-btn>
                </template>
            </v-data-table>
        </template>
    </GeneralCard>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'

    // import { useToast } from "vue-toastification"
    import { useToast } from "vue-toastification/composition"
    import { ToastSuccess } from '@/util/notification.ts'

    import { DataTableHeader } from 'vuetify/types'

    import { AdminAPIHandler } from '@/api/admin.ts'
    import { IClaims, IUpdateAccounts } from '@/models/authentication.ts'
    import { AccountStatus } from '@/models/constants/authentication.ts'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    export default defineComponent({
        name: 'AdminUncheckAccounts',
        components:{
            GeneralCard
        },
        setup(props, { emit, root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>
            const router = $router
            const adminAPIHandler = new AdminAPIHandler( store, router )
            const toast = useToast()

            const isLoading = ref(false)
            const uncheckAccounts = ref([] as Array<IClaims>)
            function HandlerGetUncheckAccounts(){
                adminAPIHandler.GetUncheckAccounts(isLoading, (response)=>{
                    uncheckAccounts.value = response.data as Array<IClaims>
                })
            }
            HandlerGetUncheckAccounts()


            function HandleGetAccounts(){
                emit("GetAccounts")
            }            

            function UpdateUncheckAccounts( uncheckAccount: IClaims, isApproved: boolean ){
                const updateAccounts  = [{
                    Guid: uncheckAccount.guid,
                    Name: "",
                    IsUpdateName: false,
                    AccountStatus: isApproved? AccountStatus.Approved : AccountStatus.Rejected,
                    IsUpdateAccountStatus: true,
                    UserRoles: [],
                    IsUpdateUserRoles: false,
                }] as Array<IUpdateAccounts>

                adminAPIHandler.UpdateAccounts(updateAccounts, isLoading, ()=>{
                        const messageTitle = uncheckAccount.name ? uncheckAccount.name : uncheckAccount.email
                        ToastSuccess( `${messageTitle}: Account status is updated!`, toast )
                        HandlerGetUncheckAccounts()
                        HandleGetAccounts()
                    })
            }

            function ApproveUncheckAccounts(uncheckAccount: IClaims){
                UpdateUncheckAccounts(uncheckAccount, true)
            }

            function DenyUncheckAccounts(uncheckAccount: IClaims){
                UpdateUncheckAccounts(uncheckAccount, false)
            }

            const search = ref("")
            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Email", value: "email", align:"center" },
                { text: "Action", value: "accountStatus", align:"center" },
            ] as Array<DataTableHeader>

            return {
                search,
                headers,
                uncheckAccounts,

                isLoading,

                ApproveUncheckAccounts,
                DenyUncheckAccounts,
                
            }
        }

    });
</script>

<style lang="scss" scoped>
</style>