<template>
    <div class="TaskSummaryReporter">
        <TaskReporter :singleSelectTarget="false" />
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

