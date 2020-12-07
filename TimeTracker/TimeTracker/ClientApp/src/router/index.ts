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

// const routes: Array<RouteConfig> = [
//     {
//         path: '/',
//         name: 'Home',
//         component: Home
//     },
//     {
//         path: '/TaskEditor',
//         name: 'TaskEditor',
//         component: () => import('../views/TaskEditor.vue')
//     },
//     {
//         path: '/TaskReporter',
//         name: 'TaskReporter',
//         component: () => import('../views/TaskReporter.vue')
//     },
//     {
//         path: '/PermissionEditor',
//         name: 'PermissionEditor',
//         component: () => import('../views/PermissionEditor.vue')
//     },
//     {
//         path: '/TrackSettings',
//         name: 'TrackSettings',
//         component: () => import('../views/TrackSettings.vue')
//     },
//     {
//         path: '/Registration',
//         name: 'Registration',
//         component: () => import('../views/Registration.vue')
//     },
//     {
//         path: '/IndividualSettings',
//         name: 'IndividualSettings',
//         component: () => import('../views/IndividualSettings.vue')
//     },
// ]

export default function(store: Store<any>): VueRouter{
    const router = new VueRouter({
        mode: 'history',
        base: process.env.BASE_URL,
        routes
    })

    ConfigRouteValidation(router, store)

    return router
}

