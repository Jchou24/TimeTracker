<template>
    <div id="app">
        <div id="nav">
            <router-link to="/">Home</router-link> |
            <router-link to="/about">About</router-link> |
            <router-link to="/testAPI">Test weather API</router-link> |
            <router-link to="/registration" v-if="!isAuthenticated">Regist</router-link> |
            <router-link to="/admin" v-if="isAdmin">Admin</router-link> |
            <router-link to="/signIn">SignIn</router-link> |
            
            {{authenticationInfo}}
            <router-link to="/settings" v-if="isAuthenticated"><span class="material-icons settings">settings</span></router-link>
        </div>
        <router-view/>
        <!-- <PageIdle /> -->
        <PageIdle :idleSeconds="3" :userConfirmSeconds="5" />
    </div>
</template>

<script lang="ts">
    // import { defineComponent, computed, watch, ref } from 'vue'
    import { defineComponent, computed, watch, ref } from '@vue/composition-api'
    // import { useStore } from 'vuex'
    import PageIdle from '@/components/PageIdle.vue'
    import { IsLogin } from '@/api/authentication.ts'
    import { IUserRole, UserRoles } from '@/models/authentication.ts'
    import { provideToast } from "vue-toastification/composition";

    export default defineComponent({
        name: 'App',
        components:{
          PageIdle
        },
        setup(props, { root }){
            provideToast()
            
            const { $store } = root
            // const store = useStore()
            const store = $store
            const unauthInfo = "You are not log in"
            // console.log( "store.state.authentication", store.state.authentication )
            // console.log( "store.state.authentication.claims.name", store.state.authentication.claims.name )

            const displayName = computed( () => 
                store.state.authentication.claims.name ? 
                store.state.authentication.claims.name : 
                store.state.authentication.claims.email)
            const NavigationMessage = () => `Hi, ${displayName.value}! You are now log in!`
            const authInfo = ref(NavigationMessage())
            watch( () => store.state.authentication.claims.name, (name: string) => {
              authInfo.value = NavigationMessage()
            })

            const isAuthenticated = computed( () => store.state.authentication.isAuthenticated )
            
            const authenticationInfo = computed(() =>{
              return isAuthenticated.value ? authInfo.value : unauthInfo
            })

            const isAdmin = computed( () => store.state.authentication.claims.userRoles.find( (x: IUserRole) => x.id == UserRoles.Admin ) )

            IsLogin(store)

            return {
                isAuthenticated,
                authenticationInfo,
                isAdmin,
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

    #nav {
        padding: 30px;

        a {
            font-weight: bold;
            color: #2c3e50;

            &.router-link-exact-active {
                color: #42b983;
            }
        }
    }
</style>

<style lang="scss" scoped>
    .material-icons.settings{
        position: absolute;
        margin-left: 5px;
        margin-top: -4px;
    }
</style>