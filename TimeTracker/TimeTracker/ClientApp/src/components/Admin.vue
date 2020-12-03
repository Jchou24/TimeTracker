<template>
    <div class="Admin">        
        <div class="">
            <h1>Uncheck Accounts</h1>
            <table class="uncheck-account" v-if="uncheckAccounts.length > 0">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="" v-for="(uncheckAccount, idx) in uncheckAccounts"
                        :key="idx"
                    >
                        <td>{{uncheckAccount.name}}</td>
                        <td>{{uncheckAccount.email}}</td>
                        <td><button @click="ApproveUncheckAccounts(uncheckAccount)">Approve</button></td>
                        <td><button @click="DenyUncheckAccounts(uncheckAccount)">Deny</button></td>
                    </tr>
                </tbody>
            </table>
            <div v-else>
                No Uncheck Accounts
            </div>
        </div>

        <div class="">
            <h1>All Accounts</h1>
            <table class="uncheck-account">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Status</th>
                        <th>Admin</th>
                        <th>User</th>                        
                    </tr>
                </thead>
                <tbody>
                    <tr class="" v-for="(account, idx) in accounts"
                        :key="idx"
                    >
                        <td>{{account.name}}</td>
                        <td>{{account.email}}</td>
                        <td>{{account.accountStatus}}</td>
                        <td>
                            <input type="checkbox" v-model="accountsRoles[account.id][1]">
                        </td>
                        <td>
                            <input type="checkbox" v-model="accountsRoles[account.id][2]">
                        </td>
                    </tr>
                </tbody>
            </table>
            <button @click="HandleUpdateAccounts" :disabled="!isUpdateAccounts">Update</button>
        </div>

    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api'

    // import { useToast } from "vue-toastification"
    import { useToast } from "vue-toastification/composition"

    // import { ILogin, Login, Logout } from '@/api/authentication.ts'
    import { IUpdateAccounts, GetUncheckAccounts, GetAccounts, UpdateAccounts } from '@/api/admin.ts'
    import { IUserInfoDetail, AccountStatus, UserRoles, IUserRole } from '@/models/authentication.ts'

    export default defineComponent({
        name: 'Admin',
        components:{
        },
        setup(props, { emit }){
            const toast = useToast()

            const uncheckAccounts = ref([] as Array<IUserInfoDetail>)
            function HandlerGetUncheckAccounts(){
                return GetUncheckAccounts()
                    .then((response)=>{
                        uncheckAccounts.value = response.data
                    })
            }
            HandlerGetUncheckAccounts()

            const accounts = ref([] as Array<IUserInfoDetail>)
            const accountsRoles = reactive({} as Record<number, Record<number, boolean> >)
            function HandleGetAccounts(){
                return GetAccounts()
                    .then((response)=>{
                        accounts.value = response.data
                        accounts.value.forEach( (account: IUserInfoDetail) =>{
                            const tmpRoles = reactive({ 1: false, 2: false } as Record<number, boolean>)
                            account.userRoles.forEach( (role: IUserRole) =>{
                                tmpRoles[role.id] = true
                            })
                            accountsRoles[account.id] = tmpRoles
                        })
                    })
            }
            HandleGetAccounts()

            function UpdateUncheckAccounts( uncheckAccount: IUserInfoDetail, isApproved: boolean ){
                const updateAccounts  = [{
                    Id: uncheckAccount.id,
                    Name: "",
                    IsUpdateName: false,
                    AccountStatus: isApproved? AccountStatus.Approved : AccountStatus.Rejected,
                    IsUpdateAccountStatus: true,
                    UserRoles: [],
                    IsUpdateUserRoles: false,
                }] as Array<IUpdateAccounts>

                UpdateAccounts(updateAccounts)
                    .then(()=>{
                        toast.success(`${uncheckAccount.name}: Account status is updated!`, {
                            icon: {
                                iconClass: 'material-icons',  
                                iconChildren: 'check_circle_outline', 
                                iconTag: 'span'               
                            },
                            timeout: 7 * 1000,
                        });

                        HandlerGetUncheckAccounts()
                        HandleGetAccounts()
                    })
            }

            function ApproveUncheckAccounts(uncheckAccount: IUserInfoDetail){
                UpdateUncheckAccounts(uncheckAccount, true)
            }

            function DenyUncheckAccounts(uncheckAccount: IUserInfoDetail){
                UpdateUncheckAccounts(uncheckAccount, false)
            }

            const updateAccountIds = computed(()=>{
                const result = [] as Array<number>
                accounts.value.forEach( (account: IUserInfoDetail) =>{
                    const tmpRoles = { 1: false, 2: false } as Record<number, boolean>
                    account.userRoles.forEach( (role: IUserRole) =>{
                        tmpRoles[role.id] = true
                    })

                    for (const [key, value] of Object.entries(tmpRoles)) {
                        if (accountsRoles[account.id][parseInt(key)] != value) {
                            result.push(account.id)
                        }
                    }
                })
                return result
            })

            const isUpdateAccounts = computed(()=> updateAccountIds.value.length > 0)
            
            function HandleUpdateAccounts(){
                if(!isUpdateAccounts.value){
                    return
                }

                const updateAccounts  = [] as Array<IUpdateAccounts>
                updateAccountIds.value.forEach( (id: number) =>{
                    updateAccounts.push({
                        Id: id,
                        Name: "",
                        IsUpdateName: false,
                        AccountStatus: AccountStatus.Approved,
                        IsUpdateAccountStatus: false,
                        IsUpdateUserRoles: true,
                        UserRoles: Object.keys(accountsRoles[id]).map(Number).filter( key => accountsRoles[id][key] ),
                    })                    
                })

                UpdateAccounts(updateAccounts)
                    .then(()=>{
                        toast.success("User Role is updated!", {
                            icon: {
                                iconClass: 'material-icons',  
                                iconChildren: 'check_circle_outline', 
                                iconTag: 'span'               
                            },
                            timeout: 7 * 1000,
                        });

                        HandleGetAccounts()
                    })           
            }

            return {
                uncheckAccounts,
                accounts,
                accountsRoles,
                ApproveUncheckAccounts,
                DenyUncheckAccounts,
                AccountStatus,
                UserRoles,
                HandleUpdateAccounts,
                isUpdateAccounts,
            }
        }

    });
</script>

<style lang="scss" scoped>
    .Admin{
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .row{
        padding: 5px;
        display: flex;
        flex-direction: row;
        justify-content: center;
    }

    .uncheck-account{
        margin: 0 auto;
    }

    .warning{
        color: orangered;
    }
</style>