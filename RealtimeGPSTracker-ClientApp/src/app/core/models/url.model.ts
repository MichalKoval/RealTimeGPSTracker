export class QueryBaseUrl {
    constructor(public value: string) {}
}

export class QueryAddress {
    constructor(public value: string) {}
}

export class HubAddress {
    constructor(public value: string) {}
}

export interface IQueryProperty {
    name: string;
    value: string;
}

export interface INavigationLinks {
    href: string;
    rel: string;
    method: string;
}