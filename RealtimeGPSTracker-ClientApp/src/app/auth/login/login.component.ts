import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { login, resetAuthState } from 'src/app/core/store/actions/auth.actions';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromAuth from 'src/app/core/store/selectors/auth.selectors';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  validateLoginForm!: FormGroup;

  loginIsLoading$: Observable<boolean>;
  loginLoaded$: Observable<boolean>;
  loginError$: Observable<string[]>;
  
  constructor(
    private store: Store<fromStore.State>,
    private formBuilder: FormBuilder
  ) {
    console.log("Login component instantiated.");

    this.loginIsLoading$ = this.store.select(fromAuth.selectAuthLoading);
    this.loginLoaded$ = this.store.select(fromAuth.selectAuthLoaded);
    this.loginError$ = this.store.select(fromAuth.selectAuthError);
  }

  ngOnInit() {
    this.store.dispatch(resetAuthState());
    
    this.validateLoginForm = this.formBuilder.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
      rememberSignIn: [true]
    });
  }

  onSubmit() {
    for (const i in this.validateLoginForm.controls) {
      this.validateLoginForm.controls[i].markAsDirty();
      this.validateLoginForm.controls[i].updateValueAndValidity();
    }

    this.store.dispatch(login({
        loginRequest: {
            userName: this.validateLoginForm.value.userName,
            password: this.validateLoginForm.value.password
        }
    }));
  }

}
