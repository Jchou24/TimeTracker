<template>
    <div class="about">
        <h1>This is an about page</h1>
        {{data}}
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    export default defineComponent({
        name: 'About',
        props:{

        },
        setup(){
            const data = ref("AAA")
            return {
                data
            }
        }        
    })
</script>

