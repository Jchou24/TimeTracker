<template>
    <transition-group
        appear
        v-on:before-enter="beforeEnter"
        v-on:enter="enter"
        >
        <slot></slot>
    </transition-group>
</template>

<script>
    // require velocity-animate
    // add data-index on element for "ripple"

    export default {
        name: "RippleTransitionFlip",
        props:{
            isFirstActive:{
                type: Boolean,
                default: false
            }
        },
        methods:{
            beforeEnter(el) {
                el.style.opacity = 0
                if(this.isFirstActive){
                    el.style.height = 0
                }
            },
            enter(el, done) {
                import('velocity-animate/velocity').then(Velocity => {
                    const idx = el.dataset.index % 20
                    const delay = idx * 150 + 300
                    setTimeout(function() {
                        // el.className += " animated flipInX"
                        // el.className += " animated rotateInDownLeft"
                        el.className += " rolldown"
                        el.style.height = "initial"
                        Velocity.default(
                            el,
                            { opacity: 1 },
                            { complete: done }
                        )
                    }, delay)
                })
            }
        }
    }
</script>

<style lang="scss" scoped>

</style>
