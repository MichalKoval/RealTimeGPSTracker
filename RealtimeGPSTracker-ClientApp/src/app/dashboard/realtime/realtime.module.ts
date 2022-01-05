import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RealtimeComponent } from './realtime.component';
import { RealtimeRoutingModule } from './realtime-routing.module';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

import { FormsModule } from '@angular/forms';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';
import { MapModule } from 'src/app/layout/map/map.module';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { RealtimeItemComponent } from './realtime-item/realtime-item.component';

@NgModule({
  imports: [
    RealtimeRoutingModule,
    CommonModule,
    FormsModule,
    LayoutModule,
    ErrorPageModule,
    IconsProviderModule,
    NzCollapseModule,
    NzButtonModule,
    NzListModule,
    NzSkeletonModule,
    NzAlertModule,
    NzSpinModule,
    NzIconModule,
    NzEmptyModule,
    NzDividerModule,
    NzToolTipModule,
    ScrollingModule,
    MapModule    
  ],
  exports: [
    RealtimeComponent
  ],
  declarations: [
    RealtimeComponent,
    RealtimeItemComponent
  ]
})
export class RealtimeModule { }
