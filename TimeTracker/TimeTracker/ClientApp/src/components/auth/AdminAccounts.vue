<template>
    <GeneralCard class="AdminAccounts my-10" title="Accounts" titleIcon="mdi-account-group" 
        :width="800"
        v-if="props.accounts">
        <template v-slot:body>
            <v-card-title class="px-10 pb-2">
                #Accounts: {{accountsExtend.length}}
                <v-spacer />
                <v-text-field 
                    v-model="search"
                    append-icon="mdi-magnify"
                    label="Search"
                    single-line
                    hide-details
                    />
            </v-card-title>

            <v-data-table class="px-6" color="primary"
                v-model="selected.value"
                show-select
                item-key="guid"
                :headers="headers"
                :items="accountsExtend"
                :search="search"
                :items-per-page="10"
                :loading="isLoading"
                :single-select="true"
                fixed-header
                filterable
                multi-sort
                no-data-text="No Accounts"
            >
                <template v-slot:header.data-table-select>
                    Change?
                </template>
                <template v-slot:item.data-table-select="{ isSelected }">
                    <v-simple-checkbox class="flex-center" :value="isSelected" color="warning" />
                </template>

                <template v-slot:item.admin="{ item }">
                    <v-simple-checkbox v-model="accountsRoles[item.guid][UserRoles.Admin]" color="primary" />
                </template>

                <template v-slot:item.user="{ item }">
                    <v-simple-checkbox v-model="accountsRoles[item.guid][UserRoles.User]" color="primary" />
                </template>

                <template v-slot:item.accountStatus="{ item }">
                    <v-select rounded dense class="mt-2 mb-n3"
                        v-model="accountsStatus[item.guid].value"
                        :items="selectOptions">
                        <template v-slot:item="{ item }">
                            <div class="text-center">
                                <v-icon class="ml-6 mr-4">{{accountStatusIcon[item.value]}}</v-icon>
                                {{item.text}}
                            </div>
                        </template>
                        <template v-slot:selection="{ item }">
                            <v-chip link outlined class="pr-4"
                                :color="accountStatusColor[item.value]">
                                <v-icon>{{accountStatusIcon[item.value]}}</v-icon>
                                {{item.text}}
                            </v-chip>
                        </template>
                    </v-select>
                </template>
            </v-data-table>
        </template>
        <template v-slot:action>
            <v-btn class="pa-6 px-8 text-center text-caption"
                color="primary"
                :loading="isLoading"
                elevation="2"
                large
                rounded         
                @click="HandleUpdateAccounts" :disabled="!isUpdateAccounts"
            >
                <v-icon left>mdi-upload-multiple</v-icon>
                    Update
                </v-btn>
        </template>
    </GeneralCard>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive, watch, Ref } from '@vue/composition-api'

    import GeneralCard from '@/components/card/GeneralCard.vue'

    // import { useToast } from "vue-toastification"
    import { useToast } from "vue-toastification/composition"
    import { ToastSuccess } from '@/util/notification.ts'
    import { DataItemsPerPageOption, DataTableHeader } from 'vuetify'

    import { AdminAPIHandler } from '@/api/admin.ts'
    import { IClaims, IUpdateAccounts, IUserRole } from '@/models/authentication.ts'
    import { accountStatusIcon, accountStatusColor, AccountStatus, UserRoles, GetAccountStatusKey, GetUserRolesKey } from '@/models/constants/authentication.ts'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    interface IUserInfoDetailExtend extends IClaims{
        admin: string;
        user: string;
        isSelectable: boolean;
    }

    interface IProps{
        accounts: Array<IClaims>;
    }

    interface IMapUserRoles{
        [key: number]: boolean;
    }

    export default defineComponent({
        name: 'AdminAccounts',
        components:{
            GeneralCard
        },
        props:{
            accounts:{
                required: true
            }
        },
        setup(props: IProps, { emit, root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>
            const router = $router
            const adminAPIHandler = new AdminAPIHandler( store, router )
            const toast = useToast()

            const isLoading = ref(false)
            function HandleGetAccounts(){
                emit("GetAccounts")
            }

            const search = ref("")
            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Email", value: "email", align:"center" },
                { text: "Status", value: "accountStatus", align:"center", width:220 },
                { text: "Admin", value: "admin", align:"center" },
                { text: "User", value: "user", align:"center" },
            ] as Array<DataTableHeader>                     

            const selectOptions = GetAccountStatusKey().map( key => ({
                text: AccountStatus[key],
                value: key,
                disabled: key == AccountStatus.Uncheck ? true : false
            } as DataItemsPerPageOption) ) as Array<DataItemsPerPageOption>

            const accountsExtend = computed( () => props.accounts.map( account => {
                const accountExtend = {} as IUserInfoDetailExtend
                Object.assign(accountExtend, account);
                accountExtend.isSelectable = false
                accountExtend.admin = ""
                accountExtend.user  = ""
                return accountExtend
            }) as Array<IUserInfoDetailExtend>)

            // =================================================================
            function GetEmptyUserRoles(): Record<number, boolean>{
                return GetUserRolesKey().reduce( ( result, key ) => {
                    result[key] = false
                    return result
                }, {} as IMapUserRoles)
            }

            function ReduceUserRoles( account: IClaims ): Record<number, boolean>{
                return account.userRoles.reduce( (accumulator, role) => {
                        accumulator[role.id] = true
                        return accumulator
                    }, reactive(GetEmptyUserRoles()) )
            }
            // =================================================================
            // accountsRoles
            const accountsRoles = reactive({} as Record<string, Record<number, boolean> >)
            function InitAccountsRoles(accounts: Array<IClaims>){
                accounts.forEach( (account: IClaims) => 
                    accountsRoles[account.guid] = ReduceUserRoles(account))
            }
            InitAccountsRoles(props.accounts)
            watch( () => props.accounts, (accounts) =>{
                InitAccountsRoles(accounts)
            })
            // =================================================================
            // accountsStatus
            const accountsStatus = reactive({} as Record<string, Ref>)
            function InitAccountsStatus(accounts: Array<IClaims>){
                accounts.forEach( account => accountsStatus[account.guid] = ref(account.accountStatus) )
            }
            InitAccountsStatus(props.accounts)
            watch( () => props.accounts, (accounts) =>{
                InitAccountsStatus(accounts)
            })
            // =================================================================
            // HandleUpdateAccounts
            const updateRolesIds = computed( (): Array<string> =>{
                return props.accounts.filter( (account: IClaims) =>{
                    const differentRoles = Object.entries( ReduceUserRoles(account) )
                        .filter( ( [key, value], idx ) => 
                            accountsRoles[account.guid][parseInt(key)] != value)
                    return differentRoles.length > 0
                }).map( account => account.guid )
            })

            const updateAccountStatusIds = computed( (): Array<string> =>{
                const updateAccount = props.accounts.filter( account => 
                    account.accountStatus !== accountsStatus[account.guid].value )
                return updateAccount.map( account => account.guid )
            })

            const isUpdateAccounts = computed(()=> updateRolesIds.value.length > 0 || updateAccountStatusIds.value.length > 0 )
            
            function GetEmptyUpdateAccounts(guid: string): IUpdateAccounts{
                return {
                    Guid: guid,
                    Name: "",
                    IsUpdateName: false,
                    AccountStatus:   AccountStatus.Approved,
                    IsUpdateAccountStatus: false,
                    IsUpdateUserRoles: false,
                    UserRoles: []
                }
            }
            
            function HandleUpdateAccounts(){
                if(!isUpdateAccounts.value){
                    return
                }

                const tmpUpdateAccounts = {} as Record<string, IUpdateAccounts>
                updateRolesIds.value.forEach((guid: string) => {
                    tmpUpdateAccounts[guid] = {
                        ...GetEmptyUpdateAccounts(guid),
                        IsUpdateUserRoles : true,
                        UserRoles : Object.keys(accountsRoles[guid]).map(Number).filter( key => accountsRoles[guid][key] ),
                    }
                })

                updateAccountStatusIds.value.forEach( (guid: string) => {
                    if( !(guid in tmpUpdateAccounts) ){
                        tmpUpdateAccounts[guid] = GetEmptyUpdateAccounts(guid)
                    }

                    tmpUpdateAccounts[guid] = {
                        ...tmpUpdateAccounts[guid],
                        IsUpdateAccountStatus : true,
                        AccountStatus : accountsStatus[guid].value,
                    }
                })

                const updateAccounts = Object.values(tmpUpdateAccounts) as Array<IUpdateAccounts>
                adminAPIHandler.UpdateAccounts(updateAccounts, isLoading, ()=>{
                        ToastSuccess( "Account(s) updated!", toast)
                        HandleGetAccounts()
                    })           
            }
            // =================================================================
            // selected: store changed row

            const selected = computed( () =>{
                const changedIds = new Set(updateRolesIds.value)
                updateAccountStatusIds.value.forEach( guid => changedIds.add(guid) )
                const result = ref( accountsExtend.value.filter( accounts => changedIds.has(accounts.guid) ) as Array<IUserInfoDetailExtend> )
                return result
            })

            return {
                props,

                search,
                selected,
                headers,
                accountsExtend,
                accountsRoles,
                accountsStatus,
                selectOptions,
                accountStatusIcon,
                accountStatusColor,

                isLoading,
                isUpdateAccounts,

                UserRoles,
                HandleUpdateAccounts,
            }
        }

    });
</script>

<style lang="scss">
    .AdminAccounts .v-data-table--fixed-header>.v-data-table__wrapper{
        overflow-y: hidden;
    }

    .AdminAccounts .theme--light.v-data-table tbody tr.v-data-table__selected{
        background: rgba(243, 229, 200, 0.6);
    }
    
</style>

<style lang="scss" scoped>
</style>