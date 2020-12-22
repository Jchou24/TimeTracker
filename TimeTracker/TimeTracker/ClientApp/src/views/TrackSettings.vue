<template>
    <Center class="TrackSettings">
        <DatePeriodForm :width="800" />

    </Center>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Center from './layouts/Center.vue'
    import DatePeriodForm from '@/components/trackTask/trackSettings/DatePeriodForm.vue'

    export default defineComponent({
        name: 'TrackSettings',
        props:{

        },
        components:{
            Center,
            DatePeriodForm,
        },  
        setup(){
            const data = ref("AAA")
            return {
                data
            }
        }        
    })
</script>

