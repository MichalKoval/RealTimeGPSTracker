import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthComponent } from './auth.component';

export const routes: Routes = [
    {
        path: '',
        component: AuthComponent,
        children: [
            { 
                path: 'login',
                loadChildren: () => 
                    import(`./login/login.module`)
                    .then(m => m.LoginModule),
                data: {
                    breadcrumb: 'Login'
                }
            },
            { 
                path: 'register',
                loadChildren: () => 
                    import(`./registration/registration.module`)
                    .then(m => m.RegistrationModule),
                data: {
                    breadcrumb: 'Registration'
                }
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
  export class AuthRoutingModule { }