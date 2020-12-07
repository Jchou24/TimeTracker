<template>
    <Center class="IndividualSettings">
        <Settings />
    </Center>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Center from './layouts/Center.vue'
    import Settings from '@/components/auth/Settings.vue'

    export default defineComponent({
        name: 'IndividualSettings',
        props:{

        },
        components:{
            Center,
            Settings
        },   
    })
</script>

<style lang="scss" scoped>
</style>