<template>
    <router-link class="NavLinkItem" :class="{ 'router-link-custom-active': isActiveLink }" :to="to">
        <NavItem :mdiIcon="navIcon" :left="left" v-if="displayName">
            {{ navDisplayName }}
        </NavItem>
        <NavItem :mdiIcon="navIcon" :left="left" v-else>
            <slot></slot>
        </NavItem>
    </router-link>
</template>

<script lang="ts">
    import { computed, defineComponent, watch } from '@vue/composition-api'
    import NavItem from './NavItem.vue'
    import { routeConfigs } from '@/router/routeConfigs'

    export default defineComponent({
        name: 'NavLinkItem',
        props:{
            routeConfigsKey:{
                type: String,
                required: true,
            },
            mdiIcon:{
                type: Boolean,
                default: false,
            },
            displayName:{
                type: Boolean,
                default: false,
            },
            left:{
                type: Boolean,
                default: false,
            }
        },
        components:{
            NavItem
        },
        computed:{
            isActiveLink(){
                return routeConfigs[this.routeConfigsKey].path == this.$route.path
            }
        },
        setup( props, { root } ){
            const { $router, $route } = root
            const to = computed( () => routeConfigs[props.routeConfigsKey].path )
            const navIcon = computed( () => props.mdiIcon ? routeConfigs[props.routeConfigsKey].nav.mdiIcon : "" )
            const navDisplayName = computed( () => props.displayName ? routeConfigs[props.routeConfigsKey].nav.displayName : "" )

            return {
                to,
                navIcon,
                navDisplayName,
            }
        }
    })
</script>

<style lang="scss">
    
</style>