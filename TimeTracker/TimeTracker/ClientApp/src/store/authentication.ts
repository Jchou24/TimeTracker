import { IAuthentication, IClaims } from '@/models/authentication'
import { Module } from "vuex";

const GetInitAuthentication = (): IAuthentication => ({
        isAuthenticated: false,
        claims: {
            guid: "",
            email: "",
            name: "",
            accountStatus: 0,
            userRoles: [],
        },
    })

function SetClaims( state: IAuthentication, claims: IClaims ){
    Object.keys(claims).forEach( key => Reflect.set( state.claims, key, Reflect.get(claims, key) ) )
}

const module: Module<IAuthentication, any> = {
    namespaced: true,
    state: GetInitAuthentication(),
    mutations: {
        Init: ( state ) => {
            const initAuthentication = GetInitAuthentication()
            Object.keys(initAuthentication).forEach( key => Reflect.set( state, key, Reflect.get(initAuthentication, key) ) )
            SetClaims(state, state.claims)
        },
        SetIsAuthenticated: ( state, isAuthenticated: boolean ) => state.isAuthenticated = isAuthenticated,
        SetClaims: ( state, claims: IClaims ) => SetClaims(state, claims),
        SetEmail: ( state, email: string ) => state.claims.email = email,
        SetName: ( state, name: string ) => state.claims.name = name,
    },
    actions: {
    },
    modules: {
    }
}

export default module