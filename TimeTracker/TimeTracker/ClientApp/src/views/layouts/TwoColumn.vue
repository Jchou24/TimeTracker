<template>
    <div class="two-column" :class="{ 'active-sidebar':isShowSidebar  }">
        <SimpleTransition enterAnimation="animate__fadeInLeft" leaveAnimation="animate__fadeOutLeft" >
            <div class="fixed-sidebar" v-show="isShowSidebar">
                <div class="fixed-sidebar-background" />
                <div class="fixed-sidebar-content">
                    <slot name="left"></slot>
                </div>
            </div>
        </SimpleTransition>
        <div class="content">
            <slot name="right"></slot>
        </div>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, onBeforeMount, onMounted, onUnmounted, watch } from '@vue/composition-api'

    import SimpleTransition from '@/util/components/transition/SimpleTransition.vue'

    import { useWindowSize } from 'vue-use-web'
    import { GetSize, ScreenSize } from '@/util/breakPoint'
    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    export default defineComponent({
        name: 'TwoColumn',
        props:{
            mdiIcon:{
                type: String,
                default: "",
            },
            left:{
                type: Boolean,
                default: false,
            }
        },
        components:{
            SimpleTransition
        },
        setup(props, { root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>

            const { width, height } = useWindowSize({
                throttleMs: 100
            });
            const screenSize = computed( () => GetSize(width.value) )

            const isShowSidebar = computed( () => store.state.twoColumn.isShowSidebar )

            const ActiveTwoColumn = () => store.commit("twoColumn/ActiveTwoColumn")
            const DisActiveTwoColumn = () => store.commit("twoColumn/DisActiveTwoColumn")
            const ActiveSidebar = () => store.commit("twoColumn/ActiveSidebar")
            const DisActiveSidebar = () => store.commit("twoColumn/DisActiveSidebar")

            function InitSidebar(){
                switch (screenSize.value) {
                    case ScreenSize.sm:
                    case ScreenSize.xs:
                        DisActiveSidebar()
                        break;                
                    default:
                        ActiveSidebar()
                        break;
                }              
            }

            watch( screenSize, () => InitSidebar() )

            onBeforeMount( () => {
                ActiveTwoColumn()
                InitSidebar()
            })

            onUnmounted( () => {
                DisActiveTwoColumn()
            })

            return {
                isShowSidebar
            }
        }
    })
</script>

<style lang="scss" scoped>
    $width-sidebar: 350px;
    $width-scrollbar: 12px;

    .two-column{
        width: 100%;

        @include vm-ransition( padding-left, 0.2s );
        &.active-sidebar{
            padding-left: $width-sidebar;
        }
        padding-right: $width-scrollbar;
    }

    .two-column .fixed-sidebar{
        position: fixed;
        left: 0px;
        top: 0px;
        width: $width-sidebar;
        height: 100vh;
        display: flex;
        justify-content: center;
        z-index: 3;
        background: white;

        @include vm-drop-shadow-1( 5px, 0px, 3px, black );

        .fixed-sidebar-content{
            display: flex;
            flex-direction: column;
            padding-top: 100px;
            padding-bottom: 20px;
            overflow: auto;

            &::-webkit-scrollbar
            {
                width: 0px;
                height: 0px;
            }
        }

        .fixed-sidebar-background{
            position: absolute;
            background: $img-dark-wood;
            // filter: blur(2px);
            // filter: brightness(80%);
            filter: brightness(100%) opacity(90%);

            width: 100%;
            height: 100%;
        }
    }
</style>