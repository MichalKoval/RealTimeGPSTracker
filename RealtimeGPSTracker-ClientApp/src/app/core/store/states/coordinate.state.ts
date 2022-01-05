import * as fromStore from './state';
import { ICoordinatesData } from 'src/app/core/models/coordinate.model';

// Coordinate feature key
export const coordinateFeatureKey: string = 'coordinate';

// Coordinate state
export interface CoordinateState {
    coordinatesData: ICoordinatesData;
    loading: boolean;
    loaded: boolean;
    error: string[];
}

// Extending app root state with coordinate state
// export interface State extends fromStore.State {
//     coordinateState: CoordinateState
// }

// Initial coordinate state with no coordinates data in it
export const initialCoordinateState: CoordinateState = {
    coordinatesData: null,
    loading: false,
    loaded: false,
    error: null,
}