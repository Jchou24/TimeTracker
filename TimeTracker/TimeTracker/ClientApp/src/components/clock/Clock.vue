<template>
    <div class="Clock">
        <AnalogClock :size="clockConfig.size" :borderWidth="clockConfig.borderWidth" />
        <DigitalClock :size="clockConfig.screenSize" />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent } from '@vue/composition-api';

    import AnalogClock from './AnalogClock.vue'
    import DigitalClock from './DigitalClock.vue'

    import { useWindowSize } from 'vue-use-web';
    import { GetSize, ScreenSize } from '@/util/breakPoint';

    interface IClockConfig{
        size: number;
        borderWidth: number;
        screenSize: string;
    }

    export default defineComponent({
        name: "Clock",
        props:{
            
        },
        components:{
            AnalogClock,
            DigitalClock,
        },
        setup(){


            const { width, height } = useWindowSize({
                throttleMs: 100
            });

            const screenSize = computed( () => GetSize(width.value) )

            const clockConfig = computed( () => {
                switch (screenSize.value) {
                    case ScreenSize.xs:
                        return {
                            size: 400,
                            borderWidth: 5,
                            screenSize: ScreenSize[screenSize.value]
                        } as IClockConfig
                
                    default:
                        return {
                            size: 500,
                            borderWidth: 5,
                            screenSize: ScreenSize[screenSize.value]
                        } as IClockConfig
                }
            })

            return {
                clockConfig
            }

        }
    })
</script>

<style lang="scss" scoped>
    .Clock{
        display: flex;
        justify-content: center;
        align-items: center;
        position: relative;
    }
</style>
