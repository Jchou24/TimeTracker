<template>
    <div class="Register" :class="{darkMode}">
        <SimpleTransition mode="out-in">
            <v-card
                :loading="isLoading"
                :disabled="isLoading"
                elevation="10"
                :width="width"
                outlined
                shaped
                v-if="!isRegistOk"
                :key="1"
                @keydown.enter.native="HandleRegist"
                >
                <v-card-title class="title flex-center text-h4 mb-4 pt-8"><v-icon class="title text-h4" left>mdi-account-plus</v-icon>註冊</v-card-title>
                <v-divider class="mb-4" v-if="!darkMode" />
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
                            counter
                            maxlength="256"
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
                        <v-text-field
                            v-model="comfirmPassword"
                            label="Comfirm Password"
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
                <v-card-actions class="py-6 flex-center card-action" :class="{'disabled': !isPasswordComfirmed}">
                    <v-btn class="pa-6 px-8 text-center"
                        :loading="isLoading"
                        elevation="2"
                        large
                        rounded
                        :color="darkMode ? 'accent' : 'primary' "
                        :block="darkMode"              
                        @click="HandleRegist" :disabled="!isPasswordComfirmed"
                    >
                        <v-icon left>mdi-send</v-icon>
                        Submit
                    </v-btn>
                </v-card-actions>
            </v-card>
            <v-card
                :loading="isLoading"
                elevation="15"
                width="500"
                height="100"
                outlined
                shaped
                v-ripple
                color="primary"
                v-else
                :key="2"
                >
                <v-container fluid fill-height >
                    <v-card-title class="regist-success text-h4 margin-center font-weight-bold"><v-icon class="text-h4" left>mdi-check-circle-outline</v-icon>註冊成功</v-card-title>
                </v-container>            
            </v-card>
        </SimpleTransition>
        
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive, onMounted } from '@vue/composition-api';

    import SimpleTransition from '@/util/components/transition/SimpleTransition.vue'

    import { useToast } from "vue-toastification/composition"
    import { ToastSuccess } from '@/util/notification.ts'

    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { ILogin } from '@/models/authentication';
    import { IStore } from '@/models/store';
    import { Store } from 'vuex/types/index';

    export default defineComponent({
        name: 'Register',
        props: {
            width:{
                type: Number,
                default: 500,
            },
            darkMode:{
                type: Boolean,
                default: false,
            },
        },
        components:{
            SimpleTransition,
        },
        setup(props, { emit, root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>
            const router = $router
            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )
            const toast = useToast()

            const email = ref("")
            const password = ref("")
            const comfirmPassword = ref("")
            const isRegistOk = ref(false)
            const isPasswordComfirmed = computed( () => password.value == comfirmPassword.value && password.value.length > 0 )
            const isLoading = ref(false)

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

            function HandleRegist(){
                if (!isPasswordComfirmed.value){
                    return                    
                }
                authenticationAPIHandler.Regist(logindata.value as ILogin, isLoading, () => {
                    EmptyErrorMessages()
                    isRegistOk.value = true
                    ToastSuccess( "Registration success! Please connect admin to activate your account.", toast, { timeout: 15 * 1000 })
                }, ( error ) => {
                    console.log( error )
                    isRegistOk.value = false
                    if (error.response.status == 400) {
                        EmptyErrorMessages()
                        errorMessages.Email = error.response.data["Email"] || []
                        errorMessages.Password = error.response.data["Password"] || []
                        errorMessages.Fail = error.response.data["Fail"] || []
                    }
                    console.log(error.response)
                })
            }

            const isAuthenticated = computed( () => store.state.authentication.isAuthenticated )
            return {
                email,
                password,
                comfirmPassword,
                errorMessages,
                HandleRegist,
                isRegistOk,
                isPasswordComfirmed,
                isAuthenticated,
                isLoading,
            }
        }
    });
</script>


<style lang="scss" scoped>
    .Register .regist-success{
        $color: #bbe9d4;
        color: $color;
        .theme--light.v-icon{
            color: $color;
        }
    }

    .Register .title{
        color: $color-title;
    }

    .Register.darkMode{
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
                padding: 0;
                padding-top: 10px !important;
                margin: 0 auto;
                max-width: 400px;

                button:disabled,
                button[disabled]{
                    background-color: #576b75!important;
                }

                &.disabled{
                    cursor: not-allowed !important;
                }
            }
        }        
    }
</style>
