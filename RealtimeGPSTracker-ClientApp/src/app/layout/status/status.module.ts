import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { StatusComponent } from './status.component';

//Custom app status based on NG Zorro Ant library. (Angular version of Ant) 
@NgModule({
  imports: [
    CommonModule,
    IconsProviderModule
  ],
  exports: [
    StatusComponent
  ],
  declarations: [
    StatusComponent
  ]
})
export class StatusModule { }
