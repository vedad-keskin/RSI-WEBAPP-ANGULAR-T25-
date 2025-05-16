import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';

export interface StudentGetByIdResponse {
  id: number;
  firstName: string;
  lastName: string;
  studentNumber: string;
  parentName?: string;
  birthDate?: string; // ISO format string za datum
  gender: string;
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
export class StudentGetByIdEndpointService
  implements MyBaseEndpointAsync<number, StudentGetByIdResponse> {
  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(id: number) {
    return this.httpClient.get<StudentGetByIdResponse>(`${this.apiUrl}/${id}`);
  }
}
