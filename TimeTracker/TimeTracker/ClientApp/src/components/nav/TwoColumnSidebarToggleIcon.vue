<template>
    <v-app-bar-nav-icon class="TwoColumnSidebarToggleIcon"
        @click="ToggleSidebar"
        :disabled="!isActiveTwoColumn"
        />
</template>


<script lang="ts">
    import { IStore } from '@/models/store'
    import { computed, defineComponent } from '@vue/composition-api'    
    import { Store } from 'vuex/types/index'

    export default defineComponent({
        name: 'TwoColumnSidebarToggleIcon',
        props:{
            mdiIcon:{
                type: String,
                default: "",
            },
            left:{
                type: Boolean,
                default: false,
            }
        },
        setup(props, { root }){
            const { $store, $router } = root
            const store = $store as Store<IStore>

            const isActiveTwoColumn = computed( () => store.state.twoColumn.isActiveTwoColumn )
            const ToggleSidebar = () => store.commit("twoColumn/ToggleSidebar")

            return {
                isActiveTwoColumn,
                ToggleSidebar,
            }
        }
    })
</script>

<style lang="scss">
    .TwoColumnSidebarToggleIcon.theme--light.v-btn.v-btn--icon{
        color: #d3d3d3;
    }

</style>