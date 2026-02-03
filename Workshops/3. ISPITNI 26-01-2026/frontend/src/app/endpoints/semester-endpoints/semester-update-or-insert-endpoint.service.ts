import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';

export interface SemesterUpdateOrInsertRequest {
  id?: number; // optional for insert
  academicYearId: number;
  studyYear: number;
  enrollmentDate: Date; // ISO date string
}

@Injectable({
  providedIn: 'root',
})
export class SemesterUpdateOrInsertEndpointService
  implements MyBaseEndpointAsync<{ studentId: number; body: SemesterUpdateOrInsertRequest }, number> {
  private apiUrl = `${MyConfig.api_address}/students/{studentId}/semesters`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: { studentId: number; body: SemesterUpdateOrInsertRequest }) {
    return this.httpClient.post<number>(this.apiUrl, request);
  }
}