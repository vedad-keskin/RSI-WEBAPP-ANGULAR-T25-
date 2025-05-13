import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';

export interface SemesterGetByStudentIdResponse {
  id: number;
  academicYearDescription:string;
  yearOfStudy:number;
  renewal:boolean;
  date:Date;
  recordedByName:string;
}

@Injectable({
  providedIn: 'root'
})
export class SemesterGetByStudentIdEndpoint
  implements MyBaseEndpointAsync<number, SemesterGetByStudentIdResponse> {
  private apiUrl = `${MyConfig.api_address}/semesters`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(id: number) {
    return this.httpClient.get<SemesterGetByStudentIdResponse>(`${this.apiUrl}/${id}`);
  }
}
