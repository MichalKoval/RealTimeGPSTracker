import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizationGuard } from './core/services/auth-guard.service';
import { Error404Component } from './error-page/error-404/error-404.component';

const routes: Routes = [
    { 
        path: 'dashboard',
        loadChildren: () => 
            import(`./dashboard/dashboard.module`)
            .then(m => m.DashboardModule)
    },
    {
        path: 'auth',
        loadChildren: () => 
            import(`./auth/auth.module`)
            .then(m => m.AuthModule)
    },
    { 
        path: '', pathMatch: 'full', redirectTo: 'dashboard'
    },
    {
        path: '**',
        redirectTo: 'error/404'
    },
    {
        path: 'error',
        loadChildren: () => 
            import(`./error-page/error-page.module`)
            .then(m => m.ErrorPageModule)
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
