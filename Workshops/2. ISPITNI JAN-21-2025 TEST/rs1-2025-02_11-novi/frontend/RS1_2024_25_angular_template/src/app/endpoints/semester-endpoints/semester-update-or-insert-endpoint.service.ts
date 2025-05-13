import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';


export interface SemesterUpdateOrInsertRequest {
  id?: number; // Nullable for insert
  studentId: number;
  academicYearId: number;
  recordedById?:number;
  yearOfStudy:number;
  renewal:boolean;
  date:Date;
  price:number;
}

@Injectable({
  providedIn: 'root'
})
export class SemesterUpdateOrInsertEndpoint
  implements MyBaseEndpointAsync<SemesterUpdateOrInsertRequest, number> {
  private apiUrl = `${MyConfig.api_address}/semesters`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: SemesterUpdateOrInsertRequest) {
    return this.httpClient.post<number>(this.apiUrl, request);
  }
}
