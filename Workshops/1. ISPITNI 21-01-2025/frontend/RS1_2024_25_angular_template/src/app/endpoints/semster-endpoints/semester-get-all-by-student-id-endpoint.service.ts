import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';

export interface SemesterGetAllByStudentIdResponse {
  id: number;
  academicYearDescription:string;
  yearOfStudy:number;
  renewal:boolean;
  dateOfEnrollment:Date;
  recordedByName:string;
}

@Injectable({
  providedIn: 'root'
})
export class SemesterGetAllByStudentIdEndpoint
  implements MyBaseEndpointAsync<number, SemesterGetAllByStudentIdResponse> {
  private apiUrl = `${MyConfig.api_address}/semesters`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(id: number) {
    return this.httpClient.get<SemesterGetAllByStudentIdResponse>(`${this.apiUrl}/${id}`);
  }
}
