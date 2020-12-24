<template>
    <transition
        :mode="mode"
        :enter-active-class="enterClass"
        :leave-active-class="leaveClass"
        >
        <slot></slot>
    </transition>
</template>

<script>    

    import { computed, defineComponent, onBeforeMount, onMounted, onUnmounted } from '@vue/composition-api'    
    export default defineComponent({
        name: "SimpleTransition",
        props:{
            mode:{
                type: String,
            },
            enterAnimation:{
                type: String,
                default: "animate__fadeIn"
            },
            leaveAnimation:{
                type: String,
                default: "animate__fadeOut"
            },
            speed:{
                type: String,
                default: "animate__faster"
            },
        },
        setup(props, { root }){
            const enterClass = computed( () => `animate__animated ${props.enterAnimation} ${props.speed}` )
            const leaveClass = computed( () => `animate__animated ${props.leaveAnimation} ${props.speed}` )

            return {
                enterClass,
                leaveClass,
            }
        }
    })
</script>

<style lang="scss" scoped>
</style>
