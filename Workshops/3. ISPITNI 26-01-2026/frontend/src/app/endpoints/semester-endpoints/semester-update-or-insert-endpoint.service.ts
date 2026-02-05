import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';



export interface SemesterUpdateOrInsertRequest {
  id?: number; // Nullable for insert
  academicYearId?: number; // Nullable for insert
  studyYear: number;
  enrollmentDate: Date;

}

@Injectable({
  providedIn: 'root'
})
export class SemesterUpdateOrInsertEndpointService {

  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(studentId:number, request: SemesterUpdateOrInsertRequest) {
    return this.httpClient.post<number>(`${this.apiUrl}/${studentId}/semesters`, request);
  }
}
