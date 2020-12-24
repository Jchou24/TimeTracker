<template>
    <Container>
        <Center class="IndividualSettings">
            <Settings />
        </Center>
    </Container>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Container from './layouts/Container.vue'
    import Center from './layouts/Center.vue'
    import Settings from '@/components/auth/Settings.vue'

    export default defineComponent({
        name: 'IndividualSettings',
        props:{

        },
        components:{
            Container,
            Center,
            Settings
        },   
    })
</script>

<style lang="scss" scoped>
</style>