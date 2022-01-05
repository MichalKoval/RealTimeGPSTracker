import * as fromStore from './state';
import { IRealtimeData } from 'src/app/core/models/realtime.model';

// Realtime feature key
export const realtimeFeatureKey: string = 'realtime';

// Realtime state
export interface RealtimeState {
    realtimeData: IRealtimeData;
    loading: boolean;
    loaded: boolean;
    error: string[];
}

// Extending app root state with logout state
// export interface State extends fromStore.State {
//     realtimeState: RealtimeState
// }

// Initial logout state with no access token in it
export const initialRealtimeState: RealtimeState = {
    realtimeData: null,
    loading: false,
    loaded: false,
    error: null
}