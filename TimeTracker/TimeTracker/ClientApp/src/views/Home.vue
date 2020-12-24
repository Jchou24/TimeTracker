<template>
    <div class="Home" :class="GetClass" >
        <div class="bg" />
        <div class="content">
            <Container>
                <Center>
                    <SimpleTransition mode="out-in" speed="animate__fastest">
                        <div class="default-screen" v-if="!isBlurBackground" >
                            <Clock />

                            <SimpleTransition mode="out-in">
                                <div class="buttons mt-8" v-if="isShowButtons">
                                    <v-btn class="button mx-8 my-4 font-weight-black text-h6" large text
                                        @click="isShowRegist = true"
                                    > Regist </v-btn>
                                    <v-btn class="button mx-8 my-4 font-weight-black text-h6" large text
                                        @click="isShowSignIn = true"
                                    > Sign In </v-btn>
                                </div>
                            </SimpleTransition>
                        </div>

                        <SignIn darkMode :width="550" v-on-clickaway="() => ReturnDefaultScreen()"
                            @loginSuccess="isOpenModalRef = false"
                            v-if="isShowSignIn" />

                        <Register darkMode :width="550" v-on-clickaway="() => ReturnDefaultScreen()"
                            v-if="isShowRegist" />
                    </SimpleTransition>         
                </Center>
            </Container>
        </div>
        <PageFlipper class="page-fipper" />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, ref, watch } from '@vue/composition-api';

    import Container from './layouts/Container.vue'
    import Center from './layouts/Center.vue'
    import PageFlipper from '@/components/PageFlipper.vue'
    import SimpleTransition from '@/util/components/transition/SimpleTransition.vue'
    import Clock from '@/components/clock/Clock.vue'
    import Register from '@/components/auth/Register.vue'
    import SignIn from '@/components/auth/SignIn.vue'

    import { directive as onClickaway } from 'vue-clickaway2'
    import { useWindowSize } from 'vue-use-web';
    import { GetSize, ScreenSize } from '@/util/breakPoint';
    import { Store } from 'vuex/types/index';
    import { IStore } from '@/models/store';

    export default defineComponent({
        name: 'Home',
        components: {
            Container,
            Center,
            PageFlipper,
            SimpleTransition,
            Clock,
            Register,
            SignIn,
        },
        directives: {
            onClickaway: onClickaway,
        },
        setup( props, { refs, root } ){
            const { $store, $router } = root
            const store = $store as Store<IStore>

            const { width, height } = useWindowSize({
                throttleMs: 100
            });            

            const isShowRegist = ref(false)
            const isShowSignIn = ref(false)
            const isBlurBackground = computed( () => isShowRegist.value || isShowSignIn.value )
            const isShowButtons = computed( () => !store.state.authentication.isAuthenticated )

            function ReturnDefaultScreen(){
                isShowRegist.value = false
                isShowSignIn.value = false
            }

            

            const GetClass = computed( () => {
                const size = GetSize(width.value)
                const cls = {
                    'blur-background': isBlurBackground.value
                }
                Reflect.set(cls, ScreenSize[size], true)
                return cls
            })

            return {
                isShowRegist,
                isShowSignIn,
                isBlurBackground,
                isShowButtons,
                GetClass,
                ReturnDefaultScreen,
            }
        }
    });
</script>

<style lang="scss">
    .Home{
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;

        width: 100%;
        height: 100%;
        
        display: flex;
        justify-content: center;
        align-items: center;
        position: relative;
        
        .bg{
            z-index: 0;
            position: absolute;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;

            background: url("~@/assets/img/forest/forest2.jpg");
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;
            filter: blur(2px) opacity(0.8) brightness(80%);
        }

        .page-fipper{
            z-index: 2;
        }

        .content{
            z-index: 1;
            width: 80%;
            height: 90%;
            border-radius: 50px;
            @include vm-drop-shadow-1(0px, 0px, 15px, rgba(197, 195, 195, 0.5));

            background: url("~@/assets/img/forest/forest2.jpg");
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;

            @include vm-ransition(all, 0.5s);
        }

        &.md, &.sm, &.xs{
            .content{
                width: 90%;
                height: 90%;
            }
        }

        &.blur-background{
            .content{
                width: 100%;
                height: 100%;
                border-radius: 0px;

                .container{
                    backdrop-filter: blur(10px);
                }
            }
        }
    }

    .Home{
        .buttons{
            @media (max-width: 751px) { 
                display: flex;
                flex-direction: column-reverse;
                justify-content: center;
                align-items: center;
            }
        }
        
        .buttons .button{
            border-top-left-radius: 10px;
            border-top-right-radius: 10px;
            width: 250px;
            color: #daf6ff;
            text-shadow: 0 0 20px rgba(10, 175, 230, 1),  0 0 20px rgba(10, 175, 230, 0);
            border-bottom: 5px solid #daf6ff;
            box-shadow: 0 6px 8px -6px rgba(10, 175, 230, 1), 0 8px 6px -6px rgba(10, 175, 230, 0);
        }

        &.xs .buttons .button{
            width: 200px;
        }
    }
</style>