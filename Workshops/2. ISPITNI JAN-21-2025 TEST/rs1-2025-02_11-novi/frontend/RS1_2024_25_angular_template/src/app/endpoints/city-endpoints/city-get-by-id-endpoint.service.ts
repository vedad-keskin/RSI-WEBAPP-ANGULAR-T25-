import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface CityGetByIdResponse {
  name: string;
  countryId: number;
  regionId: number;
}

@Injectable({
  providedIn: 'root'
})
export class CityGetByIdEndpointService implements MyBaseEndpointAsync<number, CityGetByIdResponse> {
  private apiUrl = `${MyConfig.api_address}/cities`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    return this.httpClient.get<CityGetByIdResponse>(`${this.apiUrl}/${id}`);
  }
}
