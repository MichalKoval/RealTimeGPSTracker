import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorPageComponent } from './error-page.component';
import { ErrorPageRoutingModule } from './error-page-routing.module';
import { RouterModule } from '@angular/router';
import { NzResultModule } from 'ng-zorro-antd/result';

import { Error403Component } from './error-403/error-403.component';
import { Error404Component } from './error-404/error-404.component';
import { Error500Component } from './error-500/error-500.component';

@NgModule({
  imports: [
    ErrorPageRoutingModule,
    RouterModule,
    CommonModule,
    NzResultModule
    
  ],
  exports: [
    Error403Component,
    Error404Component,
    Error500Component,
    ErrorPageComponent
  ],
  declarations: [
    Error403Component,
    Error404Component,
    Error500Component,
    ErrorPageComponent
  ]
})
export class ErrorPageModule { }
