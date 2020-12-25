<template>
    <div class="SimpleImageEditor">
        <VImageInput class="editor" 
            clearable 
            clearIcon="mdi-image-remove"        
            v-model="imageBase64"
            image-format="jpeg"
            :disabled="isLoading"
            :image-quality="0.85"            
            :imageHeight="256"
            :imageWidth="256"
            ref="VImageInput"
        />

        <v-btn icon class="delete-btn" :disabled="isLoading" v-if="imageBase64.length > 0"
            @click="HandleClickDelete">
            <v-icon>mdi-delete</v-icon>
        </v-btn>
    </div>
</template>


<script lang="ts">
    import { computed, defineComponent } from '@vue/composition-api'

    import VImageInput from 'vuetify-image-input'

    interface IVImageInputMethods{
        clear: () => void;
    }

    export default defineComponent({
        name: 'SimpleImageEditor',
        props:{
            image:{
                type: String,
                default: ""
            },
            isLoading:{
                type: Boolean,
                default: false,
            },
        },
        components:{
            VImageInput
        },
        setup( props, { emit, refs } ){

            const imageBase64 = computed({
                get: () => props.image || "",
                set: (value: string) => emit("update:image", value || "")
            })

            function Clear(){
                (refs.VImageInput as IVImageInputMethods)?.clear()
            }

            function HandleClickDelete() {
                Clear()
                emit("deleteImage")
            }

            return {
                imageBase64,
                Clear,
                HandleClickDelete,
            }
        }
    })
</script>

<style lang="scss" scoped>
    .SimpleImageEditor{
        position: relative;
        padding-left: 44px;

        .delete-btn{
            position: absolute;
            right: 4px;
            top: 40px;
        }
        // .delete-btn{
        //     position: absolute;
        //     right: 4px;
        //     top: 0px;
        // }

        .editor{
            width: 436px;
            height: 404px;
        }
    }
</style>

<style lang="scss">
    .SimpleImageEditor .editor > div > div > div{
        border-radius: 256px;
    }
</style>