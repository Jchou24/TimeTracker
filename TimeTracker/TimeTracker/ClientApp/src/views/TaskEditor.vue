<template>
    <div class="TaskEditor">
        <MetaDisplayer :selectedUser.sync="targetUser" :selectedDates.sync="targetDates" :width="widthMetaDisplayer" />

        <p>targetUser {{targetUser}}</p>
        <p>targetDates {{targetDates}}</p>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref } from '@vue/composition-api'

    import MetaDisplayer from '@/components/trackTask/toolbar/MetaDisplayer.vue'

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
    
</style>