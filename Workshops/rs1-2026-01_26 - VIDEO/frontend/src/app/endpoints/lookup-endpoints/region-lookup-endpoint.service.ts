import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {buildHttpParams} from '../../helper/http-params.helper';

// Definicija interfejsa za odgovor od backend-a
export interface RegionLookupResponse {
  id: number;
  name: string;
  countryName: string;
  countryId: number;
}

// region-get-all-request.dto.ts
export interface RegionLookupRequest {
  countryId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class RegionLookupEndpointService implements MyBaseEndpointAsync<RegionLookupRequest, RegionLookupResponse[]> {
  private apiUrl = `${MyConfig.api_address}/regions/lookup`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: RegionLookupRequest) {
    const params = buildHttpParams(request);
    return this.httpClient.get<RegionLookupResponse[]>(`${this.apiUrl}`, {params});
  }
}
