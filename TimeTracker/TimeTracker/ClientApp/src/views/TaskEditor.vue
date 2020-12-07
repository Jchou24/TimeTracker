<template>
    <div class="TaskEditor">
        
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    export default defineComponent({
        name: 'TaskEditor',
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

