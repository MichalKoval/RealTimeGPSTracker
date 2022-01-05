import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IDevicesItem } from 'src/app/core/models/device.model';
import { mapDevice, mapIDevicesItem } from 'src/app/core/services/device.service';
import { convertFromUTCDatabaseToLocalClient } from 'src/app/core/services/moment.service';

@Component({
  selector: 'app-device-item',
  templateUrl: './device-item.component.html',
  styleUrls: ['./device-item.component.scss']
})
export class DeviceItemComponent implements OnInit {
  // Devices item detail
  private _deviceItem: IDevicesItem;

  @Input()
  set deviceItem(deviceItem: IDevicesItem) {
    this._deviceItem = deviceItem;
  }
  
  get deviceItem(): IDevicesItem {
    return this._deviceItem;
  }
  
  // When device item changed against original device item or is in edit or select state
  @Output() deviceItemChange: EventEmitter<IDevicesItem> = new EventEmitter<IDevicesItem>();

  deviceDetailSize = 'small';
  uuidVisible = false;

  mapDevice = mapDevice;
  mapIDevicesItem = mapIDevicesItem;
  fromUTCDatabaseToLocalClient = convertFromUTCDatabaseToLocalClient;

  constructor() {
    console.log('Device component instantiated.')
  }

  ngOnInit() {
  }

  onSelectedChange() {
    this.deviceItemChange.emit({
      ...this.deviceItem,
      // set select flag
      isSelected: !this.deviceItem.isSelected
    });
  }

  onUuidShowOk(): void {
    this.uuidVisible = false;
  }

  validateName(name: string): boolean {
    return true;
  }

  validateInterval(interval: number): boolean {
    return true;
  }

  validateColor(color: string): boolean {
    return true;
  }

  onNameChange(nameChanged: string): void {
    
    if (this.validateName(nameChanged)) {
      
      this.deviceItemChange.emit({
        ...this.deviceItem,
        name: nameChanged,
        // set select flag
        isEdited: true
      });
    }
  }

  onIntervalChange(intervalChanged: number): void {
    
    if (this.validateInterval(intervalChanged)) {
      
      this.deviceItemChange.emit({
        ...this.deviceItem,
        interval: intervalChanged,
        // set select flag
        isEdited: true
      });
    }
  }

  onColorChange(colorChanged: string): void {
    
    if (this.validateColor(colorChanged)) {
      
      this.deviceItemChange.emit({
        ...this.deviceItem,
        color: colorChanged,
        // set select flag
        isEdited: true
      });
    }
  }

  onEdit(): void {
    this.deviceItemChange.emit({
      ...this.deviceItem,
      // set select flag
      isEdited: true
    });
  }

  onCancelEdit(): void {
    this.deviceItemChange.emit({
      ...this.deviceItem,
      // set select flag
      isEdited: false,
      // Also when addition of a new device was canceled 
      toAdd: false
    });
  }

  onSaveEdit(): void {
    // TODO: get data from device-detail-input form

    this.deviceItemChange.emit({
      // TODO: get data from device-detail-input form
      ...this.deviceItem,
      // set select flag
      isEdited: false,
      toUpdate: true
    });
  }

}
