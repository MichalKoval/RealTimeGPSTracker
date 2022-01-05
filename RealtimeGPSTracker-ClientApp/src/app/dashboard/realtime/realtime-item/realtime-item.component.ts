import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { IDevicesItem } from 'src/app/core/models/device.model';

@Component({
  selector: 'app-realtime-item',
  templateUrl: './realtime-item.component.html',
  styleUrls: ['./realtime-item.component.scss']
})
export class RealtimeItemComponent implements OnInit {
  private _deviceItem: IDevicesItem = null;
  private _isSelected: boolean = false;

  // Device item bind property (one-way) -------------------------
  @Input()
  set deviceItem(deviceItem: IDevicesItem) {
    this._deviceItem = deviceItem  
  }  

  get deviceItem(): IDevicesItem {
    return this._deviceItem;
  }

  // Device item is selected bind property (two-way) -------------
  @Input()
  set isSelected(isSelected: boolean) {
    if (this._isSelected !== isSelected) {
      this._isSelected = isSelected;
      this.isSelectedChange.emit(this._isSelected);
    }    
  }  

  get isSelected(): boolean {
    return this._isSelected;
  }

  @Output() isSelectedChange: EventEmitter<boolean> = new EventEmitter<boolean>();


  constructor() { }

  ngOnInit() {
  }

}
