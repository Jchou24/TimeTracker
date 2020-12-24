<template>
    <v-container class="Container" fluid :fill-height="isFullHeight" >
        <slot></slot>
    </v-container>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    export default defineComponent({
        name: 'Container',
        props:{
            isFullHeight:{
                type: Boolean,
                default: true,
            }
        },
    })
</script>

<style lang="scss" scoped>
    .Container{
        display: flex;
    }
</style>