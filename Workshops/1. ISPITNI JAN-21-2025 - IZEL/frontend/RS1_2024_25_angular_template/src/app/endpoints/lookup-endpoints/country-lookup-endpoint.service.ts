import {Injectable} from "@angular/core";
import {MyConfig} from "../../my-config";
import {HttpClient} from "@angular/common/http";
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {RegionLookupEndpointService} from './region-lookup-endpoint.service';

export interface CountryLookupResponse {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CountryLookupEndpointService implements MyBaseEndpointAsync<void, CountryLookupResponse[]> {
  private apiUrl = `${MyConfig.api_address}/countries/lookup`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync() {
    return this.httpClient.get<CountryLookupResponse[]>(this.apiUrl);
  }
}
