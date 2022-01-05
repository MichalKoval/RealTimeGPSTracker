import { Injectable } from "@angular/core";
//import { PageEvent } from "@angular/material";
import { PaginationModel } from "../models/pagination.model";

@Injectable()
export class PaginationService {
    private _paginationModel: PaginationModel;

    constructor() {
        this._paginationModel = new PaginationModel();
    }

    get pageIndex(): number {
        return this._paginationModel.pageIndex;
    }

    get pageSizeOptions(): number[] {
        return this._paginationModel.availableSizesPerPage;
    }

    get pageSize(): number {
        return this._paginationModel.pageSize;
    }

    // change(pageEvent: PageEvent) {
    //     this._paginationModel.pageIndex = pageEvent.pageIndex + 1;
    //     this._paginationModel.pageSize = pageEvent.pageSize;
    //     this._paginationModel.totalItemsCount = pageEvent.length;
    // }
}
