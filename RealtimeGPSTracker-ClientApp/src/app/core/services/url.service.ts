import { Injectable } from '@angular/core';
import { QueryBaseUrl, QueryAddress, HubAddress } from '../models/url.model';

@Injectable()
export class UrlQueryService {

  constructor(
    public queryUrl: QueryBaseUrl,
    public queryAddress: QueryAddress,
    public hubAddress: HubAddress
  ) { }
}
