<template>
    <div class="TaskDayLeaveCheckbox">
        <v-checkbox
            v-model="dayData.isLeave"
            label="Leave"
            color="primary"
            @click="HandleClickIsLeave(dayData, dayData.isLeave)"
        />
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import { Store } from 'vuex/types/index'
    import { TaskEditorWSHandler } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IClaims } from '@/models/authentication'
    import { IDayData, IUpdateIsLeave } from '@/models/tasks'

    import { IAddRow, IMoveRow, IRemoveRow, IUpdateCell } from './taskform'

    export default defineComponent({
        name: 'TaskDayLeaveCheckbox',
        props:{
            user:{
                type: Object as () => IClaims,
            },
            dayData:{
                type: Object as () => IDayData,
            }
        },
        setup( props, { emit, root } ){
            const { $store, $router, $route } = root
            const store = $store as Store<IStore>
            const router = $router
            const taskEditorWSHandler = new TaskEditorWSHandler( store )
            // ======================================================================
            function HandleClickIsLeave(dayData: IDayData, isLeave: boolean){
                // console.log("Handle Click IsLeave")
                taskEditorWSHandler.UpdateIsLeave({
                    ownerGuid: props.user?.guid,
                    date: dayData.date,
                    isLeave: isLeave,
                } as IUpdateIsLeave)
            }

            return {
                HandleClickIsLeave,
            }
        }        
    })
</script>

<style lang="scss">
</style>