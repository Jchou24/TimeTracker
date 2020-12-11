<template>
    <div class="DatePicker">
        <v-menu
            v-model="menu"
            :close-on-content-click="false"
            :nudge-right="40"
            transition="scale-transition"
            offset-x
            min-width="290px"
        >
            <template v-slot:activator="{ on, attrs }">
                <v-text-field dense outlined
                    v-model="selectedDate"
                    :rules="[ ValidateDate ]"
                    :label="props.label"
                    v-bind="attrs"
                    v-on="on"
                    :append-icon="isFocus ? 'mdi-target' : 'mdi-calendar'"
                    @click:append="SetToday"
                    @focus="HandleFocus"
                    @blur="HandleBlur"
                >
                    <template v-slot:prepend>
                        <v-icon left>{{prependIcon}}</v-icon>
                    </template>
                </v-text-field>
            </template>
            <v-date-picker
                v-model="selectedDate"
                no-title
                locale="zh-cn"
                :day-format="FormatDay"
                :show-week="true"
                :scrollable="true"  
            />
        </v-menu>
    </div>
</template>

<script lang="ts">
    import { computed, defineComponent, Ref, ref, watch } from '@vue/composition-api'

    import { GetToday, GetYesterday, ValidateDate } from '@/util/taskDate'

    export default defineComponent({
        name: 'DatePicker',
        props:{
            date:{
                type: String,
            },
            label:{
                type: String,
            },
            prependIcon:{
                type: String,
            }
        },
        setup( props, { emit, root } ){
            const selectedDate = computed({
                get: () => props.date,
                set: (value) => emit("update:date", value)
            })

            const menu = ref(false)

            function FormatDay(date: string){
                return parseInt(date.split("-").slice(-1)[0])
            }

            function SetToday(){
                selectedDate.value = GetYesterday(); // in order to force emit
                selectedDate.value = GetToday();
            }

            const isFocus = ref(false)
            function HandleFocus(){
                isFocus.value = true
            }

            function HandleBlur(){
                isFocus.value = false
            }

            return {
                props,
                selectedDate,
                menu,
                isFocus,
                FormatDay,
                ValidateDate,
                SetToday,
                HandleFocus,
                HandleBlur,
            }
        }        
    })
</script>

<style lang="scss">

</style>