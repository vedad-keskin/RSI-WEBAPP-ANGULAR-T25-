import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyPagedRequest} from '../../helper/my-paged-request';
import {buildHttpParams} from '../../helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface CityGetAll2Response {
  id: number;
  name: string;
  countryName: string;
}

@Injectable({
  providedIn: 'root'
})
export class CityGetAll2EndpointService implements MyBaseEndpointAsync<MyPagedRequest, CityGetAll2Response[]> {
  private apiUrl = `${MyConfig.api_address}/paged`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: MyPagedRequest) {
    const params = buildHttpParams(request);  // Use the helper function here

    return this.httpClient.get<CityGetAll2Response[]>(`${this.apiUrl}`, {params});
  }
}
