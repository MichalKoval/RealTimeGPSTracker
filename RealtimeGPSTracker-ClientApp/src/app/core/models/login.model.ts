import { IApiRequest } from 'src/app/core/models/request.model';
import { IApiResponse } from 'src/app/core/models/response.model';

export interface ILoginRequest extends IApiRequest {
    userName: string;
    password: string;
}

export interface ILoginResponse extends IApiResponse {
    accessToken: string;
}