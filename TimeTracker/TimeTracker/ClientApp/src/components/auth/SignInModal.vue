<template>
    <div class="SignInModal text-center">
        <v-dialog
            v-model="isOpenModalRef"
            :width="width"
            >
            <SignIn :width="width" @loginSuccess="isOpenModalRef = false" ref="SignIn" />
        </v-dialog>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, ref, reactive } from '@vue/composition-api';

    import SignIn from './SignIn.vue'

    interface ISignInMethods{
        ResetPassword: () => void;
    }

    export default defineComponent({
        name: 'SignInModal',
        props:{
            isOpenModal:{
                type: Boolean,
            },
            width:{
                type: Number,
                default: 500
            },
        },
        components:{
            SignIn
        },
        setup(props, { emit, root, refs }){
            const isOpenModalRef = computed({
                get(){
                    return props.isOpenModal as boolean
                },
                set(value: boolean){
                    (refs.SignIn as ISignInMethods).ResetPassword()
                    emit("update:isOpenModal", value)
                }
            })

            return {
                isOpenModalRef,
            }
        }

    });
</script>
