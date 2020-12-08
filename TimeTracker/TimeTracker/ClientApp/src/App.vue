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

    import debounce from 'lodash.debounce';    
    import { provideToast, useToast } from "vue-toastification/composition";
    // Also import the toast's css
    import "vue-toastification/dist/index.css";

    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { GetRedirectPath } from './router/routeConfigs';
    import { ValidationResults } from './models/authValidation';

    // import { WSHandler } from '@/api/webSocket'

    export default defineComponent({
        name: 'App',
        components:{
            Nav,
            NavProgress,
            PageIdle,
        },
        setup(props, { root }){
            const { $store, $router } = root
            // const router = useRouter()
            // const store = useStore()
            const store = $store
            const router = $router

            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )
            
            provideToast({ toastClassName: "toast-notification" })
            const toast = useToast()
            store.commit("SetNotificator", toast)
            
            const isLoading = ref(false)
            authenticationAPIHandler.IsLogin(isLoading)
            watch( isLoading, (value: boolean) => {
                value ? store.commit("TurnOnLoading") : store.commit("TurnOffLoading")
            })



            watch( () => store.state.authentication.isAuthenticated, debounce((isAuthenticated: boolean) => {                
                if(isAuthenticated == false){
                    router.push(GetRedirectPath(ValidationResults.logout))
                }
            }, 250))






            // const wsHandler = new WSHandler()
            // const connection = wsHandler.connection

            // connection.on("broadcastMessage", data => {
            //     console.log(data);
            // });

            // connection.on("GetUserInfo", (dataX, dataY) => {
            //     console.log( "I will get user info", dataX, dataY);
            // });


            // function InitWS(isAuthenticated: boolean){
            //     if(isAuthenticated){
            //         connection.start()
            //             .then(() => connection.invoke("send", "MyName", "Hello"));
            //     }else{
            //         connection.stop()
            //     }
            // }
            // InitWS(store.state.authentication.isAuthenticated)
            // watch( () => store.state.authentication.isAuthenticated, debounce((isAuthenticated: boolean) => {                
            //     InitWS(isAuthenticated)
            // }, 250))

            
            
            

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