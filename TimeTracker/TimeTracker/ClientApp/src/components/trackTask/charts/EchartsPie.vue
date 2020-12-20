<template>
    <div class="EchartsPie">
        <v-card class="card mx-6 py-6 pt-0" elevation="10">
            <v-card-text class="title text-center text-h3">
                {{title}}
            </v-card-text>

            <v-divider class="mx-6" />

            <div class="data-charts">
                <v-chart class="margin-center mt-n10" theme="ovilia-green" autoresize ref="chart" 
                    :options="options"    
                />

                <PieRowData :pieData="pieData" :isLoading="isShowLoading" />
            </div>

        </v-card>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref, watch } from '@vue/composition-api'

    import PieRowData from './PieRowData.vue'

    import ECharts from 'vue-echarts'
    import 'echarts/lib/chart/pie'
    import 'echarts/lib/component/tooltip'
    import 'echarts/lib/component/legend'
    import 'echarts/lib/component/legendScroll'
    import 'echarts/lib/component/title'
    // import 'echarts/theme'

    import theme from './greenTheme.json'
    ECharts.registerTheme('ovilia-green', theme)

    import { IEchartsPieRow } from '@/models/charts'
    import { EChartsFullOption } from 'echarts/lib/option'

    interface IChartMethod{
        showLoading: Function;
        hideLoading: Function;
    }

    export default defineComponent({
        name: 'EchartsPie',
        props:{
            pieData:{
                type: Array as () => Array<IEchartsPieRow>,
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
            PieRowData
        },
        setup( props, { emit, refs } ){

            const GetOptions = () => {
                const data = props.pieData
                const legendData = props.pieData.map( data => data.name )
                const legendType = legendData.length > 5 ? 'scroll' : 'plain'

                return {
                    // title: {
                    //     text: props.title,
                    //     subtext: props.subTitle,
                    //     textStyle:{
                    //         fontWeight: "bolder",
                    //         fontSize: 24,
                    //     },
                    //     left: 'center',
                    // },
                    // tooltip: {
                    //     trigger: 'item',
                    //     formatter: '{b}: {c} ({d}%)',
                    //     backgroundColor: 'rgba(50,50,50,0.7)',
                    //     borderWidth: 0,
                    //     textStyle:{
                    //         color: '#fff',
                    //         fontWeight: "normal"
                    //     },
                    // },
                    legend: {
                        type: legendType,
                        // orient: 'vertical',
                        // left: 10,
                        bottom: 0,
                        data: legendData,
                    },
                    series: [
                        {
                            name: props.title,
                            type: 'pie',
                            radius: ['55%', '70%'],
                            selectedMode: 'multiple',
                            labelLine:{
                                show: false,
                            },
                            label: {
                                // show: true,
                                show: false,
                                position: 'center',
                                // position: 'inner',
                                // formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                                formatter: '{b|{b}}\n{c|{c}}  {per|{d}%}',
                                // formatter: '',
                                
                                // backgroundColor: '#eee',
                                // borderColor: '#aaa',
                                // borderWidth: 1,
                                // borderRadius: 4,
                                
                                // shadowBlur:3,
                                // shadowOffsetX: 2,
                                // shadowOffsetY: 2,
                                // shadowColor: '#999',
                                // padding: [0, 7],
                                
                                rich: {
                                    a: {
                                        color: '#999',
                                        lineHeight: 22,
                                        align: 'center'
                                    },
                                    abg: {
                                        backgroundColor: '#333',
                                        width: '100%',
                                        align: 'right',
                                        height: 22,
                                        borderRadius: [4, 4, 0, 0]
                                    },
                                    // hr: {
                                    //     borderColor: '#aaa',
                                    //     width: '100%',
                                    //     borderWidth: 0.5,
                                    //     height: 0
                                    // },
                                    b: {
                                        fontSize: 18,
                                        lineHeight: 33,
                                        fontWeight: 'bold',
                                        color: '#4ea397',
                                    },
                                    c :{
                                        fontSize: 16,
                                        color: '#4ea397',
                                        fontWeight: 'bold',
                                    },
                                    per: {
                                        fontSize: 16,
                                        color: '#eee',
                                        backgroundColor: '#334455',
                                        padding: [6, 8],
                                        borderRadius: 10
                                    }
                                }
                            },
                            itemStyle: {
                                shadowBlur: 10,
                                shadowOffsetX: 0,
                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                            },
                            emphasis: {
                                label: {
                                    show: true,
                                    // fontSize: '30',
                                    // fontWeight: 'bold'
                                },
                                itemStyle: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                },
                            },
                            data: data,
                        }
                    ]
            } as EChartsFullOption };

            const options = reactive( GetOptions() )
            watch( () => props.pieData, () => {
                const tmpOptons = GetOptions()
                Object.keys(tmpOptons).forEach( key => {
                    Reflect.set( options, key, tmpOptons[key] )
                })
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
        max-width: 400px;

        .title{
            position: sticky;
            top: 0;
            background: white;
            z-index: 1;
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