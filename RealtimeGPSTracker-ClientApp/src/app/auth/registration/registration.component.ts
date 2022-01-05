import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { registration, resetAuthState } from 'src/app/core/store/actions/auth.actions';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromAuth from 'src/app/core/store/selectors/auth.selectors';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  validateRegistrationForm: FormGroup;

  registrationIsLoading$: Observable<boolean>;
  registrationLoaded$: Observable<boolean>;
  registrationError$: Observable<string[]>;
  
  constructor(
    private store: Store<fromStore.State>,
    private formBuilder: FormBuilder
  ) {
    console.log("Registration component instantiated.");

    this.registrationIsLoading$ = this.store.select(fromAuth.selectAuthLoading);
    this.registrationLoaded$ = this.store.select(fromAuth.selectAuthLoaded);
    this.registrationError$ = this.store.select(fromAuth.selectAuthError);
  }

  updatePasswordConfirmValidator(): void {
    Promise.resolve().then(() => this.validateRegistrationForm.controls.checkPassword.updateValueAndValidity());
  }

  passwordConfirmationValidator = (control: FormControl): { [s: string]: boolean } => {
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.validateRegistrationForm.controls.password.value) {
      return { confirm: true, error: true };
    }
    return {};
  };

  ngOnInit(): void {
    this.store.dispatch(resetAuthState());

    this.validateRegistrationForm = this.formBuilder.group({
      userName: [null, [Validators.pattern('[a-zA-Z0-9_-]*'), Validators.required]],
      firstName: [null],
      lastName: [null],
      email: [null, [Validators.email, Validators.required]],
      password: [null, [Validators.required]],
      checkPassword: [null, [Validators.required, this.passwordConfirmationValidator]]
    });
  }

  onSubmit() {
    for (const i in this.validateRegistrationForm.controls) {
      this.validateRegistrationForm.controls[i].markAsDirty();
      this.validateRegistrationForm.controls[i].updateValueAndValidity();
    }

    this.store.dispatch(registration({
      registrationRequest: {
          firstName: this.validateRegistrationForm.value.firstName,
          lastName: this.validateRegistrationForm.value.lastName,
          userName: this.validateRegistrationForm.value.userName,
          email: this.validateRegistrationForm.value.email,
          password: this.validateRegistrationForm.value.password
        }
    }));
  }

}
