<template>
    <Container>
        <Center class="Registration">
            <Register />
        </Center>
    </Container>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Container from './layouts/Container.vue'
    import Center from './layouts/Center.vue'
    import Register from '@/components/auth/Register.vue'

    export default defineComponent({
        name: 'Registration',
        props:{

        },
        components:{
            Container,
            Center,
            Register
        },      
    })
</script>

<style lang="scss" scoped>
</style>