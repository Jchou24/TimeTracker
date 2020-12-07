<template>
    <div class="about">
        
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

