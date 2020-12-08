import { VueRouter } from 'vue-router/types/router'
import { Store } from 'vuex/types/index'

import { IStore } from '@/models/store'
import { ValidateAuth } from '@/util/authValidation'
import { ValidationResults } from '@/models/authentication'
import { GetRedirectPath, routeConfigs } from './routeConfigs'

import { ToastWarning } from '@/util/notification'

function ConfigRouteValidation(router: VueRouter, store: Store<IStore> ){
    router.beforeEach((to, from, next) => { 
        // console.log( from.path, to.path )
        if (from.path == to.path) {
            next()
            return            
        }

        const result = ValidateAuth(to.name as string, store, routeConfigs)
        if( result != ValidationResults.ok ){
            // setTimeout( () => ToastWarning(store.state.notificator, `Insufficient permision found: ${ValidationResults[result]}`), 2000 )
            ToastWarning(`Insufficient permision found: ${ValidationResults[result]}`, store.state.notificator)
            next(GetRedirectPath(result))
            return
        }

        // store.state.notificator?.success("TEST OKKKK")
        next()
    })
}

export default ConfigRouteValidation