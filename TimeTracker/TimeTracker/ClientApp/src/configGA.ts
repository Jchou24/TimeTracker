

import { VueConstructor } from 'vue'
import VueAnalytics from 'vue-analytics'
import VueRouter from 'vue-router'

function ConfigGA(vue: VueConstructor<Vue>, router: VueRouter) {
    const GA_ID = process.env.GA_ID
    if (process.env.NODE_ENV === 'production' && GA_ID ) {
        vue.use(VueAnalytics, {
            id: GA_ID,
            disableScriptLoader: false, // 是否停用下载ga.js
            router, // 切換 router 會自動統計
            autoTracking: {
                pageviewOnLoad: true
            }
        })
    }
}

export default ConfigGA