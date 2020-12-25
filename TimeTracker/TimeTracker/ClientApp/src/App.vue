<template>
    <!-- <div id="app">
        <Nav />
        <router-view/>
        <PageIdle />
        <PageIdle :idleSeconds="3" :userConfirmSeconds="5" />
    </div> -->

    <v-app>
        <!-- <v-navigation-drawer app> -->
            <!-- -->
        <!-- </v-navigation-drawer> -->

        <Nav />

        <v-main class="main">
            <SimpleTransition mode="out-in">
                <router-view></router-view>
            </SimpleTransition>
        </v-main>

        <!-- <v-footer app> -->
            <!-- -->
        <!-- </v-footer> -->

        <!-- <PageIdle :isLogOutRedirect="false" /> -->
        <!-- <PageIdle :isLogOutRedirect="false" :idleSeconds="3" :userConfirmSeconds="5000" /> -->
    </v-app>
</template>

<script lang="ts">
    // import { defineComponent, computed, watch, ref } from 'vue'
    import { defineComponent, computed, watch, ref, onMounted } from '@vue/composition-api'

    import Nav from '@/components/nav/Nav.vue'
    import PageIdle from '@/components/auth/PageIdle.vue'
    import SimpleTransition from '@/util/components/transition/SimpleTransition.vue'

    import debounce from 'lodash.debounce'
    import { provideToast, useToast } from "vue-toastification/composition";
    import "vue-toastification/dist/index.css";

    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { GetRedirectPath, routeConfigs } from './router/routeConfigs';

    import { WSHandler } from '@/api/webSocket'
    import { IStore } from './models/store'
    import { Store } from 'vuex/types/index'
    import { GetAccountRemind, IUserRole, ValidationResults } from './models/authentication'
    import { AccountStatus } from './models/constants/authentication'
    import { ToastError, ToastWarning } from './util/notification'
    import { ValidateAuth } from './util/authValidation'

    export default defineComponent({
        name: 'App',
        components:{
            Nav,
            PageIdle,
            SimpleTransition,
        },
        setup(props, { root }){
            const { $store, $router, $route } = root
            // const router = useRouter()
            // const store = useStore()
            const store = $store as Store<IStore>
            const router = $router
            const route = $route
            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )
            // =================================================================
            
            provideToast({ 
                toastClassName: "toast-notification",
                transition: "Vue-Toastification__fade",
                draggable: false,
            })
            const toast = useToast()
            store.commit("notification/SetNotificator", toast)
            
            const isLoading = ref(false)
            authenticationAPIHandler.IsLogin(isLoading)
            watch( isLoading, (value: boolean) => {
                value ? store.commit("TurnOnLoading") : store.commit("TurnOffLoading")
            })

            const wsHandler = new WSHandler(store, router)
            store.commit("webSocket/SetWSHandler", wsHandler)

            // setInterval( () => console.log("WS state:", wsHandler.connection.state), 2000 )
            // =================================================================
            // Watch Account Status
            watch( () => store.state.authentication.claims.accountStatus, debounce((accountStatus: AccountStatus) => {
                if( !store.state.authentication.isAuthenticated ){
                    return
                }

                if( accountStatus != AccountStatus.Approved ){
                    ToastError( GetAccountRemind(accountStatus)), toast
                    router.push(GetRedirectPath(ValidationResults.invalidAccountStatus))
                }
            }, 250))

            watch( () => store.state.authentication.isAuthenticated, debounce((isAuthenticated: boolean) => {
                // Handle log out
                if( !isAuthenticated ){
                    // ToastWarning("", toast)
                    router.push(GetRedirectPath(ValidationResults.invalidAuthentication))
                }
            }, 250))

            let pathName = route.name
            router.afterEach((to, from) => {
                pathName = to.name
            })
            watch( () => store.state.authentication.claims.userRoles, debounce((userRoles: Array<IUserRole>) => {
                if( !store.state.authentication.isAuthenticated ){
                    return
                }

                if( pathName && ValidateAuth(pathName, store, routeConfigs) !== ValidationResults.ok ){
                    ToastWarning(`Sorry! You don't have sufficient permission.`, toast)
                    router.push(GetRedirectPath(ValidationResults.invalidRole))
                }
            }, 250) )
            // =================================================================
            return {

            }
        }

    })
</script>

<style lang="scss">
    .Vue-Toastification__container{
        padding-top: 68px;

        .toast-notification{
            width: 400px;
        }
    }

    html{
        // background: url("~@/assets/img/noisy_grid2.png");
        // background: url("~@/assets/img/vichy.png");
        
        // background: url("~@/assets/img/noisy_grid.png");
        // background: url("~@/assets/img/fabric_of_squares_gray.png");
        // background: url("~@/assets/img/whitey-light.png");
        background: url("~@/assets/img/whitey.png");

        .theme--light.v-application{
            background: unset;
        }
    }
</style>

<style lang="scss" scoped>
    
</style>