import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { SidebarComponent } from './sidebar.component';
import { RouterModule } from '@angular/router';

//Custom app sidebar based on NG Zorro Ant library. (Angular version of Ant) 
@NgModule({
  imports: [
    CommonModule,
    IconsProviderModule,
    NzLayoutModule,
    NzCardModule,
    NzAvatarModule,
    NzMenuModule,
    NzIconModule,
    RouterModule
  ],
  exports: [
    SidebarComponent
  ],
  declarations: [
    SidebarComponent
  ]
})
export class SidebarModule { }
