import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { SettingsComponent } from './settings.component';

export const routes: Routes = [
    { 
        path: '',
        component: SettingsComponent
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
  export class SettingsRoutingModule { }