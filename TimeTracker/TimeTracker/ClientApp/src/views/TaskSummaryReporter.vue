<template>
    <div class="TaskSummaryReporter">
        <TaskReporter multipleMode/>
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import TaskReporter from './TaskReporter.vue'

    export default defineComponent({
        name: 'TaskSummaryReporter',
        props:{

        },
        components:{
            TaskReporter,
        },      
    })
</script>


<style lang="scss" scoped>
    .TaskSummaryReporter{
        width: 100%;
    }
</style>