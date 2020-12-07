<template>
    <Center class="TrackSettings">
        
    </Center>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Center from './layouts/Center.vue'

    export default defineComponent({
        name: 'TrackSettings',
        props:{

        },
        components:{
            Center,
        },  
        setup(){
            const data = ref("AAA")
            return {
                data
            }
        }        
    })
</script>

