<template>
    <v-card class="GeneralCard"
        elevation="10"
        :width="width"
        :loading="loading"
        outlined
        shaped
        >
        <v-card-title class="title flex-center text-h4 my-4 font-weight-bold">
            <v-icon class="title text-h4" left v-if="titleIcon">{{titleIcon}}</v-icon>
            {{title}}
        </v-card-title>
        <v-divider class="mb-4" />


        <slot :class="{'mb-4': !isCardActionNotEmpty }" name="body"></slot>

        <div class="mb-8" v-if="isCardActionEmpty" />
        <v-divider class="mt-4" v-else />

        <v-card-actions class="py-6 flex-center" v-if="!isCardActionEmpty">
            <slot name="action"></slot>
        </v-card-actions>
    </v-card>
</template>

<script lang="ts">
    import { computed, defineComponent, onMounted, ref } from '@vue/composition-api'
    // import { useStore } from 'vuex'

    export default defineComponent({
        name: 'GeneralCard',
        props:{
            width:{
                type: Number,
                default: 500,
            },
            title:{
                type: String,
                default: ""
            },
            titleIcon:{
                type: String,
                default: ""
            },
            loading:{
                type: Boolean,
                default: false,
            }
            
        },
        setup(props, { emit, root, slots }){
            const isCardActionEmpty = computed( () => !slots["action"] )

            return {
                isCardActionEmpty,
            }
        }
    });
</script>

<style lang="scss">
    
</style>

<style lang="scss" scoped>
    .title{
        color: $color-title;
    }
</style>