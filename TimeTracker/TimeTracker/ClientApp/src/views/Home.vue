<template>
    <Center>
        <div class="home">
            <img alt="Vue logo" src="../assets/logo.png">
            <HelloWorld msg="Welcome to Your Vue.js + TypeScript App"/>
            <PageFlipper />
        </div>
    </Center>
</template>

<script lang="ts">
    import { defineComponent } from '@vue/composition-api';
    import Center from './layouts/Center.vue'
    import HelloWorld from '@/components/HelloWorld.vue'; // @ is an alias to /src
    import PageFlipper from '@/components/PageFlipper.vue'

    export default defineComponent({
        name: 'Home',
        components: {
            Center,
            HelloWorld,
            PageFlipper,
        },
    });
</script>
