<template>
    <a class="NavLinkItem router-link-custom-active" href="https://github.com/Jchou24/TimeTracker" target="_blank" rel="noopener noreferrer" >
        <NavItem icon mdiIcon="mdi-github" />
    </a>
</template>

<script lang="ts">
    import { computed, defineComponent, watch } from '@vue/composition-api'
    import NavItem from './NavItem.vue'
    import { routeConfigs } from '@/router/routeConfigs'

    export default defineComponent({
        name: 'NavGithubLink',
        props:{
        },
        components:{
            NavItem
        },
    })
</script>

<style lang="scss">
    
</style>