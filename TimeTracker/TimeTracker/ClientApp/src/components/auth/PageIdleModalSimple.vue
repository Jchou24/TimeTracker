<template>
    <div class="PageIdleModal-Wrapper">
        <transition
            name="custom-classes-transition"
            enter-active-class="animate__animated animate__fadeIn animate__faster"
            leave-active-class="animate__animated animate__fadeOut animate__faster"
        >
            <div class="PageIdleModal" v-if="isShowModal"
              @click.self="HandleClickBackground"
              >
              <div class="content">
                  <slot>
                  </slot>
              </div>
            </div>
        </transition>
    </div>
</template>

<script lang="ts">
    import { defineComponent, computed, Ref } from '@vue/composition-api';
    import 'animate.css/animate.min.css'

    export default defineComponent({
        name: 'PageIdleModal',
        props: {
            isShowModal:{
                type: Boolean,
                // required: true
            },
            isBackgroundCancellable:{
                type: Boolean,
                default: true
            },
        },
        setup(props, { emit }){
            // console.log(props.isShowModal)
            const modelValue = computed({ 
                get: () => props.isShowModal, 
                set: (value) => emit('update:isShowModal', value) 
            }) 

            function OpenModal(){
                modelValue.value = true
            }

            function CloseModal(){
                modelValue.value = false
            }

            function HandleClickBackground(){
                if(props.isBackgroundCancellable){
                    CloseModal()
                }
            }

            return {
                OpenModal,
                CloseModal,
                HandleClickBackground
            }
        }
    });
</script>


<style scoped lang="scss">
    .PageIdleModal{
        position: fixed;
        left: 0;
        top: 0;
        width: 100vw;
        height: 100vh;
        padding: 0;
        margin: 0;
        display: -webkit-box;
        display: -moz-box;
        display: -ms-flexbox;
        display: -webkit-flex;
        display: flex;
        justify-content: center;
        align-items: center;
        align-content: center;
        background: rgba(0, 0, 0, 0.5);
        z-index: 9999;
    }

    .PageIdleModal .content{
        background: white;
        border-radius: 15px;
        // width: 50%;
        min-width: 100px;
        // height: 50%;
        min-height: 100px;
        display: -webkit-box;
        display: -moz-box;
        display: -ms-flexbox;
        display: -webkit-flex;
        display: flex;
        justify-content: center;
        align-items: center;
        align-content: center;
    }
</style>
