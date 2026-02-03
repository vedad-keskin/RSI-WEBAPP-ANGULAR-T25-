import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyPagedRequest } from '../../helper/my-paged-request';
import { MyConfig } from '../../my-config';
import { buildHttpParams } from '../../helper/http-params.helper';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';
import { MyPagedList } from '../../helper/my-paged-list';

export interface SemesterGetAllRequest extends MyPagedRequest {
  studentId: number; // from route: students/{studentId}/semesters/filter
  q?: string;
  status?: string; // 'active' | 'deleted' | 'all'
}

export interface SemesterGetAllResponse {
  id: number;
  academicYearDescription: string;
  studyYear: number;
  enrollmentDate: string;
  isRenewal: boolean;
  tuitionFee: number;
  isDeleted: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class SemesterGetAllEndpointService
  implements MyBaseEndpointAsync<SemesterGetAllRequest, MyPagedList<SemesterGetAllResponse>> {
    private apiUrl = `${MyConfig.api_address}/students/{studentId}/semesters/filter`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: SemesterGetAllRequest) {
    const params = buildHttpParams(request); // Pretvori DTO u query parametre
    return this.httpClient.get<MyPagedList<SemesterGetAllResponse>>(`${this.apiUrl}`, {params});
  }
}