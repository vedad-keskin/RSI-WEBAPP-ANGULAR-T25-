import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { Observable } from 'rxjs';

export interface CityLookupRequest {
  regionId?: number; // Opcionalni filter za RegionId
  countryId?: number; // Opcionalni filter za CountryId
}

export interface CityLookupResponse {
  id: number;
  name: string;
  regionID: number;
  regionName: string;
  countryID: number;
  countryName: string;
}

@Injectable({
  providedIn: 'root'
})
export class CityLookupEndpointService {
  private apiUrl = `${MyConfig.api_address}/cities/lookup`;

  constructor(private httpClient: HttpClient) {}

  /**
   * Dohvaća listu gradova na osnovu filtera.
   * @param request Filter parametri
   * @returns Observable sa listom gradova
   */
  handleAsync(request: CityLookupRequest): Observable<CityLookupResponse[]> {
    return this.httpClient.get<CityLookupResponse[]>(this.apiUrl, { params: request as any });
  }
}
