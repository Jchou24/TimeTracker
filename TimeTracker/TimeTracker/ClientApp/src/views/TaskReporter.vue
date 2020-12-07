<template>
    <div class="TaskReporter">
        
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    export default defineComponent({
        name: 'TaskReporter',
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

