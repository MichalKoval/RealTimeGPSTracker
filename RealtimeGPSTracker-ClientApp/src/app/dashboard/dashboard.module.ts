import { DashboardRoutingModule } from './dashboard-routing.module';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { LayoutModule } from '../layout/layout.module';
import { RealtimeModule } from './realtime/realtime.module';
import { HistoryModule } from './history/history.module';
import { DevicesModule } from './devices/devices.module';
import { IconsProviderModule } from '../icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { UserModule } from './user/user.module';
import { ErrorPageModule } from '../error-page/error-page.module';

@NgModule({
  imports: [
    DashboardRoutingModule,
    CommonModule,
    RealtimeModule,
    HistoryModule,
    DevicesModule,
    UserModule,
    ErrorPageModule,
    LayoutModule,
    IconsProviderModule,
    NzLayoutModule
  ],
  exports: [
    DashboardComponent
  ],
  declarations: [
    DashboardComponent
  ]
})
export class DashboardModule { }
