import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Device } from 'src/app/core/models/device.model';
import { convertFromUTCDatabaseToLocalClient } from 'src/app/core/services/moment.service';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss']
})
export class DeviceComponent implements OnInit {
  // Device flag - device is selected
  private _isSelected: boolean = false;

  @Output() isSelectedChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input()
  set isSelected(isSelected: boolean) {
    this._isSelected = isSelected;
    this.isSelectedChange.emit(this._isSelected);
  }

  get isSelected(): boolean {
    return this._isSelected;
  }

  // Device flag - device is edited
  private _isEdited: boolean = false;
  
  @Output() isEditedChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input()
  set isEdited(isEdited: boolean) {
    this._isEdited = isEdited;
    this.isEditedChange.emit(this._isEdited);
  }

  get isEdited(): boolean {
    return this._isEdited;
  }

  // Device flag - device is updated
  private _isUpdated: boolean = false;
  
  @Output() isUpdatedChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input()
  set isUpdated(isUpdated: boolean) {
    this._isUpdated = isUpdated;
    this.isUpdatedChange.emit(this._isUpdated);
  }

  get isUpdated(): boolean {
    return this._isUpdated;
  }

  // Device flag - device is about to be added
  private _isNew: boolean = false;
  
  @Input()
  set isNew(isNew: boolean) {
    this._isNew = isNew;
  }

  get isNew(): boolean {
    return this._isNew;
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
  uuidVisible = false;

  fromUTCDatabaseToLocalClient = convertFromUTCDatabaseToLocalClient;

  constructor() {
    console.log('Device component instantiated.')
  }

  ngOnInit() {
  }

  onSelectedChange() {
    this.isSelected = !this.isSelected;
  }

  uuidShowChange(change: boolean) {

  }

  uuidShowOk(): void {
    this.uuidVisible = false;
  }

  startEdit(): void {
    this.isEdited = true;
  }

  cancelEdit(): void {
    this.isEdited = false;
  }

  saveEdit(): void {
    this.isEdited = false;
    this.isUpdated = true;
  }

}
