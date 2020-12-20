<template>
    <div class="TaskPeriodSimpleSummary">
        <FadeInOutTransition mode="out-in">
            <v-simple-table class="summary" dense fixed-header height="100%" v-if="isShowContent && summary.length > 0">
                <template v-slot:default>
                    <thead>
                        <tr>
                            <th class="text-center date">
                                Date
                            </th>
                            <th class="text-center">
                                Time
                            </th>
                            <th class="text-center">
                                Over Time
                            </th>
                        </tr>
                    </thead>
                    <RippleTransitionFlip tag="tbody" :setNumber="7">
                        <tr v-for="(dayCount, idx) in summary"
                            :class="GetClass(dayCount)"
                            :key="idx"
                            :data-index="idx"
                            >
                            <td class="text-center">{{ dayCount.date }}</td>
                            <td class="text-center">{{ dayCount.consumeTime }}</td>
                            <td class="text-center">{{ dayCount.overtime }}</td>
                        </tr>
                        <tr class="total"
                            :key="summary.length"
                            :data-index="0"
                            >
                            <td class="text-center">Total</td>
                            <td class="text-center">{{total.consumeTime}}</td>
                            <td class="text-center">{{total.overtime}}</td>
                        </tr>
                    </RippleTransitionFlip>
                </template>
            </v-simple-table>
        </FadeInOutTransition>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch  } from '@vue/composition-api'

    import FadeInOutTransition from '@/util/components/transition/FadeInOutTransition.vue'
    import RippleTransitionFlip from '@/util/components/transition/RippleTransitionFlip.vue'

    import { TaskEditorAPIHandler } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IDateRange, IDayData } from '@/models/tasks'
    import { Store } from 'vuex/types/index'
    import { UseReactiveSummary, UseDirectiveSummary } from './taskPeriodSimpleSummary'
    import { IDayCount } from "@/models/charts";
    

    export default defineComponent({
        name: 'TaskPeriodSimpleSummary',
        props:{
            daysData:{
                type: Array as () => Array<IDayData>,
            },
            isReactiveMode:{
                type: Boolean,
                default: true,
            },
            sourceSummary:{
                type: Array as () => Array<IDayCount>
            },
            selectedDates:{
                type: Object as () => IDateRange,
            },            
        },
        components:{
            FadeInOutTransition,
            RippleTransitionFlip,
        },
        setup( props, { emit, root } ){
            const { $store } = root
            const store = $store as Store<IStore>

            const daysData = computed( () => props?.daysData || [])
            const sourceSummary = computed( () => props?.sourceSummary || [] )
            const selectedDates = computed( () => props?.selectedDates || {} as IDateRange )
            if ( props.isReactiveMode ) {                
                return UseReactiveSummary(store, daysData, selectedDates)
            }else{
                
                return UseDirectiveSummary(store, sourceSummary, selectedDates)
            }
        }        
    })
</script>

<style lang="scss">
    .TaskPeriodSimpleSummary .summary .v-data-table__wrapper{
        max-height: calc( 100vh - 300px );

        &::-webkit-scrollbar
        {
            width: 10px;
            background-color: #eaeaea;
        }

        &::-webkit-scrollbar-thumb
        {
            border-radius: 10px;
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3);
            background-color: #d8d8d8;

            &:hover{
                background-color: #ecebeb;
            }
        }

        $gradient: radial-gradient(circle, #ffffff00 80%, #a09e9e);
        tbody{
            background-image: $gradient, $img-xv;
            background-repeat: repeat;
        }

        td{
            font-size: 16px;
        }

        th, tr.total{
            font-weight: 900 !important;
            font-size: 15px;
            background: $color-accent !important;
            color: #d8d8d8 !important;
            height: 40px;
        }

        th.date{
            width: 120px;
        }

        $table-border-radius: 15px;
        thead tr:first-child {
            th:first-child{
                border-top-left-radius: $table-border-radius;
            }
        }

        tbody tr:last-child{
            td:first-child{
                border-bottom-left-radius: $table-border-radius;
            }
        }
    }
</style>

<style lang="scss" scoped>
    .TaskPeriodSimpleSummary{
        width: 100%;
        width: 300px;
        border-top-left-radius: 15px;
        border-bottom-left-radius: 15px;
        border-top-right-radius: 15px;
        border-bottom-right-radius: 15px;
        overflow: hidden;
        position: relative;
        margin: 0 auto;
        margin-top: 40px;
        margin-bottom: 30px;
    }

    .TaskPeriodSimpleSummary tbody{
        position: relative;
    }   

    .TaskPeriodSimpleSummary tr{
        &.inactive{
            background: rgba(217, 217, 217, 0.7);
        }

        &.overTime{
            background: $color-warning;
        }

        &.leave{
            background: #4ea397;
        }

        &.total{
            td{
                background: $color-accent;
                position: sticky;
                bottom: 0px;
            }
        }
    }
</style>

