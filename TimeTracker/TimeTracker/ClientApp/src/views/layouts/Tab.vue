<template>
    <v-tabs class="Tab" vertical>
        
        <v-tab class="font-weight-bold text-subtitle-1"
            v-for="(title, idx) in tabTitles"
            :key="idx"
            >
            <v-icon left>{{title.icon}}</v-icon>{{title.text}}
        </v-tab>

        <slot></slot>
    </v-tabs>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    interface ITabTitle{
        text: string;
        icon: string;
    }
    export default defineComponent({
        name: 'Tab',
        props:{
            tabTitles:{
                type: Array as () => Array<ITabTitle>,
            },
        },     
    })
</script>

<style lang="scss">
    .Tab{

        $padding: 20px;
        
        .theme--light.v-tabs-items{
            background: unset;
        }

        .v-item-group.theme--light.v-slide-group.v-tabs-bar.primary--text{
            position: sticky;
            top: calc( 64px + 36px + #{$padding} );
            align-self: flex-start;
            height: auto;

            margin-right: 10px;
            border-radius: 15px;
            padding: 10px 0px 10px 10px;
            // @include vm-box-shadow-1();
            @include vm-drop-shadow-1(5px, 5px, 10px, #c5c3c3);

            .v-tab{
                justify-content: start;
                text-transform: unset;
            }   
        }
    }

    .v-tabs-slider-wrapper{
        display: flex;
    }
    .v-tabs--vertical>.v-tabs-bar .v-tabs-slider{
        margin: auto;
        height: 60%;        
    }
</style>