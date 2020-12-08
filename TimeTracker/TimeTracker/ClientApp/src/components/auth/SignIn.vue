<template>
    <div class="SignIn text-center">
        <v-dialog
            v-model="isOpenModalRef"
            width="500"            
            >
            <v-card
                :loading="isLoading"
                :disabled="isLoading"
                elevation="10"
                outlined
                shaped
                @keydown.enter.native="SignIn"                
                >
                <v-card-title class="flex-center text-h4 my-4" ><v-icon class="text-h4" left>mdi-account-circle</v-icon>登入</v-card-title>
                <v-divider class="mb-7" />
                <v-card-text class="px-10">
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
                </v-card-text>
                <v-divider class="mt-n4"></v-divider>
                <v-card-actions class="py-6 flex-center">
                    <v-btn class="pa-6 px-8 text-center"
                        color="primary"
                        :loading="isLoading"
                        elevation="2"
                        large
                        rounded                        
                        @click="SignIn" :disabled="isAuthenticated"
                    >
                        <v-icon left>mdi-send</v-icon>
                        Submit
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api';
    import { ILogin, Login } from '@/api/authentication.ts'

    export default defineComponent({
        name: 'SignIn',
        props:{
            isOpenModal:{
                type: Boolean,
            }
        },
        setup(props, { emit, root }){
            const { $store } = root
            const store = $store

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

            const GetEmptyErrorMessages = () => {
                return {
                    Email: [],
                    Password: [],
                    Fail: [],
                }
            }
            const errorMessages = reactive(GetEmptyErrorMessages())
            function EmptyErrorMessages(){
                errorMessages.Email = []
                errorMessages.Password = []
                errorMessages.Fail = []
            }

            const logindata = computed(() => { 
                return {
                    email: email.value,
                    password: password.value,
                } as ILogin
            }) 

            const isOpenModalRef = computed({
                get(){
                    return props.isOpenModal as boolean
                },
                set(value: boolean){
                    emit("update:isOpenModal", value)
                    password.value = ""
                    EmptyErrorMessages()
                }
            })

            const isLoading = ref(false)
            function SignIn(){
                if(isAuthenticated.value){
                    return
                }
                
                Login(logindata.value as ILogin, store, isLoading, () => {
                    EmptyErrorMessages()
                    password.value = ""
                    isOpenModalRef.value = false
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
                isOpenModalRef,
                isAuthenticated,
                isLoading,      
                
                email,
                password,
                errorMessages,

                SignIn,
            }
        }

    });
</script>

<style lang="scss" scoped>
</style>