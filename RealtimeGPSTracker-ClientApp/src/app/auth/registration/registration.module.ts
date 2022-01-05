import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationComponent } from './registration.component';
import { RegistrationRoutingModule } from './registration-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzSpinModule } from 'ng-zorro-antd/spin';

@NgModule({
  imports: [
    RegistrationRoutingModule,
    CommonModule,
    FormsModule,
    LayoutModule,
    ErrorPageModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzAlertModule,
    NzSpinModule
  ],
  exports: [
    RegistrationComponent
  ],
  declarations: [
    RegistrationComponent
  ]
})
export class RegistrationModule { }
