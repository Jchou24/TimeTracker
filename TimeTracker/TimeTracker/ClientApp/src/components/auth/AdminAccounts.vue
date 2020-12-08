<template>
    <GeneralCard class="AdminAccounts" title="Accounts" titleIcon="mdi-account-group" 
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
                :headers="headers"
                :items="accountsExtend"
                :search="search"
                :items-per-page="10"
                :loading="isLoading"
                :single-select="true"
                fixed-header
                filterable
                multi-sort
                no-data-text="No Uncheck Accounts"
            >
                <template v-slot:header.data-table-select>
                    Change?
                </template>
                <template v-slot:item.data-table-select="{ isSelected }">
                    <v-simple-checkbox class="flex-center" :value="isSelected" color="warning" />
                </template>

                <template v-slot:item.admin="{ item }">
                    <v-simple-checkbox v-model="accountsRoles[item.id][UserRoles.Admin]" color="primary" />
                </template>

                <template v-slot:item.user="{ item }">
                    <v-simple-checkbox v-model="accountsRoles[item.id][UserRoles.User]" color="primary" />
                </template>

                <template v-slot:item.accountStatus="{ item }">
                    <v-select rounded dense class="mt-2 mb-n3"
                        v-model="accountsStatus[item.id].value"
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
            <v-btn class="pa-6 px-8 text-center"
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

    import { IUpdateAccounts, GetUncheckAccounts, GetAccounts, UpdateAccounts } from '@/api/admin.ts'
    import { IUserInfoDetail, IUserRole } from '@/models/authentication.ts'
    import { accountStatusIcon, accountStatusColor, AccountStatus, UserRoles, GetAccountStatusKey, GetUserRolesKey } from '@/models/constants/authentication.ts'

    interface IUserInfoDetailExtend extends IUserInfoDetail{
        admin: string;
        user: string;
        isSelectable: boolean;
    }

    interface IProps{
        accounts: Array<IUserInfoDetail>;
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
        setup(props: IProps, { emit }){
            const toast = useToast()

            const isLoading = ref(false)
            function HandleGetAccounts(){
                emit("GetAccounts")
            }

            function GetEmptyUserRoles(){
                const emptyUserRoles = {} as IMapUserRoles
                GetUserRolesKey().forEach( key => emptyUserRoles[key] = false )
                return emptyUserRoles
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
            // uncheckAccounts            
            const uncheckAccounts = ref([] as Array<IUserInfoDetail>)
            function HandlerGetUncheckAccounts(){
                GetUncheckAccounts(isLoading, (response)=>{
                    uncheckAccounts.value = response.data
                })
            }
            HandlerGetUncheckAccounts()
            // =================================================================
            // accountsRoles
            const accountsRoles = reactive({} as Record<number, Record<number, boolean> >)
            function InitAccountsRoles(accounts: Array<IUserInfoDetail>){
                accounts.forEach( (account: IUserInfoDetail) =>{
                    const tmpRoles = reactive(GetEmptyUserRoles())
                    account.userRoles.forEach( (role: IUserRole) =>{
                        tmpRoles[role.id] = true
                    })
                    accountsRoles[account.id] = tmpRoles
                })
            }
            InitAccountsRoles(props.accounts)
            watch( () => props.accounts, (accounts) =>{
                InitAccountsRoles(accounts)
            })
            // =================================================================
            // accountsStatus
            const accountsStatus = reactive({} as Record<number, Ref>)
            function InitAccountsStatus(accounts: Array<IUserInfoDetail>){
                accounts.forEach( account => accountsStatus[account.id] = ref(account.accountStatus) )
            }
            InitAccountsStatus(props.accounts)
            watch( () => props.accounts, (accounts) =>{
                InitAccountsStatus(accounts)
            })
            // =================================================================
            // HandleUpdateAccounts

            const updateRolesIds = computed(()=>{
                const result = [] as Array<number>
                props.accounts.forEach( (account: IUserInfoDetail) =>{
                    const tmpRoles = GetEmptyUserRoles()
                    account.userRoles.forEach( (role: IUserRole) =>{
                        tmpRoles[role.id] = true
                    })

                    // check whether updatable by user roles 
                    for (const [key, value] of Object.entries(tmpRoles)) {
                        if (accountsRoles[account.id][parseInt(key)] != value) {
                            result.push(account.id)
                        }
                    }
                })
                return result
            })

            const updateAccountStatusIds = computed(()=>{
                const result = [] as Array<number>
                props.accounts.forEach( (account: IUserInfoDetail) =>{
                    // check whether updatable by user account status 
                    if( account.accountStatus !== accountsStatus[account.id].value ){
                        result.push(account.id)
                    }  
                })
                return result
            })

            const isUpdateAccounts = computed(()=> updateRolesIds.value.length > 0 || updateAccountStatusIds.value.length > 0 )
            
            function GetEmptyUpdateAccounts(id: number): IUpdateAccounts{
                return {
                    Id: id,
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

                const tmpUpdateAccounts = {} as Record<number, IUpdateAccounts>
                updateRolesIds.value.forEach( (id: number) => {
                    tmpUpdateAccounts[id] = GetEmptyUpdateAccounts(id)
                    tmpUpdateAccounts[id].IsUpdateUserRoles = true
                    tmpUpdateAccounts[id].UserRoles = Object.keys(accountsRoles[id]).map(Number).filter( key => accountsRoles[id][key] )
                })

                updateAccountStatusIds.value.forEach( (id: number) => {
                    if( !(id in tmpUpdateAccounts) ){
                        tmpUpdateAccounts[id] = GetEmptyUpdateAccounts(id)
                    }
                    
                    tmpUpdateAccounts[id].IsUpdateAccountStatus = true
                    tmpUpdateAccounts[id].AccountStatus = accountsStatus[id].value
                })

                const updateAccounts = Object.values(tmpUpdateAccounts) as Array<IUpdateAccounts>
                UpdateAccounts(updateAccounts, isLoading, ()=>{
                        ToastSuccess(toast, "Account(s) updated!")
                        HandleGetAccounts()
                    })           
            }
            // =================================================================
            // selected: store changed row

            const selected = computed( () =>{
                const changedIds = {} as Record<number, any>
                updateRolesIds.value.forEach( id => changedIds[id] = "" )
                updateAccountStatusIds.value.forEach( id => changedIds[id] = "" )
                const result = ref( accountsExtend.value.filter( accounts => accounts.id in changedIds ) as Array<IUserInfoDetailExtend> )
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