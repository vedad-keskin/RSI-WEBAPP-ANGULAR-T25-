import {Injectable} from "@angular/core";
import {MyConfig} from "../../my-config";
import {HttpClient} from "@angular/common/http";
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface CountryGetAllResponse {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CountryGetAllEndpointService implements MyBaseEndpointAsync<void, CountryGetAllResponse[]> {
  private apiUrl = `${MyConfig.api_address}/countries/all`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync() {
    return this.httpClient.get<CountryGetAllResponse[]>(this.apiUrl);
  }
}
