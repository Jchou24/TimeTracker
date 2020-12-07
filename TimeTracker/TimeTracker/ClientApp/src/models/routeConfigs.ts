import { VueConstructor } from "vue";

import { UserRoles } from '@/models/constants/authentication'

interface IRouteConfigs{
    [key: string]: IRouteConfig;
}

interface IRouteConfig{
    path: string;
    name: string;
    component: VueConstructor<Vue> | ( () => Promise<typeof import("*.vue")> );
    auth: IRouteAuth;
    nav: IRouteNav;
}

interface IRouteAuth{
    isRequiredAuthentication: boolean;
    isRequiredApprovedAccount: boolean;
    requiredRoles: Array<UserRoles>;
}

interface IRouteNav{
    displayName: string;
    mdiIcon: string;
}

export {
    IRouteConfigs,
    IRouteConfig,
    IRouteAuth,
    IRouteNav,
}