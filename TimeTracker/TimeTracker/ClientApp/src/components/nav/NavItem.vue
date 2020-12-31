<template>
    <v-btn class="NavItem" :icon="icon" depressed x-large>
        <v-icon :left="left" v-if="mdiIcon">{{mdiIcon}}</v-icon>
        <slot></slot>
    </v-btn>
</template>


<script lang="ts">
    import { defineComponent } from '@vue/composition-api'    

    export default defineComponent({
        name: 'NavItem',
        props:{
            mdiIcon:{
                type: String,
                default: "",
            },
            left:{
                type: Boolean,
                default: false,
            },
            icon:{
                type: Boolean,
                default: false,
            }
        }
    })
</script>

<style lang="scss" scoped>
    .v-btn:not(.v-btn--round).v-size--x-large{
        min-width: 50px;
        padding: 0 10px;
    }
</style>