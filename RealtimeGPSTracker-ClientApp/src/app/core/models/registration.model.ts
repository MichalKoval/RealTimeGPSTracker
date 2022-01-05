import { IApiRequest } from 'src/app/core/models/request.model';
import { IApiResponse } from 'src/app/core/models/response.model';

export interface IRegistrationRequest extends IApiRequest {
    firstName: string;
	lastName: string;
	userName: string;
	email: string;
	password: string;
}

export interface IRegistrationResponse extends IApiResponse {
	
}