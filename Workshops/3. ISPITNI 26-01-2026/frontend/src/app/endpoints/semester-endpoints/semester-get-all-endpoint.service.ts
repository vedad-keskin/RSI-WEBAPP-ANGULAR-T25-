import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyPagedRequest} from '../../helper/my-paged-request';
import {MyConfig} from '../../my-config';
import {buildHttpParams} from '../../helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyPagedList} from '../../helper/my-paged-list';

// DTO za zahtjev
export interface SemesterGetAllRequest extends MyPagedRequest {
  q?: string; // ak g
  status?: string; // all deleted active
}

// DTO za odgovor
export interface SemesterGetAllResponse {
  id: number;
  academicYearDescription: string;
  studyYear: number;
  enrollmentDate: Date;
  tuitionFee: number; // Državljanstvo
  isRenewal: boolean; // Općina rođenja
  isDeleted: boolean; // Općina rođenja
}

@Injectable({
  providedIn: 'root',
})
export class SemesterGetAllEndpointService {
  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync( studentId:number, request: SemesterGetAllRequest) {
    const params = buildHttpParams(request); // Pretvori DTO u query parametre


    return this.httpClient.get<MyPagedList<SemesterGetAllResponse>>(`${this.apiUrl}/${studentId}/semesters/filter`, {params});
  }
}
