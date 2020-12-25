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
    protected _isSuccessClose = false

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
            // console.log("try reconnecting")
            if (!this._store.state.authentication.isAuthenticated) {
                return
            }

            this._tryingToReconnect = true
            this._store.state.notification.NotificateWSReconnecting()
        })

        this.connection.onreconnected( () => {
            // console.log("try reconnect")
            if (!this._store.state.authentication.isAuthenticated) {
                return
            }

            this._tryingToReconnect = false
            this._store.state.notification.NotificateWSReconnected()
        })

        this.connection.onclose( () => {
            if (!this._isSuccessClose) {
                this._store.state.notification.NotificateWSClose()
            }
        })
    }

    public Start(){
        // console.log("Start WSHandler")
        this._isSuccessClose = false
        this._tryingToReconnect = false
        return this.connection.start()
    }

    public Stop(){
        // console.log("Stop WSHandler")
        this._isSuccessClose = true
        return this.connection.stop()
    }

    public TryReconnect( SuccessCallBack?: () => void, ErrorCallBack?: () => void ){
        if (this._tryingToReconnect) {
            return true
        }

        this._tryingToReconnect = true
        this.Stop()
            .then(() => {
                this.Start()
                    .then(() => {
                        this._tryingToReconnect = false
                        this.InitialListeners()

                        if(SuccessCallBack){
                            SuccessCallBack()
                        }
                    })
                    .catch(ErrorCallBack)
            })
            .catch(ErrorCallBack)
    }
}

export {
    WSHandler
}