import {Injectable} from "@angular/core";
import {MyConfig} from "../../my-config";
import {HttpClient} from "@angular/common/http";
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface AcademicYearLookupResponse {
  id: number;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class AcademicYearLookupEndpointService implements MyBaseEndpointAsync<void, AcademicYearLookupResponse[]> {
  private apiUrl = `${MyConfig.api_address}/academic-years/lookup`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync() {
    return this.httpClient.get<AcademicYearLookupResponse[]>(this.apiUrl);
  }
}
