import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoryComponent } from './history.component';
import { HistorySearchComponent } from './history-search/history-search.component';
import { HistoryItemComponent } from './history-item/history-item.component';
import { HistoryRoutingModule } from './history-routing.module';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzTimelineModule } from 'ng-zorro-antd/timeline';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzEmptyModule } from 'ng-zorro-antd/empty';

import { FormsModule } from '@angular/forms';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';
import { MapModule } from 'src/app/layout/map/map.module';

@NgModule({
  imports: [
    HistoryRoutingModule,
    CommonModule,
    FormsModule,
    NzLayoutModule,
    NzCollapseModule,
    NzDatePickerModule,
    NzListModule,
    NzTimelineModule,
    NzSkeletonModule,
    NzAlertModule,
    NzSpinModule,
    NzMessageModule,
    NzButtonModule,
    NzIconModule,
    LayoutModule,
    ErrorPageModule,    
    NzDividerModule,
    NzSelectModule,
    NzInputModule,
    NzToolTipModule,
    NzCheckboxModule,
    NzGridModule,
    NzEmptyModule,
    IconsProviderModule,
    ScrollingModule,
    MapModule
  ],
  exports: [
    HistoryComponent
  ],
  declarations: [
    HistoryComponent,
    HistorySearchComponent,
    HistoryItemComponent
  ]
})
export class HistoryModule { }
