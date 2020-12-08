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

        <NavProgress/>
        <Nav />

        <v-main>
            <v-container fluid fill-height >
                <transition   
                    mode="out-in"
                    enter-active-class="animate__animated animate__fadeIn animate__faster"
                    leave-active-class="animate__animated animate__fadeOut animate__faster"
                >
                    <router-view></router-view>
                </transition>
            </v-container>
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
    import NavProgress from '@/components/nav/NavProgress.vue'
    import PageIdle from '@/components/auth/PageIdle.vue'

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
            NavProgress,
            PageIdle,
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
            
            provideToast({ toastClassName: "toast-notification" })
            const toast = useToast()
            store.commit("SetNotificator", toast)
            
            const isLoading = ref(false)
            authenticationAPIHandler.IsLogin(isLoading)
            watch( isLoading, (value: boolean) => {
                value ? store.commit("TurnOnLoading") : store.commit("TurnOffLoading")
            })

            const wsHandler = new WSHandler(store, router)
            store.commit("SetWSHandler", wsHandler)
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
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }

    .toast-notification{
        width: 400px;
    }
</style>

<style lang="scss" scoped>
    
</style>