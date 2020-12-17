<template>
    <v-app-bar class="Nav drop-shadow-2 px-16" app>
        <NavLinkItem class="home-btn" routeConfigsKey="Home" displayName />        
        <NavLinkItem v-for="(navItem, idx) in navLeftItems" 
            :routeConfigsKey="navItem"
            mdiIcon displayName left
            :key="'left'+idx" />

        <v-spacer/>

        <v-toolbar-title class="auth-info v-btn">{{ authenticationInfo }}</v-toolbar-title>
        <NavLinkItem v-for="(navItem, idx) in navRightItems" 
            :routeConfigsKey="navItem"
            mdiIcon displayName left
            :key="'right'+idx" />
            
        <NavLinkItem class="individual-settings" routeConfigsKey="IndividualSettings" mdiIcon v-if="isAuthenticated" />
        <NavLinkItem routeConfigsKey="Registration" mdiIcon displayName left v-if="!isAuthenticated" />

        <NavItem mdiIcon="mdi-logout" left @click.native="SignOut" v-if="isAuthenticated">登出</NavItem>
        <NavItem mdiIcon="mdi-login" left @click.native="isOpenModal=true" v-else>登入</NavItem>
        <SignIn :isOpenModal.sync="isOpenModal" />

        <NavProgress/>
    </v-app-bar>
</template>

<script lang="ts">
    import { defineComponent, computed, watch, ref } from '@vue/composition-api'

    import NavProgress from '@/components/nav/NavProgress.vue'
    import NavLinkItem from "./NavLinkItem.vue"
    import NavItem from "./NavItem.vue"
    import SignIn from "@/components/auth/SignIn.vue"    
    
    import { routeConfigs } from '@/router/routeConfigs'
    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { IUserRole } from '@/models/authentication.ts'
    import { UserRoles } from '@/models/constants/authentication.ts'
    import { allNavLeftItems, allNavRightItems, FiltNavItems } from '@/models/nav.ts'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'    

    export default defineComponent({
        name: 'Nav',
        props:{

        },
        components:{
            NavProgress,
            NavLinkItem,
            NavItem,
            SignIn,
        },
        setup(props, { root }){
            const { $store, $router } = root
            // const store = useStore()
            const store = $store as Store<IStore>
            const router = $router
            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )

            const navLeftItems = computed( () => FiltNavItems( allNavLeftItems, store ))
            const navRightItems = computed( () => FiltNavItems( allNavRightItems, store ))

            const isOpenModal = ref(false)

            const isLoading = ref(false)
            function SignOut(){
                authenticationAPIHandler.Logout(isLoading)
                isOpenModal.value = false
            }
            
            watch( isLoading, (value: boolean) => {
                value ? store.commit("TurnOnLoading") : store.commit("TurnOffLoading")
            })
            
            const unauthInfo = "Please sign in to continue."
            // console.log( "store.state.authentication", store.state.authentication )
            // console.log( "store.state.authentication.claims.name", store.state.authentication.claims.name )

            const displayName = computed( () => 
                store.state.authentication.claims.name ? 
                store.state.authentication.claims.name : 
                store.state.authentication.claims.email)
            const NavigationMessage = () => `Hi ${displayName.value}!`
            const authInfo = ref(NavigationMessage())
            watch( () => store.state.authentication.claims.name, (name: string) => {
                authInfo.value = NavigationMessage()
            })

            const isAuthenticated = computed( () => store.state.authentication.isAuthenticated )
            const authenticationInfo = computed(() =>{
              return isAuthenticated.value ? authInfo.value : unauthInfo
            })            

            return {
                routeConfigs,
                navLeftItems,
                navRightItems,
                isAuthenticated,
                isOpenModal,
                SignOut,
                authenticationInfo,
            }
        }      
    })
</script>

<style lang="scss" scoped>
    .Nav{
        background-image: radial-gradient(circle, #ffffff00 80%, #a09e9e), map-get($imgs-heavy, grey_wash_wall);
        background-repeat: repeat;
    }

    .home-btn{
        margin-top: 3px;
    }

    .auth-info.v-btn{
        text-transform: unset;
        letter-spacing: initial;
        margin-left: 15px;
        margin-right: 15px;
        margin-top: 2px;
    }

    .individual-settings button{
        min-width: 60px;
    }
</style>

<style lang="scss">
    .Nav .individual-settings .v-btn:not(.v-btn--round).v-size--x-large{
        // min-width: 55px;
        margin-right: 5px;
    }

    // $color: #2c3e50;
    // $active-link-color: #a1d4bd;
    // $color: #89d1af;
    $color: #d3d3d3;
    $active-link-color: #2a3d50;
    // $active-link-color: $color-error;
    a.NavLinkItem{
        text-decoration: none;
        
        .v-btn{
            font-weight: bold !important;
            color: $color !important;
        }        

        // &.router-link-exact-active .v-btn{
        &.router-link-custom-active .v-btn{
            color: $active-link-color !important;
            @include vm-text-shadow-1( 12px, #ffffff );
        }
    }

    .auth-info.v-btn,
    .NavItem{
        font-weight: bold !important;
        color: $color !important;
    }

    .NavItem.theme--light.v-btn:not(.v-btn--flat):not(.v-btn--text):not(.v-btn--outlined){
        background-color: rgba(255, 255, 255, 0);
    }

    .v-btn:not(.v-btn--round).v-size--x-large{
        min-width: 50px;
        padding: 0 10px;
    }
</style>