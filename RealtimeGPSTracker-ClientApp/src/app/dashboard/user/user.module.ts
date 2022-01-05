import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user.component';
import { UserRoutingModule } from './user-routing.module';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzLayoutModule } from 'ng-zorro-antd/layout';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';

@NgModule({
  imports: [
    UserRoutingModule,
    CommonModule,
    FormsModule,
    LayoutModule,
    ErrorPageModule,
    IconsProviderModule,    
    ReactiveFormsModule,
    NzDividerModule,
    NzFormModule,
    NzAlertModule,
    NzSpinModule,
    NzModalModule,
    NzInputModule,
    NzButtonModule,
    NzLayoutModule
  ],
  exports: [
    UserComponent
  ],
  declarations: [
    UserComponent
  ]
})
export class UserModule { }
