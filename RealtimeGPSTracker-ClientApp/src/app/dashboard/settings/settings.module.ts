import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { FormsModule } from '@angular/forms';
import { LayoutModule } from 'src/app/layout/layout.module';
import { ErrorPageModule } from 'src/app/error-page/error-page.module';
import { SettingsRoutingModule } from './settings-routing.module';
import { SettingsComponent } from './settings.component';

@NgModule({
  imports: [
    SettingsRoutingModule,
    CommonModule,
    FormsModule,
    LayoutModule,
    ErrorPageModule,
    IconsProviderModule,
    NzDividerModule,
    NzSwitchModule
  ],
  exports: [
    SettingsComponent
  ],
  declarations: [
    SettingsComponent
  ]
})
export class SettingsModule { }
