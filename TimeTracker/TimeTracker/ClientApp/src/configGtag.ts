

import { VueConstructor } from 'vue'
import VueGtag from "vue-gtag"
import VueRouter from 'vue-router'

function ConfigGtag(vue: VueConstructor<Vue>, router: VueRouter) {
    const gaId = process.env.VUE_APP_GA_ID
    // console.log("check gtag", gaId)
    if (process.env.NODE_ENV === 'production' && gaId ) {
        vue.use(VueGtag, {
            config: { 
                id: gaId
            },
            appName: 'Time Tracker',
            pageTrackerScreenviewEnabled: true,
        }, router)
    }
}

export default ConfigGtag