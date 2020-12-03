<template>
    <div class="Registration">
        <div class="registration-form" v-if="!isRegistOk">
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

                <div class="row-input">
                    Comfirm Password: <input v-model="comfirmPassword" :disabled="isAuthenticated" type="password">
                </div>
            </div>

            <div class="row row-buttons">
                <button @click="HandleRegist" :disabled="!isPasswordComfirmed">
                    Regist
                </button>
            </div>

            <div class="row warning">
                <p v-for="(err, idx) in errorMessages.Fail" :key="idx">
                    {{err}}
                </p>
            </div>
        </div>
        <div class="registration-ok" v-else>
            Registration OK!
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api';
    // import { useStore } from 'vuex'

    // import { useToast } from "vue-toastification";
    import { useToast } from "vue-toastification/composition"

    import { ILogin, Regist } from '@/api/authentication.ts'

    export default defineComponent({
        name: 'Registration',
        props: {

        },
        setup(props, { emit, root }){
            const { $store } = root
            // const store = useStore()
            const store = $store
            const toast = useToast()

            const email = ref("")
            const password = ref("")
            const comfirmPassword = ref("")
            const isRegistOk = ref(false)
            const isPasswordComfirmed = computed( () => password.value == comfirmPassword.value && password.value.length > 0 )

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

            function HandleRegist(){
                Regist(logindata.value as ILogin, store).then(() => {
                    EmptyErrorMessages()
                    isRegistOk.value = true
                    toast.success("Registration success! Please connect admin to activate your account.", {
                        icon: {
                            iconClass: 'material-icons',  
                            iconChildren: 'check_circle_outline', 
                            iconTag: 'span'   
                        },
                        timeout: 20 * 1000
                    })
                }).catch(error=>{
                    isRegistOk.value = false
                    if (error.response.status == 400) {
                        EmptyErrorMessages()
                        errorMessages.Email = error.response.data["Email"]
                        errorMessages.Password = error.response.data["Password"]
                        errorMessages.Fail = error.response.data["Fail"]
                    }
                    console.log(error.response)
                })
            }

            return {
                email,
                password,
                comfirmPassword,
                errorMessages,
                HandleRegist,
                isRegistOk,
                isPasswordComfirmed,
            }
        }
    });
</script>


<style lang="scss" scoped>
    .registration-form{
        display: flex;
        flex-direction: column;
    }

    .row{
        padding: 5px;
    }

    .row-inputs{
        display: flex;
        flex-direction: column;

        max-width: 500px;
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
