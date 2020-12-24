<template>
    <!-- <Container> -->
    <Container :isFullHeight="false">
        <Center class="PermissionEditor">
            <Tab :tabTitles="tabTitles">
                <v-tab-item>
                    <AdminAccounts class="add-margin"
                        :width="800"
                        :accounts="accounts" 
                        @GetAccounts="HandleGetAccounts" />
                </v-tab-item>

                <v-tab-item>
                    <AdminUncheckAccounts class="add-margin"
                        :width="800"
                        @GetAccounts="HandleGetAccounts" />
                </v-tab-item>
            </Tab>
        </Center>
    </Container>
</template>

<script lang="ts">
    import { defineComponent, reactive, ref } from '@vue/composition-api'

    import Container from './layouts/Container.vue'
    import Center from './layouts/Center.vue'
    import Tab from './layouts/Tab.vue'
    import AdminAccounts from '@/components/auth/AdminAccounts.vue'
    import AdminUncheckAccounts from '@/components/auth/AdminUncheckAccounts.vue'

    import { IClaims, IUserRole } from '@/models/authentication'
    import { AdminAPIHandler } from '@/api/admin'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    export default defineComponent({
        name: 'PermissionEditor',
        props:{

        },
        components:{
            Container,
            Center,
            Tab,
            AdminAccounts,
            AdminUncheckAccounts,
        },
        setup(props, { root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>
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

            const tabTitles = [{
                    text: "Accounts",
                    icon: "mdi-account-group",
                },{
                    text: "Uncheck Accounts",
                    icon: "mdi-account-alert",
                }]

            return {
                tabTitles,
                accounts,
                HandleGetAccounts
            }
        }
    })
</script>

<style lang="scss" scoped>
    .add-margin{
        margin: 20px;
    }
</style>