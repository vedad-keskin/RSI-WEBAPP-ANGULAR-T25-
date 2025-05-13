import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';

export enum Gender {
  Male = 1,
  Female = 2,
  Other = 3
}

export interface StudentUpdateOrInsertRequest {
  id?: number; // Nullable for insert
  firstName: string;
  lastName: string;
  studentNumber: string;
  parentName?: string;
  birthDate?: string; // ISO format string (e.g., "2000-05-15")
  gender: Gender;
  citizenshipId?: number;
  birthPlace?: string;
  birthMunicipalityId?: number;
  permanentAddressStreet?: string;
  permanentMunicipalityId?: number;
  contactMobilePhone?: string;
  contactPrivateEmail?: string;
}

@Injectable({
  providedIn: 'root'
})
export class StudentUpdateOrInsertEndpointService
  implements MyBaseEndpointAsync<StudentUpdateOrInsertRequest, number> {
  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: StudentUpdateOrInsertRequest) {
    return this.httpClient.post<number>(this.apiUrl, request);
  }
}
