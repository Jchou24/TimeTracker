import { routeConfigs } from '@/router/routeConfigs'
import { ValidateAuth } from '@/util/authValidation'
import { ValidationResults } from '@/models/authentication'
import { Store } from 'vuex/types/index'
import { IStore } from './store'

const allNavLeftItems = [ "TaskEditor", "TaskReporter" ]
const allNavRightItems = [ "TrackSettings", "PermissionEditor" ]

function FiltNavItems(candidates: Array<string>, store: Store<IStore>): Array<string>{
    return candidates.filter( 
        pathName => ValidateAuth(pathName as string, store, routeConfigs) == ValidationResults.ok)
}

export {
    allNavLeftItems,
    allNavRightItems,

    FiltNavItems,
}