<template>
    <div class="PieRowData">
        <div class="mt-6 mb-n4 text-center text-h5">
            #Total: {{total}} (hr)
        </div>

        <v-card-title class="px-10 pb-2">
            <v-text-field 
                v-model="search"
                append-icon="mdi-magnify"
                label="Search"
                single-line
                hide-details
                />
        </v-card-title>

        <v-data-table class="theme-table px-6" color="primary"
            :headers="headers"
            :items="items"
            :search="search"
            :items-per-page="10"
            :loading="isLoading"
            fixed-header
            filterable
            multi-sort            
        />
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref, watch } from '@vue/composition-api'
    import { IEchartsPieRow } from '@/models/charts'
    import { DataTableHeader } from 'vuetify'

    interface IEchartsPieRowDetail extends IEchartsPieRow{
        id: number;
        percent: string;
    }

    export default defineComponent({
        name: 'PieRowData',
        props:{
            pieData:{
                type: Array as () => Array<IEchartsPieRow>,
                default: () => [],
                required: true,
            },
            isLoading:{
                type: Boolean,
                default: false
            }
        },
        setup( props ){
            
            const total = ref(0.0 as number)
            const items = ref([] as Array<IEchartsPieRowDetail>)
            const search = ref("")
            const headers = [
                { text: "Name", value: "name", align:"center" },
                { text: "Time", value: "value", align:"center" },
                { text: "Time(%)", value: "percent", align:"center" },
            ] as Array<DataTableHeader>

            function InitItems() {
                total.value = props.pieData.reduce( ( accumulator, row ) => accumulator += row.value, 0 )
                items.value = props.pieData.map( (row, idx) => ({
                    ...row,
                    id: idx,
                    percent: `${(row.value * 100 / total.value).toFixed(2)}%`,
                } as IEchartsPieRowDetail))
            }
            InitItems()

            watch( () => props.pieData, () => InitItems() )
            

            return {
                items,
                total,
                headers,
                search,
            }
        }        
    })
</script>

<style lang="scss">
    .PieRowData .v-data-footer__select{
        margin-right: 0px;

        .v-select{
            margin-left: 15px;
        }
    }

    .PieRowData .v-data-table--fixed-header .v-data-footer{
        margin-right: 0px;
        justify-content: center;
    }

    .PieRowData .v-data-footer__pagination{
        margin-right: 0px;
    }

    .PieRowData .v-data-footer__icons-before{
        margin-right: -10px;
    }
</style>