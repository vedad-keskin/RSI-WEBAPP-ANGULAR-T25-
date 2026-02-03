import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';

export interface SemesterUpdateOrInsertRequest {
  id?: number;
  academicYearId: number;
  studyYear: number;
  enrollmentDate: string;
}

@Injectable({
  providedIn: 'root',
})
export class SemesterUpdateOrInsertEndpointService {
  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(studentId: number, request: SemesterUpdateOrInsertRequest) {
    return this.httpClient.post<number>(`${this.apiUrl}/${studentId}/semesters`, request);
  }
}
