import { IRealtimeData, IRealtimeDataRequest } from 'src/app/core/models/realtime.model';
import { createAction, props } from '@ngrx/store';

export enum RealtimeActionTypes {
    LOAD_REALTIME = "[Realtime] Load Realtime",
    LOAD_REALTIME_SUCCESS = "[Realtime] Load Realtime Success",
    LOAD_REALTIME_FAIL = "[Realtime] Load Realtime Fail"
}

export const loadRealtime = createAction(
    RealtimeActionTypes.LOAD_REALTIME,
    props<{ payload: IRealtimeDataRequest }>()
);

export const loadRealtimeSuccess = createAction(
    RealtimeActionTypes.LOAD_REALTIME_SUCCESS,
    props<{ payload: IRealtimeData }>()
);

export const loadRealtimeFail = createAction(
    RealtimeActionTypes.LOAD_REALTIME_FAIL,
    props<{ errors: string[] }>()
);