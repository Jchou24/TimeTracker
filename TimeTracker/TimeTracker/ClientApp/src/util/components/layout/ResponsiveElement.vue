<template>
    <div :class="size_class">
        <slot></slot>
    </div>
</template>

<script>
    // TODO
    // 設置breakpoint
    // 作成可以 directive 也可以 import 進來當作components
    // 可以參考 vue-responsive-components https://github.com/kelin2025/vue-responsive-components
    // 設置 emit
    //     當 resize 時觸發
    //         帶出現在的 size

    // 增加 named-slot，這樣就可以在不同的size下使用不同的組件
    // 增加 scoped-slot，將現在的size等等一些變數export出來
    import settings from "../../settings.js"
    import throttle from 'lodash.throttle'
    
    export default {
        name: "ResponsiveElement",
        components:{
            
        },
        props:{
            get_class:{
                type: Function,
                default: null,
            },
            wait_time:{
                type: Number,
                default: 300,
            }
        },
        data(){
            return {
                size_class: "",
            }
        },
        watch:{
            
        },
        computed:{

        },
        methods:{
            init(){
                let that = this
                
                that.recycle()
            },
            recycle(){
                let that = this
            },

            default_get_class(width){
                return width <= settings.screen_size.sm ? "sm" : ""
            },
            maintance_size_class(){
                let width = this.$el.offsetWidth
                let get_class = this.get_class || this.default_get_class

                this.size_class = get_class(width)
            },
            resize(){
                let that = this

                that.maintance_size_class()
                that.$emit("resized")
                // console.log("resized")
            }
        },
        mounted(){
            let that = this
            that.maintance_size_class()
            import('resize-sensor').then(ResizeSensor => {
                ResizeSensor.default(that.$el, throttle(that.resize, that.wait_time))
            })
        },
        updated(){

        },
        beforeDestroy(){
            this.recycle()
        }
    }
</script>

<style lang="scss" scoped>

</style>
