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
        },
        setup(props, { root }){
            const enterClass = computed( () => `animate__animated ${props.enterAnimation} animate__faster` )
            const leaveClass = computed( () => `animate__animated ${props.leaveAnimation} animate__faster` )

            return {
                enterClass,
                leaveClass,
            }
        }
    })
</script>

<style lang="scss" scoped>

</style>
