import { VueRouter, Route } from 'vue-router/types/router'
import { Store } from 'vuex/types/index'

import { IStore } from '@/models/store'
import { ValidateAuth } from '@/util/authValidation'
import { ValidationResults } from '@/models/authentication'
import { GetRedirectPath, routeConfigs } from './routeConfigs'

import { ToastWarning } from '@/util/notification'

function ConfigRouteValidation(router: VueRouter, store: Store<IStore> ){
    router.beforeEach((to, from, next) => {
        if (from.path == to.path) {
            next()
            return            
        }

        const result = ValidateAuth(to.name as string, store, routeConfigs)
        if( result != ValidationResults.ok ){
            // setTimeout( () => ToastWarning(store.state.notificator, `Insufficient permision found: ${ValidationResults[result]}`), 2000 )
            ToastWarning(`Insufficient permision found: ${ValidationResults[result]}`, store.state.notification.notificator)
            next(GetRedirectPath(result))
            return
        }

        next()
    })

    function HasQueryParams(route: Route) {
        return !!Object.keys(route.query).length
    }

    router.beforeEach((to, from, next) => {
        if( !HasQueryParams(to) && HasQueryParams(from) ){
            next({ name: to.name as string, query: from.query })
        } else {
            next()
        }
    })
}

export default ConfigRouteValidation