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

