import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { RegistrationComponent } from './registration.component';

export const routes: Routes = [
    { 
        path: '',
        component: RegistrationComponent
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
  export class RegistrationRoutingModule { }