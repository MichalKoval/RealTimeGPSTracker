import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginModule } from './login/login.module';
import { RegistrationModule } from './registration/registration.module';
import { AuthComponent } from './auth.component';
import { AuthRoutingModule } from './auth-routing.module';
import { NzCardModule } from 'ng-zorro-antd';
import { ErrorPageModule } from '../error-page/error-page.module';

@NgModule({
  imports: [
    AuthRoutingModule,
    CommonModule,
    LoginModule,
    RegistrationModule,
    ErrorPageModule,
    NzCardModule
  ],
  exports: [
    AuthComponent
  ],
  declarations: [
    AuthComponent
  ]
})
export class AuthModule { }
