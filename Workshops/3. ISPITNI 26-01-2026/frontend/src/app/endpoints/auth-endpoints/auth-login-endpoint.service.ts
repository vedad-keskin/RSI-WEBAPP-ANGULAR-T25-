import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {tap} from 'rxjs/operators';
import {MyConfig} from '../../my-config';
import {MyAuthService} from '../../services/auth-services/my-auth.service';
import {LoginTokenDto} from '../../services/auth-services/dto/login-token-dto';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface LoginRequest {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthLoginEndpointService implements MyBaseEndpointAsync<LoginRequest, LoginTokenDto> {
  private apiUrl = `${MyConfig.api_address}/auth/login`;

  constructor(private httpClient: HttpClient, private myAuthService: MyAuthService) {
  }

  handleAsync(request: LoginRequest) {
    return this.httpClient.post<LoginTokenDto>(`${this.apiUrl}`, request).pipe(
      tap((response) => {
        // Use MyAuthService to store login token and auth info
        this.myAuthService.setLoggedInUser({
          token: response.token,
          myAuthInfo: response.myAuthInfo
        });
      })
    );
  }
}
