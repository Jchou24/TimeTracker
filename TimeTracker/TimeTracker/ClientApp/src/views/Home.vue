<template>
    <div class="Home" :class="GetClass" >
        <div class="bg" />
        <div class="content">
            <Container>
                <Center>
                    <SimpleTransition mode="out-in" speed="animate__fastest">
                        <div class="default-screen" :class="{'with-buttons': isShowButtons}" v-if="!isBlurBackground" >
                            <Clock class="clock" />

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

                            <div class="github-icon" >
                                <a href="https://github.com/Jchou24/TimeTracker" target="_blank" rel="noopener noreferrer" >
                                    <v-btn icon class="button font-weight-black text-h6" x-large>
                                        <v-icon>mdi-github</v-icon>
                                    </v-btn>
                                </a>
                            </div>
                        </div>

                        <SignIn darkMode :width="550" v-on-clickaway="() => ReturnDefaultScreen()"
                            @loginSuccess="ReturnDefaultScreen"
                            v-if="isShowSignIn" />

                        <Register darkMode :width="550" v-on-clickaway="() => ReturnDefaultScreen()"
                            @registSuccess="HandleRegistSuccess"
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

            function HandleRegistSuccess() {
                setTimeout( ReturnDefaultScreen, 5000 )
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
                HandleRegistSuccess,
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

        .default-screen{
            @include vm-transition(all, 0.5s);

            height: 500px;
            &.with-buttons{
                height: 600px;
            }
        }

        .content{
            position: relative;
            z-index: 1;
            width: 90%;
            height: 95%;
            border-radius: 50px;
            @include vm-drop-shadow-1(0px, 0px, 15px, rgba(197, 195, 195, 0.5));

            $hover-width: 15px;
            $color-hover: rgba(197, 195, 195, 0.5);
            box-shadow: 
                0px 0px $hover-width $color-hover;

            background: url("~@/assets/img/forest/forest2.jpg");
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;

            @include vm-transition(all, 0.5s);
        }

        &.md, &.sm, &.xs{
            .content{
                width: 95%;
                height: 95%;
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

        .github-icon{
            position: absolute;
            right: 20px;
            bottom: 20px;
            .button{
                width: 50px;
                border-bottom: unset;
                box-shadow: unset;
                color: #daf6ff !important;
                text-shadow: 0 0 20px rgba(10, 175, 230, 1),  0 0 20px rgba(10, 175, 230, 0);                
           }
        }

        &.xs .buttons .button{
            width: 200px;
        }
        &.xs .buttons.github-icon .button{
            width: 50px;
        }
    }
</style>