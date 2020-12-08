import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import { Store } from 'vuex/types/index'
// import Home from '../views/Home.vue'
import { routeConfigs } from './routeConfigs'
import ConfigRouteValidation from './routeRoleValidation' 

Vue.use(VueRouter)


const routes: Array<RouteConfig> = Object.values(routeConfigs).map( routeConfig => {
    return {
        path: routeConfig.path,
        name: routeConfig.name,
        component: routeConfig.component,
    }
})

export default function(store: Store<any>): VueRouter{
    const router = new VueRouter({
        mode: 'history',
        base: process.env.BASE_URL,
        routes
    })

    ConfigRouteValidation(router, store)

    return router
}

