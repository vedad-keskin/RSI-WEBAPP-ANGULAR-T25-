import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';
import {Gender} from './student-update-or-insert-endpoint.service';

export interface StudentDeleteRequest {
  id: number; // Nullable for insert
  userDeletedId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class StudentDeleteEndpointService implements MyBaseEndpointAsync<StudentDeleteRequest, void> {
  private apiUrl = `${MyConfig.api_address}/students`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: StudentDeleteRequest) {
    return this.httpClient.delete<void>(this.apiUrl, {
      body: request
    });
  }


}
