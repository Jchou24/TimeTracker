<template>
    <Center class="Registration">
        <Register />
    </Center>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Center from './layouts/Center.vue'
    import Register from '@/components/auth/Register.vue'

    export default defineComponent({
        name: 'Registration',
        props:{

        },
        components:{
            Center,
            Register
        },      
    })
</script>

<style lang="scss" scoped>
</style>