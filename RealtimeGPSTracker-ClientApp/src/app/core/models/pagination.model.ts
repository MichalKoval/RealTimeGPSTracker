export class PaginationModel {
    // kolko prvkov na jednej stranke bude mozne zobrazit
    availableSizesPerPage: number[] = [5, 10, 25, 50];
    // defaultne nastavime zobrazenie pre najviac 5 prvkov na stranke
    pageSize: number = this.availableSizesPerPage[0];
    // defaultne bude pociatocna stranka prva
    pageIndex: number = 1;
    // zatial nevieme pocet prvkov, defalt 0
    totalItemsCount: number = 0;
}

export interface IPaginationHeader {
    totalItemsCount: number;
    pageSize: number;
    pageIndex: number;
    totalPages: number;
}

export interface IPaginationSettings {
    pageIndex: number;
    totalItemsCount: number;
    pageSize: number;
    totalPages: number;
}