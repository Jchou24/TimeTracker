<template>
    <div class="Settings">
        <div class="row">
            <div class="row-title">
                Account status
            </div>
            <div class="row-info">
                {{accountStatus}}
            </div>
        </div>

        <div class="row warning" v-if="accountRemind.length > 0">
            {{accountRemind}}
        </div>

        <div class="row">
            <div class="row-title">
                Email
            </div>
            <div class="row-info">
                {{email}}
            </div>
        </div>

        <div class="row">
            <div class="row-title">
                Roles
            </div>
            <div class="row-info">
                {{roles}}
            </div>
        </div>

        <div class="row">
            <div class="row-title">
                Name
            </div>
            <div class="row-info">
                <input v-model="name" type="text">
                <button v-if="isUpdateName" @click="HandleUpdateName">
                    <span class="material-icons">
                        import_export
                    </span>
                </button>                
            </div>
        </div>

        <div class="row">
            <div class="row-title">
                <button @click="ToggleIsResetPassword">
                    Reset Password
                </button> 
            </div>
            <div class="row-info">
            </div>
        </div>

        <span v-if="isResetPassword">
            <div class="row" >
                <div class="row-title">
                    Current Password
                </div>
                <div class="row-info">
                    <input v-model="currentPassword" type="password">
                </div>
            </div>
            <div class="row warning" v-if="passwordErrorMessage.length > 0">
                {{passwordErrorMessage}}
            </div>

            <div class="row">
                <div class="row-title">
                    New Password
                </div>
                <div class="row-info">
                    <input v-model="password" type="password">
                </div>
            </div>

            <div class="row">
                <div class="row-title">
                    Comfirm Password
                </div>
                <div class="row-info">
                    <input v-model="comfirmPassword" type="password">
                </div>
            </div>

            <div class="row">
                <div class="row-title">
                </div>
                <div class="row-info">
                    <button :disabled="!isUpdatePassword" @click="HandleUpdatePassword">
                        Update Password
                    </button> 
                </div>
            </div>

            
        </span>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, ref } from '@vue/composition-api'
    // import { useStore } from 'vuex'

    // import { useToast } from "vue-toastification";
    import { useToast } from "vue-toastification/composition"

    import { ILogin, IUpdatePassword, UpdateName, UpdatePassword } from '@/api/authentication.ts'
    import { AccountStatus, IUserRole } from '@/models/authentication.ts'

    export default defineComponent({
        name: 'Settings',
        setup(props, { emit, root }){
            const { $store } = root
            // const store = useStore()
            const store = $store
            const toast = useToast()

            const accountStatus = computed( () =>{
                switch (store.state.authentication.claims.accountStatus) {
                    case AccountStatus.Uncheck:
                        return "Uncheck"  
                    case AccountStatus.Approved:
                        return "Approved"
                    case AccountStatus.Rejected:
                        return "Rejected"
                    case AccountStatus.Suspend:
                        return "Suspend"
                    default:
                        return ""
                }
            })
            const accountRemind = computed( () =>{
                switch (store.state.authentication.claims.accountStatus) {
                    case AccountStatus.Uncheck:
                        return "Please connect admin to activate your account."  
                    case AccountStatus.Approved:
                        return ""
                    case AccountStatus.Rejected:
                        return "Your account is rejected."
                    case AccountStatus.Suspend:
                        return "Your account is suspended."
                    default:
                        return ""
                }
            })

            const email = computed( () => store.state.authentication.claims.email )
            const roles = computed( () =>{
                const tmp: Array<string> = []
                store.state.authentication.claims.userRoles.forEach((role: IUserRole) => {
                    tmp.push(role.displayName)
                });
                return tmp.join(", ")
            })

            const name = ref(store.state.authentication.claims.name)
            const isUpdateName = computed( () => name.value != store.state.authentication.claims.name && name.value.length > 0 )
            function HandleUpdateName(){
                UpdateName( { name: name.value } as ILogin, store)
            }

            const isResetPassword = ref(false)
            const currentPassword = ref("")
            const password = ref("")
            const comfirmPassword = ref("")
            const passwordErrorMessage = ref("")
            const isUpdatePassword = computed( () => 
                currentPassword.value.length > 0 &&
                password.value == comfirmPassword.value && 
                password.value.length > 0 )
            function ToggleIsResetPassword(){
                isResetPassword.value = !isResetPassword.value
            }
            function HandleUpdatePassword(){
                passwordErrorMessage.value = ""
                UpdatePassword({ currentPassword: currentPassword.value, password: password.value } as IUpdatePassword)
                    .then(()=>{
                        toast.success("Password changed!.", {
                            icon: {
                                iconClass: 'material-icons',  
                                iconChildren: 'check_circle_outline', 
                                iconTag: 'span'               
                            },
                            timeout: 10 * 1000,
                        })
                        isResetPassword.value = false
                        currentPassword.value = ""
                        password.value = ""
                        comfirmPassword.value = ""
                        passwordErrorMessage.value = ""
                    })
                    .catch(error=>{
                        if (error.response.status == 400) {
                            passwordErrorMessage.value =error.response.data["Fail"][0]
                        }
                    })
            }

            return {
                accountStatus,
                accountRemind,
                email,
                roles,
                name,
                isUpdateName,
                HandleUpdateName,
                isResetPassword,
                ToggleIsResetPassword,
                currentPassword,
                password,
                comfirmPassword,
                passwordErrorMessage,
                isUpdatePassword,
                HandleUpdatePassword,
            }
        }
    });
</script>

<style lang="scss" scoped>
    .Settings{
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .row{
        padding: 5px;
        display: flex;
        flex-direction: row;
        justify-content: center;
    }

    .row .row-title{
        margin-right: 10px;
        width: 180px;
        border-right: 1px solid black;
    }

    .row .row-info{
        width: 200px;
        display: flex;
        flex-direction: row;
    }

    .warning{
        color: orangered;
    }
</style>