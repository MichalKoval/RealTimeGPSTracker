import { SortModel } from "../models/sort.model";
import { Injectable } from "@angular/core";
//import { MatSort } from "@angular/material";

@Injectable()
export class SortService {
    private _sortModel: SortModel;

    private firstLetterUpperCase(str: string) {
        return (str.length > 0) ? str.charAt(0).toUpperCase() + str.slice(1) : str;
    }

    constructor() {
        this._sortModel = new SortModel();
    }

    get orderDirection() : string {
        return this.firstLetterUpperCase(this._sortModel.orderDirection);
    }
    get orderBy() : string {
        return this._sortModel.orderBy;
    }

    // change(sortEvent: MatSort) {
    //     // Smer zoradenia (property Sort.direction)
    //     this._sortModel.orderDirection = sortEvent.direction;
    //     // Zoradit podla nazbu stlpca (property Sort.active)
    //     this._sortModel.orderBy = sortEvent.active;
    // }
}