<template>
    <div class="PageFlipper flex-container" v-if="isShowFlipper">
        <div class="flipper animate__animated" 
            :class=cls
            v-for="(setting, idx) in settings"
            :key=idx
            :style=setting.style
            :ref=GetRef(idx)
            ></div>
    </div>
</template>

<script>
    import $ from 'jquery'

    export default {
        name: "PageFlipper",
        components:{ 
        },
        props:{
            flipperNumbers:{
                type: Number,
                default: 8
            },
            baseRunTime:{
                type: Number,
                default: 1300
            },
            duration:{
                type: Number,
                default: 100
            },
            animateStyleCandidate:{
                type: Array,
                default: () => [
                    "animate__flipOutY",
                    "animate__zoomOutDown",
                    "animate__zoomOutLeft",
                    "animate__zoomOutRight",
                    "animate__zoomOutUp",
                    "animate__rollOut",
                    "animate__slideOutDown",
                    "animate__slideOutLeft",
                    "animate__slideOutRight",
                    "animate__slideOutUp",
                    "animate__rotateOut",
                    "animate__rotateOutDownLeft",
                    "animate__rotateOutDownRight",
                    "animate__rotateOutUpLeft",
                    "animate__rotateOutUpRight",
                ]
            }
        },
        data(){
            return {
                isShowFlipper: true,
                isSnimated: false,
                isSnimationDone: this.GetIsAnimationDone(),
                settings: this.GetSettings(),
                animateStyle: this.Sample(this.animateStyleCandidate),
            }
        },
        watch:{
            isAllAnimationDone(){
                if( this.isAllAnimationDone == true ){
                    this.isShowFlipper = false
                    this.$emit("AnimationDone")
                }
            }
        },
        methods:{
            ShuffleArray(array){
                for (let i = array.length - 1; i > 0; i--) {
                    const j = Math.floor(Math.random() * (i + 1));
                    [array[i], array[j]] = [array[j], array[i]];
                }
            },
            Sample(array){
                const random = Math.floor(Math.random() * array.length);
                return array[random]
            },
            GetRef(idx){
                return `flipper-${idx}`
            },
            GetIsAnimationDone(){
                return new Array(this.flipperNumbers).fill(false)
            },
            GetSettings(){
                const settings = [...Array(this.flipperNumbers).keys()]
                    for (let index = 0; index < settings.length; index++) {
                        const delay = settings[index]*this.duration
                        settings[index] = {
                            style: `animation-delay: ${delay}ms;`,
                        }
                    }
                this.ShuffleArray(settings)
                return settings
            },
            Init(){
                // const that = this

                this.isShowFlipper = true
                this.isSnimated = false
                this.isSnimationDone = this.GetIsAnimationDone()
                this.settings = this.GetSettings()

                this.BindAllTransitionEvent()                

                setTimeout(() => {
                    this.isSnimated = true
                }, this.baseRunTime)
            },
            BindAllTransitionEvent(){
                // Function from David Walsh: http://davidwalsh.name/css-animation-callback
                // function whichTransitionEvent(){
                //     var t, el = document.createElement("fakeelement");

                //     var transitions = {
                //         "transition"      : "transitionend",
                //         "OTransition"     : "oTransitionEnd",
                //         "MozTransition"   : "transitionend",
                //         "WebkitTransition": "webkitTransitionEnd"
                //     }

                //     for (t in transitions){
                //         if (el.style[t] !== undefined){
                //             return transitions[t];
                //         }
                //     }
                // }
                
                // let transitionEvent = whichTransitionEvent()
                const transitionEvent = "animationend"

                for (let index = 0; index < this.flipperNumbers; index++){
                    this.BindTransitionEvent(index, transitionEvent)
                } 
            },
            BindTransitionEvent(idx, transitionEvent){     
                // const that = this
                $(this.$refs[this.GetRef(idx)]).on(transitionEvent, () => {
                    this.$set(this.isSnimationDone, idx, true)
                    // console.log(idx+"animated")
                })
            }

        },
        computed:{
            cls:{
                get(){
                    const cls = {}
                    cls[this.animateStyle] = this.isSnimated
                    return cls
                }
            },
            isAllAnimationDone(){
                for( const isSnimationDone of this.isSnimationDone ){
                    if( isSnimationDone == false ){
                        return false
                    }
                }
                return true
            },
        },
        mounted(){
            this.Init()            
        }
    }
</script>

<style lang="scss" scoped>
    .flex-container{
        display: flex;
        flex-direction: row;
        // width: 150vw;
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
    }

    .flex-container .flipper{
        z-index: 100;
        flex: 1;
        // width: 250px;
        height: 150vh;
        margin-top: -150px;
        // margin-left: 10px;
        // margin-right: 10px;

        // background-color: #1c2f46;
        // background-image: map-get($imgs-heavy, grey_wash_wall);
        background-image: map-get($imgs-heavy, always_grey);
        // background-image: map-get($imgs-heavy, footer_lodyas);
        // background-image: map-get($imgs-heavy, dark_wood);
        background-repeat: repeat;

        -ms-transform: rotate(8deg); /* IE 9 */
        transform: rotate(8deg);

        // border: 5px solid #a1b7bd;

        &:first-child{
            margin-left: -120px;
        }

        &:last-child{
            margin-right: -80px;
        }

    }
</style>
