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
    protected _tryingToReconnect = false

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
        
        this.connection.onreconnecting( () => {
            if (!this._store.state.authentication.isAuthenticated) {
                return
            }

            this._tryingToReconnect = true
            this._store.state.notification.NotificateWSReconnecting()
        })

        this.connection.onreconnected( () => {
            if (!this._store.state.authentication.isAuthenticated) {
                return
            }

            this._tryingToReconnect = false
            this._store.state.notification.NotificateWSReconnected()
        })

        this.connection.onclose( () => {
            if (!this._store.state.authentication.isAuthenticated) {
                return
            }

            let retry = 3
            while( retry > 0 ){
                setTimeout( () => 
                    this.Start()
                        .then( () => retry = -1 )
                        .catch( () => retry -= 1 )
                , 5000)
            }

            if (retry == 0) {
                // retry success
                this._store.state.notification.NotificateWSReconnected()
            }else{
                // retry fail
                this._store.state.notification.NotificateWSClose()
            }
        })
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