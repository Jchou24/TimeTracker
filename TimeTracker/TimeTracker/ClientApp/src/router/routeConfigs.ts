import Home from '../views/Home.vue'

import { UserRoles } from '@/models/constants/authentication'
import { IRouteConfigs } from '@/models/routeConfigs'
import { ValidationResults } from '@/models/authentication'

const routeConfigs: IRouteConfigs = {
    Home: {
        path: '/',
        name: 'Home',
        component: Home,
        auth:{
            isRequiredAuthentication: false,
            isRequiredApprovedAccount: false,
            requiredRoles:[],
        },
        nav:{
            displayName: "Time Tracker",
            mdiIcon: "",
        }
    },
    TaskEditor: {
        path: '/TaskEditor',
        name: 'TaskEditor',
        component: () => import('../views/TaskEditor.vue'),
        auth:{
            isRequiredAuthentication: true,
            isRequiredApprovedAccount: true,
            requiredRoles:[ UserRoles.User ],
        },
        nav:{
            displayName: "工時填寫",
            mdiIcon: "mdi-circle-edit-outline",
        }
    },
    TaskReporter: {
        path: '/TaskReporter',
        name: 'TaskReporter',
        component: () => import('../views/TaskReporter.vue'),
        auth:{
            isRequiredAuthentication: true,
            isRequiredApprovedAccount: true,
            requiredRoles:[ UserRoles.User ],
        },
        nav:{
            displayName: "工時報表",
            mdiIcon: "mdi-view-quilt-outline",
        }
    },
    TaskSummaryReporter: {
        path: '/TaskSummaryReporter',
        name: 'TaskSummaryReporter',
        component: () => import('../views/TaskSummaryReporter.vue'),
        auth:{
            isRequiredAuthentication: true,
            isRequiredApprovedAccount: true,
            requiredRoles:[ UserRoles.User ],
        },
        nav:{
            displayName: "全體工時報表",
            mdiIcon: "mdi-view-dashboard-variant-outline",
        }
    },
    PermissionEditor: {
        path: '/PermissionEditor',
        name: 'PermissionEditor',
        component: () => import('../views/PermissionEditor.vue'),
        auth:{
            isRequiredAuthentication: true,
            isRequiredApprovedAccount: true,
            requiredRoles:[ UserRoles.Admin ],
        },
        nav:{
            displayName: "權限管理",
            mdiIcon: "mdi-account-edit",
        }
    },
    TrackSettings: {
        path: '/TrackSettings',
        name: 'TrackSettings',
        component: () => import('../views/TrackSettings.vue'),
        auth:{
            isRequiredAuthentication: true,
            isRequiredApprovedAccount: true,
            requiredRoles:[ UserRoles.Admin ],
        },
        nav:{
            displayName: "工時設定",
            mdiIcon: "mdi-cog-outline",
        }
    },
    Registration: {
        path: '/Registration',
        name: 'Registration',
        component: () => import('../views/Registration.vue'),
        auth:{
            isRequiredAuthentication: false,
            isRequiredApprovedAccount: false,
            requiredRoles:[],
        },
        nav:{
            displayName: "註冊",
            mdiIcon: "mdi-account-plus",
        }
    },
    IndividualSettings: {
        path: '/IndividualSettings',
        name: 'IndividualSettings',
        component: () => import('../views/IndividualSettings.vue'),
        auth:{
            isRequiredAuthentication: true,
            isRequiredApprovedAccount: false,
            requiredRoles:[],
        },
        nav:{
            displayName: "個人設定",
            mdiIcon: "mdi-account-circle",
        }
    }
}

function GetRedirectPath( validationResult: ValidationResults ){
    switch (validationResult) {
        case ValidationResults.ok:
            return "/"
        case ValidationResults.invalidPath:
            return "/"
        case ValidationResults.invalidAccountStatus:
            return "/"
        case ValidationResults.invalidAuthentication:
            return "/"
        case ValidationResults.invalidRole:
            return "/"
        case ValidationResults.logout:
            return "/"
        default:
            return "/"
    }
}

export {
    routeConfigs,
    GetRedirectPath
} 