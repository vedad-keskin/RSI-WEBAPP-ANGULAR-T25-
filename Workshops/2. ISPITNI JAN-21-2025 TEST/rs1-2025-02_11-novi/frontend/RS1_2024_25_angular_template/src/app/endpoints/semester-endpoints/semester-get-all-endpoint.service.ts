import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';

export interface SemestersGetAllResponse {
  id: number;
  academicYearDescription: string;
  recordedByName: string;
  yearOfStudy: number;
  renewal: boolean;
  date: Date;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class SemesterGetAllEndpoint
  implements MyBaseEndpointAsync<number, SemestersGetAllResponse> {
  private apiUrl = `${MyConfig.api_address}/semesters`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(id: number) {
    return this.httpClient.get<SemestersGetAllResponse>(`${this.apiUrl}/${id}`);
  }
}
