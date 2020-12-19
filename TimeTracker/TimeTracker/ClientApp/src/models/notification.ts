import ToastInterface from 'vue-toastification/dist/types/src/ts/interface';

interface INotification{
    Notificate403: any;
    Notificate401: any;
    NotificateWSReconnected: any;
    NotificateWSReconnecting: any;
    NotificateWSClose: any;
    notificator: ReturnType<typeof ToastInterface> | undefined,
}


export {
    INotification
}