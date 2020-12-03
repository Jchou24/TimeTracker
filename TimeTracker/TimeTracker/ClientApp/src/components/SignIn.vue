<template>
    <div class="SignIn">
        <div class="row row-inputs">
            <div class="row-input">
                Email: <input v-model="email" :disabled="isAuthenticated" type="email">
            </div>
            <div class="row warning">
                <p v-for="(err, idx) in errorMessages.Email" :key="idx">
                    {{err}}
                </p>
            </div>

            <div class="row-input">
                Password: <input v-model="password" :disabled="isAuthenticated" type="password">
            </div>
            <div class="row warning">
                <p v-for="(err, idx) in errorMessages.Password" :key="idx">
                    {{err}}
                </p>
            </div>
        </div>

        <div class="row row-buttons">
            <button @click="login" :disabled="isAuthenticated" >
                sign in
            </button>

            <button @click="logout" :disabled="!isAuthenticated">
                sign out
            </button>
        </div>

        <div class="row warning">
            <p v-for="(err, idx) in errorMessages.Fail" :key="idx">
                {{err}}
            </p>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api';
    // import { useStore } from 'vuex'
    import { ILogin, Login, Logout } from '@/api/authentication.ts'

    export default defineComponent({
        name: 'SignIn',
        setup(props, { emit, root }){
            const { $store } = root
            // const store = useStore()
            const store = $store
            const isAuthenticated = computed(() => store.state.authentication.isAuthenticated)
            const email = computed({
                get(){
                    return store.state.authentication.claims.email
                },
                set(value){
                    store.commit("authentication/SetEmail", value)
                }
            })
            const password = ref("")

            const GetEmptyErrorMessages = () => {
                return {
                    Email: "",
                    Password: "",
                    Fail: "",
                }
            }
            const errorMessages = reactive(GetEmptyErrorMessages())
            function EmptyErrorMessages(){
                errorMessages.Email = ""
                errorMessages.Password = ""
                errorMessages.Fail = ""
            }

            const logindata = computed(() => { 
                return {
                    email: email.value,
                    password: password.value,
                } as ILogin
            }) 

            function login(){
                Login(logindata.value as ILogin, store).then(() => {
                    EmptyErrorMessages()
                }).catch(error=>{
                    if (error.response.status == 400) {
                        EmptyErrorMessages()
                        errorMessages.Email = error.response.data["Email"]
                        errorMessages.Password = error.response.data["Password"]
                        errorMessages.Fail = error.response.data["Fail"]
                    }
                    // console.log(error.response)
                })

                emit('login-success')
            }

            function logout(){
                Logout(store)
            }

            return {
                isAuthenticated,
                email,
                password,
                errorMessages,

                login,
                logout
            }
        }

    });
</script>

<style lang="scss" scoped>
    .SignIn{
        display: flex;
        flex-direction: column;
    }

    .row{
        padding: 5px;
    }

    .row-inputs{
        display: flex;
        flex-direction: column;

        max-width: 300px;
        margin: 0 auto;

        .row-input{
            margin-left: auto;
            padding: 5px;
            
        }
    }

    .warning{
        color: orangered;
    }
</style>