<template>
    <transition   
        :mode="mode"
        enter-active-class="animate__animated animate__fadeIn animate__faster"
        leave-active-class="animate__animated animate__fadeOut animate__faster"
        >
        <slot></slot>
    </transition>
</template>

<script>    
    export default {
        name: "FadeInOutTransition",
        props:{
            mode:{
                type: String,
            },
        },
    }
</script>

<style lang="scss" scoped>

</style>
