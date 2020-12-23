<template>
    <div class="Settings">
        <v-card
            elevation="10"
            width="600"
            outlined
            shaped
            >
            <v-card-title class="title flex-center text-h4 my-4 font-weight-black"><v-icon class="title text-h4" left>mdi-account-circle</v-icon>個人資訊/設定</v-card-title>
            <v-divider class="mb-4" />
            <v-card-title class="setting-info">
                <v-container fluid class="px-6">
                    <v-row no-gutters>
                        <v-col cols="4" >
                            Account status
                        </v-col>
                        <v-col cols="8">
                            <v-text-field :class="{'warn-account': accountRemind.length > 0}"
                                v-model="accountStatus"
                                :error-messages="accountRemind"
                                outlined
                                dense
                                disabled
                            />
                        </v-col>
                    </v-row>

                    <v-row no-gutters>
                        <v-col cols="4">
                            Email
                        </v-col>
                        <v-col cols="8">
                            <v-text-field
                                v-model="email"
                                outlined
                                dense
                                disabled
                            />                            
                        </v-col>
                    </v-row>

                    <v-row no-gutters>
                        <v-col cols="4">
                            Roles
                        </v-col>
                        <v-col cols="8">
                            <v-text-field
                                v-model="roles"
                                outlined
                                dense
                                disabled
                            />                            
                        </v-col>
                    </v-row>

                    <v-row no-gutters>
                        <v-col cols="4">
                            Name
                        </v-col>
                        <v-col cols="8" class="">
                            <v-text-field
                                v-model="name"
                                outlined
                                dense
                                counter
                                maxlength="256"
                                label="Edit Name"
                                append-icon="mdi-square-edit-outline"
                                @keydown.enter.native="HandleUpdateName"
                                :loading="isUpdateNameLoading"
                            >
                                <template v-slot:append>
                                    <v-btn depressed x-small text class="btn-upload-name"
                                        v-if="isUpdateName" @click="HandleUpdateName"
                                        >
                                        <v-icon>mdi-upload</v-icon>
                                    </v-btn>
                                </template>
                            </v-text-field>
                        </v-col>
                    </v-row>

                    <v-row no-gutters>
                        <v-col cols="4" class="reset-password" v-ripple
                            @click="ToggleIsResetPassword" >
                            Reset Password
                        </v-col>
                        <v-col cols="8" class="">
                            <v-text-field
                                type="password"
                                v-model="currentPassword"
                                :error-messages="passwordErrorMessage"
                                outlined
                                dense
                                :label="isResetPassword ? 'Current Password' : 'Click Reset Password to reset' "
                                :append-icon="isResetPassword ? 'mdi-eye' : '' "
                                :disabled="!isResetPassword"
                                @keydown.enter.native="HandleUpdatePassword"
                            />
                        </v-col>
                    </v-row>
                    <v-row no-gutters v-if="isResetPassword">
                        <v-col cols="4"></v-col>
                        <v-col cols="8" class="">
                            <v-text-field
                                type="password"
                                v-model="password"
                                outlined
                                dense
                                label="New Password"
                                append-icon="mdi-eye"
                                @keydown.enter.native="HandleUpdatePassword"
                            />
                        </v-col>
                        <v-col cols="4"></v-col>
                        <v-col cols="8" class="">
                            <v-text-field
                                type="password"
                                v-model="comfirmPassword"
                                outlined
                                dense
                                label="Comfirm Password"
                                append-icon="mdi-eye"
                                @keydown.enter.native="HandleUpdatePassword"
                            />
                        </v-col>
                        <v-col cols="4"></v-col>
                        <v-col cols="8" class="">
                            <v-btn class="text-capitalize"
                                color="primary"
                                :loading="isUpdatePasswordLoading"
                                elevation="2"
                                large
                                rounded                        
                                @click="HandleUpdatePassword" :disabled="!isUpdatePassword"
                            >
                                <v-icon left>mdi-send</v-icon>
                                Reset Password
                            </v-btn>
                        </v-col>
                    </v-row>
                </v-container>
            </v-card-title>
        </v-card>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, ref } from '@vue/composition-api'
    // import { useStore } from 'vuex'

    // import { useToast } from "vue-toastification";
    import { useToast } from "vue-toastification/composition"
    import { ToastSuccess } from '@/util/notification.ts'

    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { IUserRole, GetAccountRemind, ILogin, IUpdatePassword } from '@/models/authentication.ts'
    import { AccountStatus } from '@/models/constants/authentication.ts'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    export default defineComponent({
        name: 'Settings',
        setup(props, { emit, root }){
            const { $store, $router } = root
            // const store = useStore()
            const store = $store as Store<IStore>
            const router = $router
            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )
            const toast = useToast()

            const accountStatus = computed( () => AccountStatus[store.state.authentication.claims.accountStatus] )
            const accountRemind = computed( () => GetAccountRemind(store.state.authentication.claims.accountStatus) )

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
            const isUpdateNameLoading = ref(false)
            function HandleUpdateName(){
                if( !isUpdateName.value ){
                    return
                }

                authenticationAPIHandler.UpdateName( { name: name.value } as ILogin, isUpdateNameLoading)
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
            
            const isUpdatePasswordLoading = ref(false)            
            function HandleUpdatePassword(){
                if(!isUpdatePassword.value){
                    return
                }
                passwordErrorMessage.value = ""
                authenticationAPIHandler.UpdatePassword({ currentPassword: currentPassword.value, password: password.value } as IUpdatePassword, isUpdatePasswordLoading, ()=>{
                        ToastSuccess( "Password changed!.", toast )
                        isResetPassword.value = false
                        currentPassword.value = ""
                        password.value = ""
                        comfirmPassword.value = ""
                        passwordErrorMessage.value = ""
                    },( error )=>{
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
                isUpdateNameLoading,
                HandleUpdateName,
                isResetPassword,
                ToggleIsResetPassword,
                currentPassword,
                password,
                comfirmPassword,
                passwordErrorMessage,
                isUpdatePassword,
                isUpdatePasswordLoading,
                HandleUpdatePassword,
            }
        }
    });
</script>

<style lang="scss">
    .Settings .warn-account.v-input--is-disabled{
        color: $color-error;

        fieldset {
            border-color: $color-error;
        }

        input{
            color: $color-error;
        }
    }
</style>

<style lang="scss" scoped>
    .setting-info .col-4{
        padding-top: 3px;
    }

    .btn-upload-name{
        color: $color-primary;
    }

    .reset-password{
        cursor: pointer;
        height: 35px;
        border-radius: 15px;
    }

    .title{
        color: $color-title;
    }
</style>