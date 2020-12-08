<template>
    <div class="PageIdle">
        <!-- <PageIdleModal :isShowModal.sync="isShowUserConfirm" :isBackgroundCancellable="false">
            <div class="notification">
                <div><p>You will be logged out after {{remainUserComfirmSeconds}}</p></div>
                <button @click="UserConfirm"> Do not log out</button>
            </div>
        </PageIdleModal> -->

        <v-dialog
            v-model="isShowUserConfirm"
            width="500"
            persistent
            >
            <v-card
                :loading="true"
                elevation="10"
                outlined
                shaped  
                >
                <v-card-title class="pt-8 text-h4 flex-center">網頁閒置中</v-card-title>
                <v-card-text class="py-2 pb-6 text-h6">
                    即將在 {{remainUserComfirmSeconds}} 秒後自動登出
                </v-card-text>
                <v-divider class="mx-4" />
                <v-card-actions class="py-6 flex-center">
                    <div class="text-center">
                        <v-btn
                            color="primary"
                            elevation="2"
                            large
                            rounded                        
                            @click="UserConfirm"
                        >
                            <!-- <v-icon left>mdi-play-circle-outline</v-icon> -->
                            <!-- <v-icon left>mdi-circle-off-outline</v-icon> -->
                            不要登出
                        </v-btn>
                    </div>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { defineComponent, PropType, onMounted, onBeforeUnmount, ref, computed, watch } from '@vue/composition-api'
    // import { useRoute, useRouter } from 'vue-router'
    // import { useStore } from 'vuex'

    import $ from "jquery"
    // import { useToast } from "vue-toastification";
    import { useToast } from "vue-toastification/composition"
    import { ToastSuccess } from '@/util/notification.ts'
    import throttle from 'lodash.throttle'
    import debounce from 'lodash.debounce'

    import { AuthenticationAPIHandler } from '@/api/authentication.ts'
    import { IdleDetermineStates } from '@/models/pageIdle'
    // import { ToastID } from 'vue-toastification/src/types';
    import { ToastID } from 'vue-toastification/dist/types/src/types';
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    interface IProps{
        idleSeconds: number;
        userConfirmSeconds: number;
        maxIdleTimes: number;
        unidleEvents: string;
        isLogOutRedirect: boolean;
        LogOutRedirectPath: string;
    }


    export default defineComponent({
        name: 'PageIdle',
        props:{
            idleSeconds:{
                type: Number,
                default: 60 * 10,
                // required: true,
            },
            userConfirmSeconds:{
                type: Number,
                default: 60,
                // required: true,
            },
            maxIdleTimes:{
                type: Number,
                default: 2,
                // required: true,
            },    
            unidleEvents:{
                type: String,
                default: 'mousemove click mouseup mousedown keydown keypress keyup submit change mouseenter scroll resize dblclick'
            },
            isLogOutRedirect:{
                type: Boolean,
                default: true,
            },
            LogOutRedirectPath:{
                type: String,
                default: "/"
            }
        },
        setup(props: IProps, { root }){
            const { $router, $route, $store } = root
            // const store = useStore()
            // const router = useRouter()
            const store = $store as Store<IStore>
            const router = $router
            const route = $route

            const authenticationAPIHandler = new AuthenticationAPIHandler( store, router )

            const isLoading = ref(false)
            const remainUserComfirmSeconds = ref(props.userConfirmSeconds)
            const SetIdleDetermineStates = ( idleDetermineStates: IdleDetermineStates ) => store.commit("pageIdle/SetIdleDetermineStates", idleDetermineStates)
            // =================================================================
            const isShowUserConfirm = computed( () => store.state.pageIdle.isShowUserConfirm )
            const SetIsShowUserConfirm = (value: boolean) => store.commit("pageIdle/SetIsShowUserConfirm", value)
            // =================================================================
            const idleTimes = ref(store.state.pageIdle.idleTimes)
            const idleRemainTimes = computed( () => props.maxIdleTimes + 1 - idleTimes.value )
            const SetIdleTimes = (value: number) => store.commit("pageIdle/SetIdleTimes", value)

            function AddIdleTimes(){
                idleTimes.value ++
                SetIdleTimes(idleTimes.value)
            }
            // =================================================================
            const toast = useToast()
            let toastId: ToastID | undefined
            const SetIsShowLogOutNotification = (value: boolean) => store.commit("pageIdle/SetIsShowLogOutNotification", value)

            function OpenToast(){
                // console.log("OpenToast")
                return ToastSuccess( "User is logged out automatically.", toast, {
                    timeout: false,
                    onClick: () => SetIsShowLogOutNotification(false)
                })
            }

            function HandleLogOutNotification(isShowLogOutNotification: boolean){
                if(isShowLogOutNotification){
                    toast.dismiss(toastId as ToastID)
                    toastId = OpenToast()
                }else{
                    toast.dismiss(toastId as ToastID)
                }
            }

            watch( () => store.state.pageIdle.isShowLogOutNotification, debounce((isShowLogOutNotification: boolean) => {
                HandleLogOutNotification(isShowLogOutNotification)
            }, 250))
            // =================================================================
            function InitialParameters(){
                idleTimes.value = 0
            }

            function GetPassSeconds(startCountDownTimestamp?: Date): number | undefined{
                // read startCountDownTimestamp from local storage
                // startCountDownTimestamp may be setted from other tab
                const currentTimestamp = new Date()

                if (!startCountDownTimestamp) {
                    return undefined              
                }
                startCountDownTimestamp = new Date(startCountDownTimestamp)
                return (currentTimestamp.getTime() - startCountDownTimestamp.getTime()) / 1000
            }

            function SetRemainUserComfirmSeconds(){
                const passSeconds = GetPassSeconds(store.state.pageIdle.startUserConfirmTimestamp)
                if(!passSeconds){
                    return
                }
                remainUserComfirmSeconds.value = Math.ceil(props.userConfirmSeconds - passSeconds)
                remainUserComfirmSeconds.value = Math.max( remainUserComfirmSeconds.value, 0 )
                remainUserComfirmSeconds.value = Math.min( remainUserComfirmSeconds.value, props.userConfirmSeconds )
            }

            function StartTimer(){
                SetRemainUserComfirmSeconds()
                const intervalRunner = setInterval(()=>{                  
                    let startCountDownTimestamp, passSeconds

                    switch (store.state.pageIdle.idleDetermineStates) {
                        case IdleDetermineStates.PageIdle:
                            passSeconds = GetPassSeconds(store.state.pageIdle.startIdleTimestamp)
                            if(!passSeconds){
                                break
                            }

                            // console.log("PageIdle passSeconds", passSeconds)
                            if ( passSeconds >= props.idleSeconds) {
                                SetIdleDetermineStates(IdleDetermineStates.UserComfirm)
                            }
                            break

                        case IdleDetermineStates.UserComfirm:
                            SetRemainUserComfirmSeconds()

                            // console.log("UserComfirm")
                            if ( remainUserComfirmSeconds.value <= 0 ) {
                                SetIdleDetermineStates(IdleDetermineStates.Logout)
                            }
                            break

                        case IdleDetermineStates.Logout:
                            SetIdleDetermineStates(IdleDetermineStates.ByPass)
                            break        

                        case IdleDetermineStates.ByPass:
                        default:
                          break
                    }

                },250)
            }

            function SetTimeStamp(){
                const startIdleTimestamp = new Date()
                const startUserConfirmTimestamp = new Date (startIdleTimestamp.getTime() + props.idleSeconds * 1000 + 250)
                store.commit("pageIdle/SetStartIdleTimestamp", startIdleTimestamp)
                store.commit("pageIdle/SetStartUserConfirmTimestamp", startUserConfirmTimestamp)
            }

            function UserConfirm(){
                if( store.state.pageIdle.idleDetermineStates == IdleDetermineStates.UserComfirm){
                    SetTimeStamp()
                    SetIdleDetermineStates(IdleDetermineStates.PageIdle)
                    SetIsShowUserConfirm(false)
                }
            }   

            function PageIdleHandler(){
                if( store.state.pageIdle.idleDetermineStates == IdleDetermineStates.PageIdle ){
                    SetTimeStamp()
                }
            }

            function KeepAliveHandler(){
                if ( store.state.pageIdle.idleDetermineStates == IdleDetermineStates.ByPass ) {
                    return
                }
                authenticationAPIHandler.KeepAlive()
            }

            // =================================================================
            watch( () => store.state.pageIdle.idleDetermineStates, (newStates: IdleDetermineStates, oldStates: IdleDetermineStates) =>{
                if (oldStates == IdleDetermineStates.PageIdle && newStates == IdleDetermineStates.UserComfirm) {
                    remainUserComfirmSeconds.value = props.userConfirmSeconds
                    AddIdleTimes()
                    // console.log( "idleRemainTimes", idleRemainTimes.value )
                    if ( idleRemainTimes.value <= 0 ) {
                        SetIdleDetermineStates(IdleDetermineStates.Logout)
                    }else{
                        SetIsShowUserConfirm(true)
                    }
                }

                if (oldStates == IdleDetermineStates.UserComfirm && newStates == IdleDetermineStates.Logout) {
                    // console.log("IdleDetermineStates.Logout")
                    authenticationAPIHandler.Logout(isLoading)
                    SetIsShowLogOutNotification(true)
                }
            })

            watch( () => store.state.authentication.isAuthenticated, debounce((isAuthenticated: boolean) => {
                // console.log("watch store.state.authentication.isAuthenticated", store.state.authentication.isAuthenticated)
                // if(isAuthenticated == true && store.state.pageIdle.idleDetermineStates == IdleDetermineStates.ByPass){
                if(isAuthenticated == true){
                    SetIdleDetermineStates(IdleDetermineStates.PageIdle)
                    SetTimeStamp()
                }
                
                if(isAuthenticated == false){
                    SetIdleDetermineStates(IdleDetermineStates.ByPass)
                    InitialParameters()

                    if(props.isLogOutRedirect){
                        router.push(props.LogOutRedirectPath)
                    }
                }
            }, 250))

            onMounted(() => {
                $(window).bind(props.unidleEvents, throttle(PageIdleHandler, 200) )
                // Call KeepAlive when use is interecting to keep back-end session alive
                $(window).bind(props.unidleEvents, debounce(KeepAliveHandler, 30000));

                StartTimer()

                setTimeout(()=>{
                    HandleLogOutNotification(store.state.pageIdle.isShowLogOutNotification)  
                }, 500)              
            })

            return {
                isLoading,
                isShowUserConfirm,
                remainUserComfirmSeconds,
                UserConfirm,
            }
        }
    });
</script>

<style lang="scss" scoped>
    .notification{
        padding: 30px;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
</style>