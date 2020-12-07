<template>
    <v-progress-linear class="NavProgress" indeterminate stream  v-if="isNavLoading"/>
</template>

<script lang="ts">
    import { defineComponent, computed } from '@vue/composition-api'

    export default defineComponent({
        name: 'NavProgress',
        setup(props, { root }){
            const { $store } = root
            // const store = useStore()
            const store = $store            
            const isNavLoading = computed( () => store.state.isLoading )       

            return {
                isNavLoading
            }
        }      
    })
</script>

<style lang="scss" scoped>
    .NavProgress{
        position: fixed;
        left: 0;
        top: 0;
        width: 100vw;
        height: 5px;
        z-index: 99999;
    }
</style>