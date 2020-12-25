<template>
    <div class="SimpleImageEditorModal">

        <v-dialog :persistent="imageBase64.length > 0"
            v-model="isOpenModalRef"
            :width="width"
            >
            <v-card class="simple-imagee-ditor-content pa-8 pt-0"
                :loading="isLoading"
                >
                <v-container class="pt-14">
                    <v-row class="justify-center">
                        <SimpleImageEditor  :width="width" :isLoading="isLoading" :image.sync="imageBase64" refs="imageEditor"
                            @deleteImage="HandleClickDelete" />
                    </v-row>
                </v-container>

                <v-card-actions class="justify-center">
                    <v-btn class="action-btn pl-5 pr-6" rounded elevation="4" large
                        :loading="isLoading"
                        :disabled="isDisabledOK"
                        color="primary"
                        @click="HandleClickOK"
                        >
                        <v-icon left>mdi-check</v-icon>
                        OK
                    </v-btn>
                    <v-btn class="action-btn pl-5 pr-6" rounded elevation="4" large
                        color="accent"
                        @click="HandleClickCancel"
                        >
                        <v-icon left>mdi-close</v-icon>
                        Cancel
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>


<script lang="ts">
    import { computed, defineComponent, nextTick, onUpdated, ref, watch } from '@vue/composition-api'

    import SimpleImageEditor from './SimpleImageEditor.vue';

    export default defineComponent({
        name: 'SimpleImageEditorModal',
        props:{
            image:{
                type: String,
                default: ""
            },
            isOpenModal:{
                type: Boolean,
            },
            isLoading:{
                type: Boolean,
                default: false,
            },
            width:{
                type: Number,
                default: 500
            },
        },
        components:{
            SimpleImageEditor
        },
        setup( props, { emit, refs } ){

            const imageBase64 = ref(props.image)
            const InitImageBase64 = () => imageBase64.value = props.image || ""
            watch( () => props.image, InitImageBase64 )
            watch( () => props.isOpenModal, () => InitImageBase64)

            const isOpenModalRef = computed({
                get: () => props.isOpenModal as boolean,
                set: (value) => emit("update:isOpenModal", value)
            })

            function HandleClickOK() {
                emit("updateImage", imageBase64.value)
            }

            function HandleClickCancel() {
                isOpenModalRef.value = false;
            }

            function HandleClickDelete() {
                emit("deleteImage")
            }

            const isDisabledOK = computed( () => {
                if( props.isLoading ){
                    return true
                }
                
                if( imageBase64.value.length == 0 ){
                    return true
                }

                if( props.image == imageBase64.value ){
                    return true
                }

                return false
            })

            return {
                isOpenModalRef,
                isDisabledOK,
                imageBase64,
                HandleClickOK,
                HandleClickCancel,
                HandleClickDelete,
            }
        }
    })
</script>

<style lang="scss">
    .simple-imagee-ditor-content{

        .action-btn{
           width: 120px !important;
        }
    }
</style>