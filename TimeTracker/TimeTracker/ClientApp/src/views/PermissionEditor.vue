<template>
    <Center class="PermissionEditor">
        <AdminUncheckAccounts @GetAccounts="HandleGetAccounts" />
        <AdminAccounts :accounts="accounts" @GetAccounts="HandleGetAccounts" />
    </Center>
</template>

<script lang="ts">
    import { defineComponent, reactive, ref } from '@vue/composition-api'

    import Center from './layouts/Center.vue'
    import AdminAccounts from '@/components/auth/AdminAccounts.vue'
    import AdminUncheckAccounts from '@/components/auth/AdminUncheckAccounts.vue'
    import { IClaims, IUserRole } from '@/models/authentication'
    import { AdminAPIHandler } from '@/api/admin'

    export default defineComponent({
        name: 'PermissionEditor',
        props:{

        },
        components:{
            Center,
            AdminAccounts,
            AdminUncheckAccounts,
        },
        setup(props, { root }){
            const { $store, $router } = root
            const store = $store
            const router = $router
            const adminAPIHandler = new AdminAPIHandler( store, router )

            const isLoading = ref(false)
            const accounts = ref([] as Array<IClaims>)
            function HandleGetAccounts(){
                adminAPIHandler.GetAccounts(isLoading, ( response )=>{
                    accounts.value = response.data as Array<IClaims>
                })
            }
            HandleGetAccounts()

            return {
                accounts,
                HandleGetAccounts
            }
        }
    })
</script>

