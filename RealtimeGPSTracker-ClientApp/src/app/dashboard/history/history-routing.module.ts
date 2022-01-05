import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HistoryComponent } from './history.component';
import { AuthorizationGuard } from 'src/app/core/services/auth-guard.service';

export const routes: Routes = [
    { 
        path: '',
        component: HistoryComponent
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
  export class HistoryRoutingModule { }