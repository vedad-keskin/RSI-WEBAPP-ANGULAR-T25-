import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {buildHttpParams} from '../../helper/http-params.helper';

// Definicija interfejsa za odgovor od backend-a
export interface RegionGetAllResponse {
  id: number;
  name: string;
  countryName: string;
}

// region-get-all-request.dto.ts
export interface RegionGetAllRequest {
  countryId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class RegionGetAllEndpointService implements MyBaseEndpointAsync<RegionGetAllRequest, RegionGetAllResponse[]> {
  private apiUrl = `${MyConfig.api_address}/regions/all`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: RegionGetAllRequest) {
    const params = buildHttpParams(request);
    return this.httpClient.get<RegionGetAllResponse[]>(`${this.apiUrl}`, {params});
  }
}
