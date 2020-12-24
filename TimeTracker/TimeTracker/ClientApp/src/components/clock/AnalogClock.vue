<template>
    <div class="AnalogClock">
        <VueAnalogClock class="analog-clock"
            :size="size"
            :borderWidth="borderWidth"
            borderColor="rgba(0, 0, 0, 0.5)" 

            backgroundColor="rgba(0, 0, 0, 0.6)"

            scaleType="roman"
            scaleColor="#b6c3c9"

            handType="line"

            hourColor="#b6c3c9"

            secondHandColor="#ff5722"
            minuteHandColor="#b6c3c9"
            hourHandColor="#7392a0"  
        />
    </div>
</template>

<script>
    import { AnalogClock } from 'vue-analog-clock'

    export default {
        name: "AnalogClock",
        components:{ 
            VueAnalogClock: AnalogClock
        },
        props:{
            size:{
                type: Number,
                default: 500,
            },
            borderWidth:{
                type: Number,
                default: 5,
            }
        },
    }
</script>

<style lang="scss" scoped>
    .analog-clock{
        @include vm-drop-shadow-1(0px, 0px, 30px, rgba(197, 195, 195, 0.5));
    }
</style>
