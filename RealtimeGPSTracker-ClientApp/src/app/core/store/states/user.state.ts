import { IUser } from 'src/app/core/models/user.model';
import * as fromStore from './state';
import { first } from 'rxjs/operators';

// User feature key
export const userFeatureKey: string = 'user';

// User state
export interface UserState {
    user: IUser;
    loading: boolean;
    loaded: boolean;
    error: string[] | null;
}

// Extending app root state with user state
// export interface State extends fromStore.State {
//     userState: UserState
// }

// Initial user state with no user in it
export const initialUserState: UserState = {
    user: {
        id: '',
        userName: 'UserName',
        firstName: 'Name',
        lastName: 'Surname',
        email: 'user@mail.com'
    },
    loading: false,
    loaded: true,
    error: null
}