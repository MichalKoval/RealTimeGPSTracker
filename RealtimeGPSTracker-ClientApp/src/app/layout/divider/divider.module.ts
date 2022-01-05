import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DividerComponent } from './divider.component';

//Custom app divider based on NG Zorro Ant library. (Angular version of Ant) 
@NgModule({
  imports: [
    CommonModule
  ],
  exports: [
    DividerComponent
  ],
  declarations: [
    DividerComponent
  ]
})
export class DividerModule { }
