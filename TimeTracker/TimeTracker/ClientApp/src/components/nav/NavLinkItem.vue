<template>
    <router-link class="NavLinkItem" :class="{ 'router-link-custom-active': isActiveLink }" :to="to">
        <NavItem :icon="icon" :mdiIcon="navIcon" :left="left" v-if="displayName">
            {{ navDisplayName }}
        </NavItem>
        <NavItem :icon="icon" :mdiIcon="navIcon" :left="left" v-else>
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
            },
            icon:{
                type: Boolean,
                default: false,
            }
        },
        components:{
            NavItem
        },
        setup( props, { root } ){
            const { $router, $route } = root
            const to = computed( () => routeConfigs[props.routeConfigsKey].path )
            const navIcon = computed( () => props.mdiIcon ? routeConfigs[props.routeConfigsKey].nav.mdiIcon : "" )
            const navDisplayName = computed( () => props.displayName ? routeConfigs[props.routeConfigsKey].nav.displayName : "" )
            const isActiveLink = computed( () => routeConfigs[props.routeConfigsKey].path == $router.currentRoute.path )

            return {
                to,
                navIcon,
                navDisplayName,
                isActiveLink,
            }
        }
    })
</script>

<style lang="scss">
    
</style>