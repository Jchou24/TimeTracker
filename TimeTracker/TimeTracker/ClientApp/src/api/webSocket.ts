import { WSMapCode } from '@/models/constants/webSocket';
import { IStore } from '@/models/store';
import * as signalR from '@microsoft/signalr'
import { watch } from '@vue/composition-api';
import VueRouter from 'vue-router';
import { Store } from 'vuex/types/index';
import debounce from 'lodash.debounce';
import { AuthenticationAPIHandler } from './authentication';


class WSHandler{
    public connection: signalR.HubConnection
    protected _store: Store<IStore>
    protected _rootRouter: VueRouter
    protected _authenticationAPIHandler: AuthenticationAPIHandler

    constructor( store: Store<IStore>, router: VueRouter ) {
        this._store = store
        this._rootRouter = router
        this._authenticationAPIHandler = new AuthenticationAPIHandler(store, router)
        this.connection = this.BuildConnection()

        this.Init()
    }    

    protected BuildConnection(): signalR.HubConnection{
        return new signalR.HubConnectionBuilder()
            .withUrl(process.env.VUE_APP_SERVER_URL+"wssync")
            .build();
    }

    public Init(){
        this.InitialListeners()

        if( this._store.state.authentication.isAuthenticated ){
            this.Start()
        }        
    }

    protected InitialListeners(){
        watch( () => this._store.state.authentication.isAuthenticated, debounce((isAuthenticated: boolean) => {                
            if(isAuthenticated){
                this.Start()
            }else{
                this.Stop()
            }
        }, 250))
        
        this.connection.on( WSMapCode[WSMapCode.GetUserInfo], () => {
            // console.log("GetUserInfo")
            this._authenticationAPIHandler.GetUserInfo()
        });
    }

    public Start(){
        // console.log("Start WSHandler")
        return this.connection.start()
    }

    public Stop(){
        // console.log("Stop WSHandler")
        return this.connection.stop()
    }
}

export {
    WSHandler
}