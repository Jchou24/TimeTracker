import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import CreateRouter from './router/index'
import store from './store'
import VueCompositionAPI from '@vue/composition-api'
import ConfigGtag from './configGtag'

import vuetify from './vuetify'

import 'jctk-table-form'
import 'jctk-table-form/dist/jctk-table-form.css'

// for icon
import 'material-design-icons/iconfont/material-icons.css'
import '@mdi/font/css/materialdesignicons.css'

import 'animate.css/animate.min.css'

Vue.config.productionTip = false

Vue.use(VueCompositionAPI)

const router = CreateRouter(store)

ConfigGtag( Vue, router)

new Vue({
    vuetify,
    router,
    store,
    render: h => h(App)
}).$mount('#app')
