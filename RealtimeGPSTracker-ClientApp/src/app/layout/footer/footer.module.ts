import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { FooterComponent } from './footer.component';

//Custom app footer based on NG Zorro Ant library. (Angular version of Ant) 
@NgModule({
  imports: [
    CommonModule,
    NzLayoutModule
  ],
  exports: [
    FooterComponent
  ],
  declarations: [
    FooterComponent
  ]
})
export class FooterModule { }
