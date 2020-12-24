<template>
    <Container>
        <Center>
            <div class="home">
                <img alt="Vue logo" src="../assets/logo.png">
                <HelloWorld msg="Welcome to Your Vue.js + TypeScript App"/>
                <PageFlipper />
            </div>
        </Center>
    </Container>
</template>

<script lang="ts">
    import { defineComponent } from '@vue/composition-api';
    import Container from './layouts/Container.vue'
    import Center from './layouts/Center.vue'
    import HelloWorld from '@/components/HelloWorld.vue'; // @ is an alias to /src
    import PageFlipper from '@/components/PageFlipper.vue'

    export default defineComponent({
        name: 'Home',
        components: {
            Container,
            Center,
            HelloWorld,
            PageFlipper,
        },
    });
</script>

<style lang="scss" scoped>
    .home{
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }
</style>