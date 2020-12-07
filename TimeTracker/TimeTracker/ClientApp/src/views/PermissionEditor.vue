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
    import { IUserInfoDetail, IUserRole } from '@/models/authentication'
    import { GetAccounts } from '@/api/admin'

    export default defineComponent({
        name: 'PermissionEditor',
        props:{

        },
        components:{
            Center,
            AdminAccounts,
            AdminUncheckAccounts,
        },
        setup(props){

            const isLoading = ref(false)
            const accounts = ref([] as Array<IUserInfoDetail>)
            function HandleGetAccounts(){
                return GetAccounts(isLoading)
                    .then((response)=>{
                        accounts.value = response.data
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

