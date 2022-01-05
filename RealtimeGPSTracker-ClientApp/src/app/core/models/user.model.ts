import { IUpdateMessage } from './signalr.model';
import { IApiRequest } from './request.model';
import { IApiResponse } from './response.model';

export interface IUser {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
}

export interface IUpdateUserRequest extends IApiRequest {
    userDetails?: {
        firstName?: string;
        lastName?: string;
        email: string;
    };
    userPassword?: {
        password: string;
    }
    
}

export interface IUpdateUserResponse extends IApiResponse {

}

export interface IChangeUserPasswordResponse extends IApiResponse {

}

export interface IDeleteUserRequest extends IApiRequest {

}

export interface IDeleteUserResponse extends IApiResponse {

}

export interface IUpdateUserDetailMessage extends IUpdateMessage {

}