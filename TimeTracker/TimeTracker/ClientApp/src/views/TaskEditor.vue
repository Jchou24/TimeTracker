<template>
    <div class="TaskEditor">
        <MetaDisplayer :selectedUser.sync="targetUser" :selectedDates.sync="targetDates" :width="widthMetaDisplayer" />

        <TaskDayTimeline :user="targetUser" :dateRange="targetDates" />

        <!-- <p>targetUser {{targetUser}}</p>
        <p>targetDates {{targetDates}}</p> -->
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref } from '@vue/composition-api'

    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'
    import TaskDayTimeline from '@/components/trackTask/taskForm/TaskDayTimeline.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange } from '@/models/tasks'

    export default defineComponent({
        name: 'TaskEditor',
        props:{
            widthMetaDisplayer:{
                type: Number,
                default: 300
            }
        },
        components:{
            MetaDisplayer,
            TaskDayTimeline
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
        // overflow-x: scroll;
    }
</style>