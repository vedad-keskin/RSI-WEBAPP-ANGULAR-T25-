import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface SemesterGetAllResponse {
  id: number;
  academicYearId: number;
  academicYear: AcademicYear;
  recordedById: number;
  recordedBy: RecordedBy;
  yearOfStudy: number;
  renewal: boolean;
  date: Date;
  price: number;
}

export interface AcademicYear {
  id: number;
  description: string;
}

export interface RecordedBy {
  id: number;
  email: string;
}



@Injectable({
  providedIn: 'root'
})
export class SemesterGetAllEndpoint implements MyBaseEndpointAsync<number, SemesterGetAllResponse[]> {
  private apiUrl = `${MyConfig.api_address}/semesters`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id:number) {
    return this.httpClient.get<SemesterGetAllResponse[]>(`${this.apiUrl}/${id}`);
  }
}
