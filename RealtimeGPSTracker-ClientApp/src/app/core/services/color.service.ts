import * as moment from 'moment';

export function randomHexColor(): string {
    return `#${Math.floor(Math.random()*16777215).toString(16)}`;
}

export function checkHexColor(strColor: string): boolean {
    return /(^#[0-9a-fA-F]{6}$)|(^#[0-9a-fA-F]{3}$)/i.test(strColor);
}