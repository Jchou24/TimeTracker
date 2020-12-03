import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import VueCompositionAPI from '@vue/composition-api'

import Toast from "vue-toastification"
import "vue-toastification/dist/index.css"

// for icon
import 'material-design-icons/iconfont/material-icons.css'

Vue.config.productionTip = false

Vue.use(VueCompositionAPI)
Vue.use(Toast)

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')
