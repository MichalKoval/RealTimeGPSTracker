import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Device } from 'src/app/core/models/device.model';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { checkHexColor } from 'src/app/core/services/color.service';

@Component({
  selector: 'app-device-detail-input',
  templateUrl: './device-detail-input.component.html',
  styleUrls: ['./device-detail-input.component.scss']
})
export class DeviceDetailInputComponent implements OnInit, OnDestroy {
  private _device: Device = null;
  
  @Output() deviceChange: EventEmitter<Device> = new EventEmitter<Device>();
  @Input()
  set device(device: Device) {
    this._device = device;
  }
  
  get device(): Device {
    return this._device;
  }

  validateDeviceDetailInputForm: FormGroup;
  deviceDetailSize = 'small';

  constructor(private formBuilder: FormBuilder) {
    console.log('Device Detail Input component instantiated.');
  }
  
  nameValidator = (control: FormControl): { [s: string]: boolean } => {
    //TODO: 
    
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.validateDeviceDetailInputForm?.controls['name']?.value) {
      return { confirm: true, error: true };
    }
    return {};
  };

  intervalValidator = (control: FormControl): { [s: string]: boolean } => {
    //TODO: 
    
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.validateDeviceDetailInputForm?.controls['interval']?.value) {
      return { confirm: true, error: true };
    }
    return {};
  };

  colorValidator = (control: FormControl): { [s: string]: boolean } => {
    //TODO: 
    
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.validateDeviceDetailInputForm?.controls['color']?.value) {
      return { confirm: true, error: true };
    }
    return {};
  };

  ngOnInit() {
    this.validateDeviceDetailInputForm = this.formBuilder.group({
      name: [ this.device.name, [Validators.required]],
      interval: [ this.device.interval, [Validators.required, this.intervalValidator]],
      color: [ this.device.color, [Validators.required, this.colorValidator]]
    });
  }
  
  onDeviceDetailSubmit() {
    for (const i in this.validateDeviceDetailInputForm.controls) {
      this.validateDeviceDetailInputForm.controls[i].markAsDirty();
      this.validateDeviceDetailInputForm.controls[i].updateValueAndValidity();
    }
    
    this.deviceChange.emit(this.device);
  }

  ngOnDestroy(): void {
    console.log('Device Detail Input component is disposed.')
  }
}
