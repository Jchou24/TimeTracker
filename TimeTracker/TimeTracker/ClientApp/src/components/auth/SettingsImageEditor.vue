<template>
    <div class="SettingsImageEditor">
        <SimpleImageEditorModal 
            :image="userImage"
            :isLoading="isLoading"
            :isOpenModal.sync="isOpenSimpleImageEditor" 
            @updateImage="UpdateUserImage"
            @deleteImage="DeleteUserImage"
            />

        <v-btn class="update-icon" icon
            :class="{'use-image': isUseUserImage}"
            :elevation=" isUseUserImage ? 4 : 0"
            @click="isOpenSimpleImageEditor = true"  >

            <v-img class="user-image-big" :src="userImage" v-if="isUseUserImage" />

            <v-icon class="default-icon" v-else
            >mdi-account-circle</v-icon>

        </v-btn>
    </div>
</template>


<script lang="ts">
    import { computed, defineComponent, ref } from '@vue/composition-api'

    import SimpleImageEditorModal from '@/components/img/SimpleImageEditorModal.vue'

    import { useToast } from "vue-toastification/composition"
    import { ToastSuccess, ToastWarning } from '@/util/notification.ts'
    import { Store } from 'vuex/types/index'
    import { IStore } from '@/models/store'
    import { ImageAPIHandler } from '@/api/imges'

    export default defineComponent({
        name: 'SettingsImageEditor',
        props:{
        },
        components:{
            SimpleImageEditorModal
        },
        setup(props, { emit, root }){
            const { $store, $router } = root
            // const store = useStore()
            const store = $store as Store<IStore>
            const router = $router
            const toast = useToast()
            const imageAPIHandler = new ImageAPIHandler( store, router )

            const userImage = ref("")
            const isLoading = ref(false)
            const isOpenSimpleImageEditor = ref(false)
            const isUseUserImage = computed( () => userImage.value.length > 0 )

            function InitUserImage() {
                imageAPIHandler.GetImage(isLoading, (response) => {
                    userImage.value = response.data
                })
            }
            InitUserImage()            

            function UpdateUserImage(updatedUserImage: string){
                imageAPIHandler.CreateOrUpdateImage(updatedUserImage, isLoading, () => {
                    userImage.value = updatedUserImage
                    ToastSuccess(`Update image success.`, toast)
                    isOpenSimpleImageEditor.value = false
                }, () => {
                    ToastWarning(`Sorry, the image is not updated.`, toast)
                })
            }

            function DeleteUserImage(){
                imageAPIHandler.DeleteImage(isLoading, () => {
                    userImage.value = ""
                    ToastSuccess(`Delete image success.`, toast)
                    isOpenSimpleImageEditor.value = false
                }, () => {
                    ToastWarning(`Sorry, the image is not deleted.`, toast)
                })
            }

            return {
                isLoading,
                isOpenSimpleImageEditor,
                isUseUserImage,
                userImage,
                UpdateUserImage,
                DeleteUserImage,
            }
        }
    })
</script>

<style lang="scss" scoped>
    .SettingsImageEditor{

        display: flex;
        justify-content: center;
        align-items: center;

        .update-icon{
            width: 256px;
            height: 256px;
            margin-top: -40px;
            margin-bottom: 10px;

            .default-icon{
                font-size: 256px ;
            }

            &.use-image{
                @include vm-drop-shadow-1(5px, 5px, 5px, rgba(197, 195, 195, 0.5));
            }
        }

    }
</style>