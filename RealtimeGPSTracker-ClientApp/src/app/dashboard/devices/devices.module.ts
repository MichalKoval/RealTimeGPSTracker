import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevicesComponent } from './devices.component';
import { DeviceFilterComponent } from './device-filter/device-filter.component';
import { DeviceNewComponent } from './device-new/device-new.component';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { DevicesRoutingModule } from './devices-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QRCodeModule } from 'angularx-qrcode';
import { ColorPickerModule } from 'ngx-color-picker';
import { DeviceComponent } from './device/device.component';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { DeviceDetailInputComponent } from './device-detail-input/device-detail-input.component';
import { DeviceItemComponent } from './device-item/device-item.component';

@NgModule({
  imports: [
    DevicesRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    LayoutModule,
    ErrorPageModule,
    IconsProviderModule,
    NzTableModule,
    NzBadgeModule,
    NzButtonModule,
    NzTagModule,
    NzDividerModule,
    NzInputModule,
    NzPopoverModule,
    NzIconModule,
    NzDescriptionsModule,
    NzFormModule,
    NzPopconfirmModule,
    NzCheckboxModule,
    NzTabsModule,
    QRCodeModule,
    ColorPickerModule,
    NzSpinModule,
    NzAlertModule,
    NzSelectModule,
    NzToolTipModule,
    NzRadioModule,
    NzModalModule,
    NzDrawerModule
  ],
  exports : [
    DevicesComponent
  ],
  declarations: [
    DevicesComponent,
    DeviceComponent,
    DeviceItemComponent,
    DeviceFilterComponent,
    DeviceNewComponent,
    DeviceDetailInputComponent
  ]
})
export class DevicesModule { }
