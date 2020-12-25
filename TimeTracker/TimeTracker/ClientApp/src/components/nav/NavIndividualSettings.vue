<template>
    <NavLinkItem class="individual-settings" routeConfigsKey="IndividualSettings" v-if="isUseUserImage">
        <v-img class="user-image-nav" :src="userImage" />
    </NavLinkItem>
    <NavLinkItem class="individual-settings" routeConfigsKey="IndividualSettings" mdiIcon v-else />
</template>

<script lang="ts">
    import { computed, defineComponent, watch } from '@vue/composition-api'

    import { IStore } from '@/models/store'
    import { Store } from 'vuex/types/index'

    import NavLinkItem from './NavLinkItem.vue'

    export default defineComponent({
        name: 'NavIndividualSettings',
        props:{
        },
        components:{
            NavLinkItem
        },
        setup( props, { root } ){
            const { $store } = root
            const store = $store as Store<IStore>
            const userImage = computed( () => store.state.userImage )
            const isUseUserImage = computed( () => userImage.value.length > 0 )

            return {
                userImage,
                isUseUserImage,
            }
        }
    })
</script>

<style lang="scss">
    
</style>