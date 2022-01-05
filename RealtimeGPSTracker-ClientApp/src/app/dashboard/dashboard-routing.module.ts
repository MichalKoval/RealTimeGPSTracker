import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { AuthorizationGuard } from '../core/services/auth-guard.service';

export const routes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        data: {
            breadcrumb: 'Dashboard'
        },
        children: [
            { 
                path: 'realtime',
                loadChildren: () => 
                    import(`./realtime/realtime.module`)
                    .then(m => m.RealtimeModule),
                canActivate: [AuthorizationGuard]
            },
            { 
                path: 'history',
                loadChildren: () => 
                    import(`./history/history.module`)
                    .then(m => m.HistoryModule),
                canActivate: [AuthorizationGuard]
            },
            { 
                path: 'devices',
                loadChildren: () => 
                    import(`./devices/devices.module`)
                    .then(m => m.DevicesModule),
                canActivate: [AuthorizationGuard]
            },
            { 
                path: 'user',
                loadChildren: () => 
                    import(`./user/user.module`)
                    .then(m => m.UserModule),
                canActivate: [AuthorizationGuard]
            },{ 
                path: 'settings',
                loadChildren: () => 
                    import(`./settings/settings.module`)
                    .then(m => m.SettingsModule),
                canActivate: [AuthorizationGuard]
            },
            {
                path: '',
                redirectTo: 'realtime',
                pathMatch: 'full'
            }
        ]
    },    
    {
        path: '**',
        redirectTo: 'error/404'
    },
    {
        path: 'error',
        loadChildren: () => 
            import(`../error-page/error-page.module`)
            .then(m => m.ErrorPageModule)
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class DashboardRoutingModule { }