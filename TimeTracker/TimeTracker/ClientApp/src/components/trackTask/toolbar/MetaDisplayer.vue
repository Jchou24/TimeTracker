<template>
    <div class="MetaDisplayer">
        <TargetModalSelector 
            :singleSelectTarget="singleSelectTarget" 
            :isOpenModal.sync="isOpenTargetSelector" 
            :selectedUsers.sync="targetUsers" />
        
        <div>
            <v-row>
                <v-btn class="btn-select btn-select-target font-weight-black"
                    color="accent"
                    tile outlined fab depressed
                    :width="width" 
                    @click="isOpenTargetSelector = true">
                    <v-icon left>mdi-account-edit</v-icon>
                    {{ selectedUserInfo }}
                </v-btn>
            </v-row>
            <v-row>
                <DateMenuSelector :selectedDates.sync="targetDates" >
                    <template v-slot:activator="{ activator }">
                        <v-btn class="btn-select btn-select-date mt-1"
                            color="accent"
                            tile outlined fab depressed
                            v-bind="activator.attrs"
                            v-on="activator.on"
                            :width="width" 
                            :height="isShowPeriodName ? 110 : undefined"
                            >
                            <v-container>
                                <v-row no-gutters justify="center" v-if="isShowPeriodName">
                                    <v-col class="mt-2 font-weight-black">
                                        <v-icon class="mr-1">mdi-calendar-multiple</v-icon>{{targetDates.periodName}}
                                    </v-col>
                                </v-row>
                                <v-row no-gutters justify="center" v-if="isShowPeriodName">
                                    <v-divider class="mt-3 mb-2" />
                                </v-row>
                                <v-row no-gutters justify="center">
                                    <v-col class="text-button font-weight-bold">
                                        <v-icon class="mr-1">mdi-calendar</v-icon> <span>{{targetDates.startDate}}</span>
                                        <v-icon class="mx-2" >mdi-arrow-left-right-bold</v-icon>
                                        <v-icon class="mr-1" >mdi-calendar</v-icon> <span>{{targetDates.endDate}}</span>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-btn>
                    </template>
                </DateMenuSelector>
            </v-row>
        </div>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref } from '@vue/composition-api'

    import TargetModalSelector from '@/components/trackTask/toolbar/TargetModalSelector.vue'
    import DateMenuSelector from '@/components/trackTask/toolbar/DateMenuSelector.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange } from '@/models/tasks'

    export default defineComponent({
        name: 'MetaDisplayer',
        props:{
            selectedUsers:{
                type: Array as () => Array<IClaims>,
                default: () => [],
            },
            selectedDates:{
                type: Object as () => IDateRange,
            },
            width:{
                type: Number,
                default: 300
            },
            singleSelectTarget:{
                type: Boolean,
                default: true
            },
        },
        components:{
            TargetModalSelector,
            DateMenuSelector,
        },
        setup( props, { emit } ){
            const targetUsers = computed({
                get: () => props.selectedUsers,
                set: (value) => emit("update:selectedUsers", value)
            })

            const targetDates = computed({
                get: () => props.selectedDates,
                set: (value) => emit("update:selectedDates", value)
            })

            const isOpenTargetSelector = ref(false)
            const isShowPeriodName = computed( () => targetDates?.value?.isUsedPeriod && targetDates?.value?.periodName?.length > 0 )
            const GetSingleSelectedUserInfo = () => {
                const singleTarget = targetUsers.value[0]
                if (!singleTarget) {
                    return `No user selected.`
                }
                return singleTarget.name && singleTarget.name.length > 0 ? singleTarget.name : singleTarget.email
            }
            const selectedUserInfo = computed( () => {
                if ( props.singleSelectTarget ) {
                    return GetSingleSelectedUserInfo()
                }

                if( targetUsers.value.length == 1 ){
                    return GetSingleSelectedUserInfo()
                }

                return `Select ${targetUsers.value.length} users.`
            })

            return {
                isOpenTargetSelector,
                isShowPeriodName,
                targetUsers,
                targetDates,
                selectedUserInfo,
            }
        }        
    })
</script>

<style lang="scss">
    .MetaDisplayer{
        min-width: 300px;
        margin-left: 20px;
    }
    .MetaDisplayer .v-btn{
        text-transform: initial;
    }
    
    .MetaDisplayer .btn-select{
        background: whitesmoke;        
    }

    $border-radius: 15px;
    .MetaDisplayer .btn-select-target{
        border-top-left-radius: $border-radius;
        border-top-right-radius: $border-radius;

        // background-image: radial-gradient(circle, #ffffff00 80%, #a09e9e), $img-vichy;
        background-image: radial-gradient(circle, #ffffff00 80%, #a09e9e), $img-xv;
        background-repeat: repeat;
    }
    .MetaDisplayer .btn-select-date{
        border-bottom-left-radius: $border-radius;
        border-bottom-right-radius: $border-radius;

        background-image: radial-gradient(circle, #ffffff00 80%, #a09e9e), $img-xv;
        background-repeat: repeat;
    }
</style>