import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../helper/my-base-endpoint-async.interface';
import {buildHttpParams} from '../../helper/http-params.helper';

export interface ChatUserGetRequest {
  userType?: string; // "professor", "student", null za sve korisnike
  searchTerm?: string; // Pretraga po imenu, prezimenu ili emailu
}

export interface ChatUserGetResponse {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  type: string; // "Professor", "Student", itd.
}

@Injectable({
  providedIn: 'root',
})
export class ChatUserGetEndpointService implements MyBaseEndpointAsync<ChatUserGetRequest, ChatUserGetResponse[]> {
  private apiUrl = `${MyConfig.api_address}/chat-users`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: ChatUserGetRequest) {
    // Koristimo buildHttpParams helper za generisanje parametara
    const params = buildHttpParams(request);

    // Slanje GET zahtjeva sa generisanim parametrima
    return this.httpClient.get<ChatUserGetResponse[]>(this.apiUrl, { params });
  }
}
