import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyPagedRequest} from '../../helper/my-paged-request';
import {MyConfig} from '../../my-config';
import {buildHttpParams} from '../../helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyPagedList} from '../../helper/my-paged-list';

// DTO za zahtjev
export interface StudentGetAllRequest extends MyPagedRequest {
  q?: string; // Upit za pretragu (ime, prezime, broj indeksa, itd.)
}

// DTO za odgovor
export interface StudentGetAllResponse {
  id: number;
  firstName: string;
  lastName: string;
  studentNumber: string;
  citizenship?: string; // Državljanstvo
  birthMunicipality?: string; // Općina rođenja
}

@Injectable({
  providedIn: 'root',
})
export class StudentGetAllEndpointService
  implements MyBaseEndpointAsync<StudentGetAllRequest, MyPagedList<StudentGetAllResponse>> {
  private apiUrl = `${MyConfig.api_address}/students/filter`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: StudentGetAllRequest) {
    const params = buildHttpParams(request); // Pretvori DTO u query parametre
    return this.httpClient.get<MyPagedList<StudentGetAllResponse>>(`${this.apiUrl}`, {params});
  }
}
