export class SortModel {
    orderDirection: string = "";
    orderBy: string = "";
}


export enum SortOrder {
    ASC = "Asc",
    DESC = "Desc",
}

export interface ISortSettings {
    order: SortOrder;
}