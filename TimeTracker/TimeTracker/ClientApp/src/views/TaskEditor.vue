<template>
    <div class="TaskEditor">
        <TargetModalSelector :isOpenModal.sync="isOpenTargetSelector" :selectedUser.sync="targetUser" />
        
        <div>
            <v-row>
                <v-btn class="btn-select-target"
                    color="primary"
                    tile outlined fab depressed
                    :width="width" 
                    @click="isOpenTargetSelector = true">
                    <v-icon left>mdi-account-edit</v-icon>
                    {{ targetUser.name && targetUser.name.length > 0 ? targetUser.name : targetUser.email }}
                </v-btn>
            </v-row>
            <v-row>
                <DateMenuSelector :selectedDates.sync="targetDates" >
                    <template v-slot:activator="{ activator }">
                        <v-btn class="btn-select-date mt-1"
                            color="primary"
                            tile outlined fab depressed
                            v-bind="activator.attrs"
                            v-on="activator.on"
                            :width="width" 
                            :height="isShowPeriodName ? 110 : undefined"
                            >
                            <v-container>
                                <v-row no-gutters justify="center" v-if="isShowPeriodName">
                                    <v-col class="mt-2">
                                        <v-icon class="mr-1">mdi-calendar</v-icon>{{targetDates.periodName}}
                                    </v-col>
                                </v-row>
                                <v-row no-gutters justify="center" v-if="isShowPeriodName">
                                    <v-divider class="mt-3 mb-2" />
                                </v-row>
                                <v-row no-gutters justify="center">
                                    <v-col class="text-button">
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


        <p>targetUser {{targetUser}}</p>
        <p>targetDates {{targetDates}}</p>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, reactive, ref } from '@vue/composition-api'

    import TargetModalSelector from '@/components/trackTask/TargetModalSelector.vue'
    import DateMenuSelector from '@/components/trackTask/DateMenuSelector.vue'

    import { IClaims } from '@/models/authentication.ts'
    import { IDateRange } from '@/models/tasks'

    export default defineComponent({
        name: 'TaskEditor',
        props:{
            width:{
                type: Number,
                default: 300
            }
        },
        components:{
            TargetModalSelector,
            DateMenuSelector,
        },
        setup(){
            const targetUser = ref({} as IClaims)
            const targetDates = ref({} as IDateRange)

            const isOpenTargetSelector = ref(false)
            const isShowPeriodName = computed( () => targetDates.value.isUsedPeriod && targetDates.value.periodName.length > 0 )

            // console.log( targetUser )

            return {
                isOpenTargetSelector,
                isShowPeriodName,
                targetUser,
                targetDates,
            }
        }        
    })
</script>

<style lang="scss">
    .TaskEditor .v-btn{
        text-transform: initial;
    }

    $border-radius: 15px;

    .TaskEditor .btn-select-target{
        border-top-left-radius: $border-radius;
        border-top-right-radius: $border-radius;
    }
    .TaskEditor .btn-select-date{
        border-bottom-left-radius: $border-radius;
        border-bottom-right-radius: $border-radius;
    }
</style>