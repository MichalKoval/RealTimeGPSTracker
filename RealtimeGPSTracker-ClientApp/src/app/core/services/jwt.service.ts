import { Injectable, Inject } from '@angular/core';
import * as jwt_devode from 'jwt-decode';
import { CanActivate } from '@angular/router';

@Injectable()
export class JwtService {
    // Query adresa na server pre 'Coordinate' --> '/coordinate'
        
    constructor() {
    }

    decodeToken(token: string) {
        return jwt_devode(token);
    }    
}