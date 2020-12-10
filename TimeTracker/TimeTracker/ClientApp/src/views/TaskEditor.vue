<template>
    <div class="TaskEditor">
        <TargetSelector :isOpenModal.sync="isOpenTargetSelector" :selectedAccount.sync="targetAccount" />
        <v-btn @click="isOpenModal = true">Open Modal</v-btn>

        {{targetAccount}}
    </div>
</template>

<script lang="ts">
    import { defineComponent, reactive, ref } from '@vue/composition-api'

    import TargetSelector from '@/components/trackTask/TargetSelector.vue'

    import { IClaims } from '@/models/authentication.ts'

    export default defineComponent({
        name: 'TaskEditor',
        props:{
            
        },
        components:{
            TargetSelector
        },
        setup(){

            const isOpenTargetSelector = ref(false)
            const targetAccount = ref({} as IClaims)

            console.log( targetAccount )

            return {
                isOpenTargetSelector,
                targetAccount,
            }
        }        
    })
</script>

