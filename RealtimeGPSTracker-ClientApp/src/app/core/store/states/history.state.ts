import * as fromStore from './state';
import { IHistoryData, TripOrderBy } from '../../models/history.model';
import { Observable, of } from 'rxjs';
import { ICoordinate } from '../../models/coordinate.model';
import * as HistoryTestData from '../data/history.test.data'
import { SortOrder } from '../../models/sort.model';

// History feature key
export const historyFeatureKey: string = 'history';

// History state
export interface HistoryState {
    historyData: IHistoryData;
    loading: boolean;
    loaded: boolean;
    error: string[];
}

// Extending app root state with history state
// export interface State extends fromStore.State {
//     historyState: HistoryState
// }

export const useTestData: boolean = false;

// Initial history state with no trips in it
export const initialHistoryState: HistoryState = {
    historyData: {
        settings: {
            pagination: {
                pageIndex: 1,
                pageSize: 5,
                totalItemsCount: 0,
                totalPages: 1
            },
            sort: {
                order: SortOrder.DESC,
                orderBy: TripOrderBy.END 
            },
            fromDate: null,
            toDate: null,
            searchInProgress: false

        },
        historyItems: (useTestData) ? HistoryTestData.historyItems : null
    },
    loading: false,
    loaded: (useTestData) ? true : false,
    error: null
}