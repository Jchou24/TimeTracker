<template>
    <div class="TaskDayTimelineTitle text-h5 font-weight-bold py-3 px-4" :class="{ active: !dayData.isLeave}" v-ripple
        @click="HandleClickIsLeave(dayData)">
        <v-icon class="icon" v-if="dayData.isLeave">mdi-marker-cancel</v-icon>
        <v-icon class="icon" v-else>mdi-pencil</v-icon>
        {{FormatDate(dayData.date, "MM/DD")}}

        <SimpleTransition mode="out-in">
            <div class="is-leave" v-if="dayData.isLeave">
                <v-checkbox
                    input-value="false"
                    label="Leave"
                    :disabled="true"
                    color="blue lighten-5"
                    v-ripple
                />
            </div>
        </SimpleTransition>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import SimpleTransition from '@/util/components/transition/SimpleTransition.vue'

    import { Store } from 'vuex/types/index'
    import { TaskEditorWSHandler } from '@/api/taskEditor'
    import { IStore } from '@/models/store'
    import { IClaims } from '@/models/authentication'
    import { IDayData, IUpdateIsLeave } from '@/models/tasks'

    import { FormatDate } from '@/util/taskDate'

    export default defineComponent({
        name: 'TaskDayTimelineTitle',
        props:{
            user:{
                type: Object as () => IClaims,
            },
            dayData:{
                type: Object as () => IDayData,
            }
        },
        components:{
            SimpleTransition,
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

                emit("input")
            }

            const color = computed( () => props.dayData?.isLeave ? "accent" : "primary" )
            

            return {
                color,
                HandleClickIsLeave,
                FormatDate,
            }
        }        
    })
</script>

<style lang="scss">
    $color: whitesmoke;
    .TaskDayTimelineTitle{
        cursor: pointer;
        background: $color-accent;
        color: $color;

        @include vm-drop-shadow-1(5px, 5px, 5px);

        &.active{
            background: $color-primary;
        }

        .icon{
            color: $color;
            top: -2px
        }

        .is-leave{
            position: absolute;
            
            $color: whitesmoke;
            .v-input__slot{
                height: 30px;
                margin-left: -3px;
                padding-left: 10px;
                padding-right: 14px;
                border-radius: 5px;
                background: $color-warning;

                label{
                    color: $color;
                }
            }

            .theme--light.v-input--selection-controls.v-input--is-disabled:not(.v-input--indeterminate) .v-icon{
                color: $color !important;
            }
        }
    }
</style>