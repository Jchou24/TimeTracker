import { ValidationResults } from '@/models/authentication'
import { Store } from 'vuex/types/index'
import { AccountStatus } from '../models/constants/authentication'
import { IRouteConfigs } from '../models/routeConfigs'
import { IStore } from '../models/store'

function ValidatePathExists(pathName: string, store: Store<IStore>, routeConfigs: IRouteConfigs){
    const isFoundPath = pathName in routeConfigs
    if( !isFoundPath ){
        return ValidationResults.invalidPath
    }

    return ValidationResults.ok
}

function ValidateAccountStatus(pathName: string, store: Store<IStore>, routeConfigs: IRouteConfigs){
    const isRequiredApprovedAccount = routeConfigs[pathName].auth.isRequiredApprovedAccount
    const isAccountApproved = store.state.authentication.claims.accountStatus == AccountStatus.Approved
    if( isRequiredApprovedAccount && !isAccountApproved ){
        return ValidationResults.invalidAccountStatus
    }

    return ValidationResults.ok
}

function ValidateAuthentication(pathName: string, store: Store<IStore>, routeConfigs: IRouteConfigs){
    const isRequiredAuthentication = routeConfigs[pathName].auth.isRequiredAuthentication
    const isAuthenticated = store.state.authentication.isAuthenticated
    if( isRequiredAuthentication && !isAuthenticated ){
        return ValidationResults.invalidAuthentication
    }

    return ValidationResults.ok
}

function ValidateRole(pathName: string, store: Store<IStore>, routeConfigs: IRouteConfigs){
    console.log("ValidateRole")
    const requiredRoles = routeConfigs[pathName].auth.requiredRoles
    const userRoleIds = store.state.authentication.claims.userRoles.map( x => x.id )

    if( requiredRoles.length == 0 ){
        return ValidationResults.ok
    }

    if( requiredRoles.filter( allowRole => userRoleIds.includes(allowRole) ).length !== requiredRoles.length ){
        return ValidationResults.invalidRole
    }

    return ValidationResults.ok
}

function ValidateAuth(pathName: string, store: Store<IStore>, routeConfigs: IRouteConfigs){
    const chaninedValidater: Array<(pathName: string, store: Store<IStore>, routeConfigs: IRouteConfigs) => ValidationResults> = [
        ValidatePathExists,
        ValidateAccountStatus,
        ValidateAuthentication,
        ValidateRole,
    ]

    for (const Validater of chaninedValidater) {
        const result = Validater(pathName, store, routeConfigs)
        if( result != ValidationResults.ok ){
            return result
        }
    }
    return ValidationResults.ok
}

export {
    ValidatePathExists,
    ValidateAccountStatus,
    ValidateAuthentication,
    ValidateRole,
    ValidateAuth,
}