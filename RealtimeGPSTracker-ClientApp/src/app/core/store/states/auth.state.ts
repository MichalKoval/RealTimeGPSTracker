import * as fromStore from './state';

// Auth feature key
export const authFeatureKey: string = 'auth';

// Auth state
export interface AuthState {
    accessToken: string;
    isAuthenticated: boolean;
    isRegistrationSuccessful: boolean;
    loading: boolean;
    loaded: boolean;
    error: string[];
}

// Extending app root state with auth state
// export interface State extends fromStore.State {
//     authState: AuthState
// }

// Initial auth state with no access token in it
export const initialAuthState: AuthState = {
    accessToken: null,
    isAuthenticated: false,
    isRegistrationSuccessful: false,
    loading: false,
    loaded: false,
    error: null
}