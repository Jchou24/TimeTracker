<template>
    <Center class="TrackSettings">
        <DatePeriodForm :width="width" :modalWidth="modalWidth" />

        <OptionForm :width="width" :modalWidth="modalWidth"
            title="Task Types"
            titleIcon="mdi-form-select"
            :OptionAPIHandler="taskTypesOptionAPIHandler"
        />

        <OptionForm :width="width" :modalWidth="modalWidth"
            title="Task Sources"
            titleIcon="mdi-form-select"
            :OptionAPIHandler="taskSourcesOptionAPIHandler"
        />
    </Center>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Center from './layouts/Center.vue'
    import DatePeriodForm from '@/components/trackTask/trackSettings/DatePeriodForm.vue'
    import OptionForm from '@/components/trackTask/trackSettings/OptionForm.vue'

    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { ParameterAPIHandler, TaskSourcesOptionAPIHandler, TaskTypesOptionAPIHandler } from '@/api/parameter'
    import { IOptionMethods } from '@/models/parameter'

    export default defineComponent({
        name: 'TrackSettings',
        props:{
            width:{
                type: Number,
                default: 800
            },
            modalWidth:{
                type: Number,
                default: 500
            }
        },
        components:{
            Center,
            DatePeriodForm,
            OptionForm,
        },  
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const parameterAPIHandler = new ParameterAPIHandler( store, router )
            const taskTypesOptionAPIHandler = new TaskTypesOptionAPIHandler( store, router )
            const taskSourcesOptionAPIHandler = new TaskSourcesOptionAPIHandler( store, router )

            return {
                taskTypesOptionAPIHandler,
                taskSourcesOptionAPIHandler,
            }
        }        
    })
</script>

<style lang="scss" scoped>

</style>