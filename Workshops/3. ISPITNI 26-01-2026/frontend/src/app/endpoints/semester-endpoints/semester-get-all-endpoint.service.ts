import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyPagedRequest } from '../../helper/my-paged-request';
import { MyConfig } from '../../my-config';
import { buildHttpParams } from '../../helper/http-params.helper';
import { MyPagedList } from '../../helper/my-paged-list';

export interface SemesterGetAllRequest extends MyPagedRequest {
  q?: string;
  status?: string;
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
export class SemesterGetAllEndpointService {
  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(studentId: number, request: SemesterGetAllRequest) {
    const params = buildHttpParams(request);
    return this.httpClient.get<MyPagedList<SemesterGetAllResponse>>(
      `${this.apiUrl}/${studentId}/semesters/filter`,
      { params }
    );
  }
}
