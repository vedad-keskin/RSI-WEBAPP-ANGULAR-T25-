import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { Observable } from 'rxjs';

export interface MunicipalityLookupRequest {
  countryId?: number; // Opcionalni filter za CountryId
  regionId?: number;  // Opcionalni filter za RegionId
  cityId?: number;    // Opcionalni filter za CityId
}

export interface MunicipalityLookupResponse {
  id: number;
  name: string;
  cityID: number;
  cityName: string;
  regionID: number;
  regionName: string;
  countryID: number;
  countryName: string;
}

@Injectable({
  providedIn: 'root'
})
export class MunicipalityLookupEndpointService {
  private apiUrl = `${MyConfig.api_address}/municipalities/lookup`;

  constructor(private httpClient: HttpClient) {}

  /**
   * Dohvaća listu opština na osnovu filtera.
   * @param request Filter parametri
   * @returns Observable sa listom opština
   */
  handleAsync(request: MunicipalityLookupRequest): Observable<MunicipalityLookupResponse[]> {
    return this.httpClient.get<MunicipalityLookupResponse[]>(this.apiUrl, { params: request as any });
  }
}
