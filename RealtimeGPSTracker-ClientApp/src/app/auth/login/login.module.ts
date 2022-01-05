import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';
import { LoginRoutingModule } from './login-routing.module';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';
import { NzFormModule} from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzSpinModule } from 'ng-zorro-antd/spin';

@NgModule({
  imports: [
    LoginRoutingModule,
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
    NzCheckboxModule,
    NzAlertModule,
    NzSpinModule
  ],
  exports: [
    LoginComponent
  ],
  declarations: [
    LoginComponent
  ]
})
export class LoginModule { }
