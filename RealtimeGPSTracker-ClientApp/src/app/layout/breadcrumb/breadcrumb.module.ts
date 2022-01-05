import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { BreadcrumbComponent } from './breadcrumb.component';

//Custom app breadcrumb based on NG Zorro Ant library. (Angular version of Ant) 
@NgModule({
  imports: [
    CommonModule,
    IconsProviderModule,
    NzBreadCrumbModule
  ],
  exports: [
    BreadcrumbComponent
  ],
  declarations: [
    BreadcrumbComponent
  ]
})
export class BreadcrumbModule { }
