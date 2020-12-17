<template>
    <div class="TaskEditor">
        <TwoColumn>
            <template v-slot:left>
                <MetaDisplayer :selectedUser.sync="targetUser" :selectedDates.sync="targetDates" :width="widthMetaDisplayer" />
            </template>
            <template v-slot:right>
                <TaskDayTimeline :user="targetUser" :dateRange="targetDates" />
            </template>
        </TwoColumn>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref } from '@vue/composition-api'

    import TwoColumn from '@/views/layouts/TwoColumn.vue'
    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'
    import TaskDayTimeline from '@/components/trackTask/taskForm/TaskDayTimeline.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange } from '@/models/tasks'
    import { TaskEditorAPIHandler, TaskEditorWSHandler } from '@/api/taskEditor'

    export default defineComponent({
        name: 'TaskEditor',
        props:{
            widthMetaDisplayer:{
                type: Number,
                default: 300
            }
        },
        components:{
            TwoColumn,
            MetaDisplayer,
            TaskDayTimeline,
        },
        beforeRouteLeave(to, from, next) {
            // const taskEditorAPIHandler = new TaskEditorAPIHandler( this.$store, this.$router )
            // taskEditorAPIHandler.UnsubscribeWSGroup()
            const taskEditorWSHandler = new TaskEditorWSHandler( this.$store )
            taskEditorWSHandler.Unsubscribe()
            next();
        },
        setup(){
            const targetUser = ref({} as IClaims)
            const targetDates = ref({} as IDateRange)

            // console.log( targetUser )

            return {
                targetUser,
                targetDates,
            }
        }        
    })
</script>

<style lang="scss">
    .TaskEditor{
        width: 100%;
        min-width: 1400px;
        padding-top: 30px;
    }
</style>