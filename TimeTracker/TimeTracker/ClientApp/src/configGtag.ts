

import { VueConstructor } from 'vue'
import VueGtag from "vue-gtag"
import VueRouter from 'vue-router'

function ConfigGtag(vue: VueConstructor<Vue>, router: VueRouter) {
    const GA_ID = process.env.GA_ID
    console.log("check gtag", GA_ID)
    if (process.env.NODE_ENV === 'production' && GA_ID ) {
        console.log("enable gtag", GA_ID)

        vue.use(VueGtag, {
            config: { 
                id: GA_ID
            },
            appName: 'Time Tracker',
            pageTrackerScreenviewEnabled: true,
            onReady () {
                console.log("gtag ready")
            }
        }, router)
    }
}

export default ConfigGtag