<template>
    <div class="TaskPeriodEchartsLine">
        <v-card class="card mx-6 py-6 pt-0" elevation="10">
            <v-card-text class="title text-center text-h4 font-weight-black">
                {{title}}
            </v-card-text>

            <v-divider class="mx-6" />

            <div class="data-charts">
                <v-chart class="margin-center mt-n10" theme="ovilia-green" autoresize ref="chart" 
                    :options="options"    
                />

                
            </div>

        </v-card>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref, watch } from '@vue/composition-api'

    import ECharts from 'vue-echarts'
    import 'echarts/lib/chart/line'
    import 'echarts/lib/component/tooltip'
    import 'echarts/lib/component/legend'
    import 'echarts/lib/component/legendScroll'
    import 'echarts/lib/component/title'
    import { EChartOption } from 'echarts'

    import theme from './greenTheme.json'
    ECharts.registerTheme('ovilia-green', theme)

    import { IEchartsPieRow, ITaskTimeSummaryDetail } from '@/models/charts'
    import { FormatDate } from '@/util/taskDate'

    interface IChartMethod{
        showLoading: Function;
        hideLoading: Function;
    }

    export default defineComponent({
        name: 'TaskPeriodEchartsLine',
        props:{
            lineData:{
                type: Array as () => Array<ITaskTimeSummaryDetail>,
                default: () => [],
                required: true,
            },
            title:{
                type: String,
                default: "",
            },
            subTitle:{
                type: String,
                default: "",
            },
            isShowLoading:{
                type: Boolean,
                default: false
            }
        },
        components: {
            'v-chart': ECharts,
        },
        setup( props, { emit, refs } ){

            const GetOptions = () => {
                const dates = props.lineData.map( x => FormatDate(x.date, "MM-DD") )

                const minConsumeTime = props.lineData.map( x => x.minConsumeTime )
                const maxConsumeTime = props.lineData.map( x => x.maxConsumeTime )
                const avgConsumeTime = props.lineData.map( x => x.avgConsumeTime )
                const medianConsumeTime = props.lineData.map( x => x.medianConsumeTime )

                const minOvertime = props.lineData.map( x => x.minOvertime )
                const maxOvertime = props.lineData.map( x => x.maxOvertime )
                const avgOvertime = props.lineData.map( x => x.avgOvertime )
                const medianOvertime = props.lineData.map( x => x.medianOvertime )


                return {
                    xAxis: {
                        type: 'category',
                        data: dates
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [{
                        data: minConsumeTime,
                        type: 'line',
                        symbol: 'none',
                        // stack: 'x',
                        // smooth: false,
                        // areaStyle: {
                        //     color: '#ccc',
                        //     // origin: "end"
                        // }
                    },{
                        data: maxConsumeTime,
                        type: 'line',
                        symbol: 'none',
                        // stack: 'x',
                        smooth: false,
                        // areaStyle: {
                        //     color: '#ccc',
                        //     origin: "end"
                        // }
                    },{
                        data: avgConsumeTime,
                        type: 'line',
                        symbol: 'none',
                        smooth: false,
                    },{
                        data: medianConsumeTime,
                        type: 'line',
                        symbol: 'none',
                        smooth: false,
                    },

                    {
                        data: minOvertime,
                        type: 'line',
                        symbol: 'none',
                        smooth: false,
                    },{
                        data: maxOvertime,
                        type: 'line',
                        symbol: 'none',
                        smooth: false,
                    },{
                        data: avgOvertime,
                        type: 'line',
                        symbol: 'none',
                        smooth: false,
                    },{
                        data: medianOvertime,
                        type: 'line',
                        symbol: 'none',
                        smooth: false,
                    },

                    ],
                    
            } as EChartOption<EChartOption.SeriesLines> };

            const options = ref( GetOptions() )
            watch( () => props.lineData, () => {
                options.value = GetOptions()
            })

            // =================================================================
            const ShowLoading = () => (refs.chart as IChartMethod ).showLoading()
            const HideLoading = () => (refs.chart as IChartMethod ).hideLoading()

            watch( () => props.isShowLoading, () => {
                if( props.isShowLoading ){
                    ShowLoading()
                }else{
                    HideLoading()
                }
            })
            // =================================================================

            return {
                options,
                ShowLoading,
                HideLoading,
            }
        }        
    })
</script>

<style lang="scss">
    .EchartsPie .echarts{
        width: 350px;
        height: 370px;
        // margin-left: -100px;
        // margin-right: -100px;
    }
</style>

<style lang="scss" scoped>
    .EchartsPie{
        display: flex;
        justify-content: center;
    }

    .EchartsPie .card{
        border-radius: 25px;
        max-width: 460px;

        .title{
            position: sticky;
            top: 0;
            background: white;
            z-index: 1;
            color: #334b5c;
        }

        .data-charts{
            overflow-y: auto;
            min-width: 400px;
            height: calc( 100vh - 240px );

            &::-webkit-scrollbar
            {
                width: 8px;
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
        }
    }
</style>