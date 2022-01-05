import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Device } from 'src/app/core/models/device.model';
import { v4 as uuid } from 'uuid';
import { randomHexColor } from 'src/app/core/services/color.service';
import { OnDestroy } from '@angular/core';

@Component({
  selector: 'app-device-new',
  templateUrl: './device-new.component.html',
  styleUrls: ['./device-new.component.scss']
})
export class DeviceNewComponent implements OnInit, OnDestroy {
  // Device flag - device adding is canceled
  private _isCanceled: boolean = false;
  
  @Output() isCanceledChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input()
  set isCanceled(isCanceled: boolean) {
    this._isCanceled = isCanceled;
    this.isCanceledChange.emit(this._isCanceled);
  }

  get isCanceled(): boolean {
    return this._isCanceled;
  }

  // Device detail
  private _device: Device;

  @Output() deviceChange: EventEmitter<Device> = new EventEmitter<Device>();
  @Input()
  set device(device: Device) {
    this._device = device;
    this.deviceChange.emit(this._device);
  }
  
  get device(): Device {
    return this._device;
  }
  
  deviceDetailSize = 'small';

  constructor() {
    console.log('Device Add New component instantiated.');
  }
  
  ngOnInit() {
    // Prefilled name, interval and color fields with default values
    this.device.name = `Device_${uuid()}`.substring(0, 14);
    this.device.interval = 1000;
    this.device.color = randomHexColor();
  }

  ngOnDestroy(): void {
    console.log('Device Add New component is disposed.')
  }

  onDeviceDetailInputChange(device: Device) {

  }

  onCanceledChange(): void {
    this.isCanceled = !this.isCanceled;
  }

  onSave(): void {
  }

}
