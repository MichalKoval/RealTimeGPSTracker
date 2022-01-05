import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { FooterComponent } from './footer/footer.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { StatusComponent } from './status/status.component';
import { LoaderComponent } from './loader/loader.component';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '../icons-provider.module';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzSpinModule } from 'ng-zorro-antd/spin';

//Custom app layout based on NG Zorro Ant library. (Angular version of Ant) 
@NgModule({
  imports: [
    CommonModule,
    IconsProviderModule,
    NzBreadCrumbModule,
    NzLayoutModule,
    NzPageHeaderModule,
    NzIconModule,
    NzCardModule,
    NzAvatarModule,
    NzMenuModule,
    NzDividerModule,
    NzButtonModule,
    NzGridModule,
    NzEmptyModule,
    NzSpinModule,
    RouterModule
  ],
  exports: [
    HeaderComponent,
    BreadcrumbComponent,
    FooterComponent,
    SidebarComponent,
    StatusComponent,
    LoaderComponent
  ],
  declarations: [
    HeaderComponent,
    BreadcrumbComponent,
    FooterComponent,
    SidebarComponent,
    StatusComponent,
    LoaderComponent
  ]
})
export class LayoutModule { }
