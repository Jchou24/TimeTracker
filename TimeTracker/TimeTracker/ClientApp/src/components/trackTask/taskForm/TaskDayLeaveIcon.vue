<template>
    <v-btn class="TaskDayLeaveIcon"        
        fab
        :color="color"
        :loading="false"
        @click="HandleClickIsLeave(dayData)"
        >
        <v-icon v-if="dayData.isLeave">mdi-marker-cancel</v-icon>
        <v-icon v-else>mdi-pencil</v-icon>
    </v-btn>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import { Store } from 'vuex/types/index'
    import { TaskEditorWSHandler } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IClaims } from '@/models/authentication'
    import { IDayData, IUpdateIsLeave } from '@/models/tasks'

    export default defineComponent({
        name: 'TaskDayLeaveIcon',
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
            function HandleClickIsLeave(dayData: IDayData){
                // console.log("Handle Click IsLeave")
                dayData.isLeave = !dayData.isLeave
                taskEditorWSHandler.UpdateIsLeave({
                    ownerGuid: props.user?.guid,
                    date: dayData.date,
                    isLeave: dayData.isLeave,
                } as IUpdateIsLeave)
            }

            const color = computed( () => props.dayData?.isLeave ? "accent" : "primary" )
            
            return {
                color,
                HandleClickIsLeave,
            }
        }        
    })
</script>

<style lang="scss">
</style>