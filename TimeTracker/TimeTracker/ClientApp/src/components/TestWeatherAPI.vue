<template>
    <div class="">
        {{weather}}
    </div>
</template>

<script lang="ts">
    import { defineComponent, onMounted, ref } from '@vue/composition-api'
    import axios, { AxiosResponse } from 'axios';

    export default defineComponent({
        name: 'TestWeatherAPI',
        setup(){
            const weather: any = ref([123]);

            onMounted(()=>{
                axios.get(process.env.VUE_APP_SERVER_URL + 'WeatherForecast').then(response =>{
                    weather.value = response.data
                    console.log(weather)
                }, error => {
                    console.log(error)
                    console.log(error.response.status)
                    if( error.response.status == 401 ){
                        weather.value = "401 Unauthorize, please log in to access Weather API"
                    }
                })
            })

            return {
                weather
            }
        }

    });
</script>

