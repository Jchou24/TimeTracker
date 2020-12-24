<template>
    <Container :isFullHeight="false">
        <Center class="TrackSettings">
            <Tab :tabTitles="tabTitles">
                <v-tab-item>
                    <DatePeriodForm class="add-margin"
                        :width="width" :modalWidth="modalWidth" />
                </v-tab-item>
                <v-tab-item>
                    <OptionForm class="add-margin"
                        :width="width" :modalWidth="modalWidth"
                        title="Task Types"
                        titleIcon="mdi-form-select"
                        :OptionAPIHandler="taskTypesOptionAPIHandler"
                    />
                </v-tab-item>
                <v-tab-item>
                    <OptionForm class="add-margin"
                        :width="width" :modalWidth="modalWidth"
                        title="Task Sources"
                        titleIcon="mdi-form-select"
                        :OptionAPIHandler="taskSourcesOptionAPIHandler"
                    />
                </v-tab-item>
            </Tab>
        </Center>
    </Container>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Container from './layouts/Container.vue'
    import Center from './layouts/Center.vue'
    import Tab from './layouts/Tab.vue'
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
            Container,
            Center,
            Tab,
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

            const tabTitles = [{
                    text: "Periods",
                    icon: "mdi-calendar-multiple",
                },{
                    text: "Task Types",
                    icon: "mdi-form-select",
                },{
                    text: "Task Sources",
                    icon: "mdi-form-select",
                }]

            return {
                tabTitles,
                taskTypesOptionAPIHandler,
                taskSourcesOptionAPIHandler,
            }
        }        
    })
</script>

<style lang="scss" scoped>
    .add-margin{
        margin: 20px;
    }
</style>