import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from 'src/app/core/models/user.model';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromUser from 'src/app/core/store/selectors/user.selectors';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { setCurrentDashboardTitle } from 'src/app/core/store/actions/dashboard.actions';
import { loadUser, updateUser, changeUserPassword } from 'src/app/core/store/actions/user.actions';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['../../app.component.css', 'user.component.scss']
})
export class UserComponent implements OnInit {
  private _title: string = 'User';
  
  validateUserDetailEditForm: FormGroup;
  validateUserPasswordEditForm: FormGroup;

  user$: Observable<IUser>;
  userIsLoading$ : Observable<boolean>;
  userLoaded$ : Observable<boolean>;
  userError$ : Observable<string[]>;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<fromStore.State>,
    private message: NzMessageService,
    private modal: NzModalService
  ) {
    console.log("User component instantiated.");
    this.store.dispatch(setCurrentDashboardTitle({title: this._title }));

    this.user$ = this.store.select(fromUser.selectUserDetail);
    this.userIsLoading$ = this.store.select(fromUser.selectUserDetailLoading);
    this.userLoaded$ = this.store.select(fromUser.selectUserDetailLoaded);
    this.userError$ = this.store.select(fromUser.selectUserDetailError);
  }

  updatePasswordConfirmValidator(): void {
    Promise.resolve().then(() => this.validateUserPasswordEditForm.controls.checkPassword.updateValueAndValidity());
  }

  passwordConfirmationValidator = (control: FormControl): { [s: string]: boolean } => {
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.validateUserPasswordEditForm.controls.password.value) {
      return { confirm: true, error: true };
    }
    return {};
  };

  ngOnInit() {
    this.store.dispatch(loadUser());

    this.user$.subscribe(user => {
      this.validateUserDetailEditForm = this.formBuilder.group({
        userName: [
          { value: user.userName, disabled: true },
          [
            Validators.pattern('[a-zA-Z0-9_-]*'),
            Validators.required
          ]
        ],
        firstName: [user.firstName],
        lastName: [user.lastName],
        email: [
          [ user.email ],
          [
            Validators.email,
            Validators.required
          ]
        ]
      });
    });    

    this.validateUserPasswordEditForm = this.formBuilder.group({
      password: [null, [Validators.required]],
      checkPassword: [null, [Validators.required, this.passwordConfirmationValidator]]
    });

  }

  onSaveUserDetail() {
    for (const i in this.validateUserDetailEditForm.controls) {
      this.validateUserDetailEditForm.controls[i].markAsDirty();
      this.validateUserDetailEditForm.controls[i].updateValueAndValidity();
    }

    this.store.dispatch(updateUser({
      updateUserRequest: {
        userDetails: {
          firstName: this.validateUserDetailEditForm.value.firstName,
          lastName: this.validateUserDetailEditForm.value.lastName,
          email: this.validateUserDetailEditForm.value.email[0]
        }        
      }
    }));
    
    // this.message.success('User details were updated', {
    //   nzDuration: 3000
    // });
  }

  onChangeUserPassword() {
    for (const i in this.validateUserDetailEditForm.controls) {
      this.validateUserDetailEditForm.controls[i].markAsDirty();
      this.validateUserDetailEditForm.controls[i].updateValueAndValidity();
    }

    this.store.dispatch(changeUserPassword({
      updateUserRequest: {
        userPassword: {
          password: this.validateUserDetailEditForm.value.password
        }
      }
    }));

    // this.message.success('User password was updated', {
    //   nzDuration: 3000
    // });
  }

  onDeleteUser() {
    this.modal.confirm({
      nzTitle: 'Do you Want to delete your user account?',
      nzContent: 'When you click the OK button, your account will be permanently deleted.',
      nzOkType: 'danger',
      nzOnOk: () => {

      }
    });
  }
}
