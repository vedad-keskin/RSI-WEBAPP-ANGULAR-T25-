import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface AcademicYearGetAllResponse {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class AcademicYearGetAllEndpoint implements MyBaseEndpointAsync<void, AcademicYearGetAllResponse[]> {
  private apiUrl = `${MyConfig.api_address}/academicYears/all`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync() {
    return this.httpClient.get<AcademicYearGetAllResponse[]>(`${this.apiUrl}`);
  }
}
