<template>
    <div class="DigitalClock" :class="cls">
        <div class="date">{{ date }}</div>
        <div class="time">{{ time }}</div>
    </div>
</template>

<script>
    export default {
        name: "DigitalClock",
        props:{
            size:{
                type: String,
                default: 'lg'
            },
            week:{
                type: Array,
                default: () => ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'],
            },
        },
        data(){
            return {
                time: "",
                date: "",
            }
        },
        computed:{
            cls:{
                get(){
                    const tmp = {}
                    tmp[this.size] = true
                    return tmp
                }
            }
        },
        methods:{
            updateTime(){
                function zeroPadding(num, digit) {
                    let zero = '';
                    for(let i = 0; i < digit; i++) {
                        zero += '0';
                    }
                    return (zero + num).slice(-digit);
                }
                
                const cd = new Date();
                this.time = zeroPadding(cd.getHours(), 2) + ':' + zeroPadding(cd.getMinutes(), 2) + ':' + zeroPadding(cd.getSeconds(), 2);
                this.date = zeroPadding(cd.getFullYear(), 4) + '-' + zeroPadding(cd.getMonth()+1, 2) + '-' + zeroPadding(cd.getDate(), 2) + ' ' + this.week[cd.getDay()];
            }
        },
        mounted(){
            const week = ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'];
            const timerID = setInterval(this.updateTime, 1000);
            this.updateTime();            
        }
    }
</script>

<style lang="scss" scoped>
    .DigitalClock{
        font-family: 'Share Tech Mono', monospace;
        color: #ffffff;
        text-align: center;
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        color: #daf6ff;
        text-shadow: 0 0 20px rgba(10, 175, 230, 1),  0 0 20px rgba(10, 175, 230, 0);
        .time {
            letter-spacing: 0.05em;
            font-size: 72px;
            padding: 5px 0;
            margin-top: -12px;
        }

        .date {
            letter-spacing: 0.1em;
            font-size: 24px;
        }

        &.xs{
            .time {
                font-size: 56px;
            }

            .date {
                font-size: 20px;
            }
        }
    }

    .DigitalClock{
        @include vm-drop-shadow-1(0px, 0px, 30px, rgba(197, 195, 195, 0.2));
    }
</style>
