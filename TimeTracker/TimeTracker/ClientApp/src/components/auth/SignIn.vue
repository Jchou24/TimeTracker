<template>
    <div class="SignIn text-center" :class="{darkMode}">
        <v-card
            :loading="isLoading"
            :disabled="isLoading"
            elevation="10"
            outlined
            :shaped="!darkMode"
            @keydown.enter.native="SignIn"
            :width="width"
            >
            <v-card-title class="title flex-center text-h4 mb-4 pt-8 font-weight-black" ><v-icon class="title text-h4" left>mdi-account-circle</v-icon>登入</v-card-title>
            <v-divider class="mb-7" v-if="!darkMode" />
            <v-card-text class="px-10">
                <div class="content">
                    <v-text-field
                        v-model="email"
                        :error-messages="errorMessages.Email"
                        :error-count="errorMessages.Email.length"
                        label="Email"
                        prepend-icon="mdi-email"
                        outlined
                        dense
                    />
                    <v-text-field
                        v-model="password"
                        :error-messages="errorMessages.Password"
                        :error-count="errorMessages.Password.length"
                        label="Password"
                        type="password"
                        prepend-icon="mdi-key"
                        append-icon="mdi-eye"
                        outlined
                        dense
                    />
                    <p class="error--text" v-for="(err, idx) in errorMessages.Fail" :key="idx">
                        {{err}}
                    </p>
                </div>
            </v-card-text>
            <v-divider v-if="!darkMode" />
            <v-card-actions class="py-6 flex-center card-action">
                <v-btn class="pa-6 px-8 text-center"
                    :loading="isLoading"
                    elevation="2"
                    large
                    rounded
                    :color="darkMode ? 'accent' : 'primary' "
                    :block="darkMode"
                    @click="SignIn" :disabled="isAuthenticated"
                >
                    <v-icon left>mdi-send</v-icon>
                    Submit
                </v-btn>
            </v-card-actions>
        </v-card>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api';
    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { ILogin } from '@/models/authentication';
    import { IStore } from '@/models/store';
    import { Store } from 'vuex/types/index';

    export default defineComponent({
        name: 'SignIn',
        props:{
            width:{
                type: Number,
                default: 450,
            },
            darkMode:{
                type: Boolean,
                default: false,
            },
        },
        setup(props, { emit, root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>
            const router = $router

            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )

            const password = ref("")
            const isAuthenticated = computed(() => store.state.authentication.isAuthenticated)
            const email = computed({
                get(){
                    return store.state.authentication.claims.email
                },
                set(value){
                    store.commit("authentication/SetEmail", value)
                }
            })

            const GetEmptyErrorMessages = () => ({
                Email: [],
                Password: [],
                Fail: [],
            })

            const errorMessages = reactive(GetEmptyErrorMessages())
            function EmptyErrorMessages(){
                errorMessages.Email = []
                errorMessages.Password = []
                errorMessages.Fail = []
            }

            const logindata = computed(() => ({
                    email: email.value,
                    password: password.value,
                } as ILogin))

            function ResetPassword(){
                password.value = ""
                EmptyErrorMessages()
            }

            const isLoading = ref(false)
            function SignIn(){
                if(isAuthenticated.value){
                    return
                }
                
                authenticationAPIHandler.Login(logindata.value as ILogin, isLoading, () => {
                    EmptyErrorMessages()
                    password.value = ""
                    emit( "loginSuccess" )
                }, ( error ) => {
                    if (error.response.status == 400) {
                        EmptyErrorMessages()
                        errorMessages.Email = error.response.data["Email"] || []
                        errorMessages.Password = error.response.data["Password"] || []
                        errorMessages.Fail = error.response.data["Fail"] || []
                    }
                    // console.log(error.response)
                })
            }

            return {
                isAuthenticated,
                isLoading,      
                
                email,
                password,
                errorMessages,

                SignIn,
                ResetPassword,
            }
        }

    });
</script>

<style lang="scss" scoped>
    .SignIn .title{
        color: $color-title;
    }

    .SignIn.darkMode{
        .theme--light.v-card{
            border: unset;
            border-radius: 15px;
            padding-top: 0px;
            padding-bottom: 15px;
            background: linear-gradient(to bottom right, #7d7a8280, #38343e );

            .title{
                color: white;
                @include vm-drop-shadow-1(0px, 0px, 5px, rgba(197, 195, 195, 0.5));
            }

            .content{
                margin: 0 auto;
                display: flex;
                flex-direction: column;
                max-width: 400px;
                background: linear-gradient(to bottom right, #ffffff 75%, #dadada );;
                padding: 20px;
                padding-top: 30px;
                padding-bottom: 10px;
                border-radius: 15px;
                @include vm-drop-shadow-1(0px, 0px, 10px, rgba(197, 195, 195, 0.5));
            }

            .card-action{
                padding-top: 10px !important;
                margin: 0 auto;
                max-width: 400px;
            }
        }        
    }
</style>