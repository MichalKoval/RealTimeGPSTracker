import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { LoginComponent } from './login.component';

export const routes: Routes = [
    { 
        path: '',
        component: LoginComponent
    },
    {
        path: '**',
        redirectTo: 'error/404'
    },
    {
        path: 'error',
        loadChildren: () => 
            import(`../../error-page/error-page.module`)
            .then(m => m.ErrorPageModule)
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class LoginRoutingModule { }