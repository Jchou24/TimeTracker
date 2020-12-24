<template>
    <Container>
        <div class="TaskSummaryReporter">
            <TaskReporter multipleMode/>
        </div>
    </Container>
</template>

<script lang="ts">
    import { defineComponent, ref } from '@vue/composition-api'

    import Container from './layouts/Container.vue'
    import TaskReporter from './TaskReporter.vue'

    export default defineComponent({
        name: 'TaskSummaryReporter',
        props:{

        },
        components:{
            Container,
            TaskReporter,
        },      
    })
</script>


<style lang="scss" scoped>
    .TaskSummaryReporter{
        width: 100%;
    }
</style>