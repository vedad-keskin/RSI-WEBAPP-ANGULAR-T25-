import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyAuthInfo } from './dto/my-auth-info';
import { LoginTokenDto } from './dto/login-token-dto';
import {MySignalRService} from '../signalr-services/my-signal-r.service';

@Injectable({ providedIn: 'root' })
export class MyAuthService {
  constructor(private httpClient: HttpClient, private signalRService: MySignalRService) {}

  getMyAuthInfo(): MyAuthInfo | null {
    return this.getLoginToken()?.myAuthInfo ?? null;
  }

  isLoggedIn(): boolean {
    return this.getMyAuthInfo() != null && this.getMyAuthInfo()!.isLoggedIn;
  }

  isAdmin(): boolean {
    return this.getMyAuthInfo()?.isAdmin ?? false;
  }

  isManager(): boolean {
    return this.getMyAuthInfo()?.isManager ?? false;
  }

  setLoggedInUser(x: LoginTokenDto | null) {
    if (x == null) {
      window.localStorage.setItem('my-auth-token', '');
      this.signalRService.stopConnection(); // Zaustavljanje SignalR konekcije nakon odjave
    } else {
      window.localStorage.setItem('my-auth-token', JSON.stringify(x));
      this.signalRService.startConnection(); // Pokretanje SignalR konekcije nakon prijave
    }
  }

  getLoginToken(): LoginTokenDto | null {
    let tokenString = window.localStorage.getItem('my-auth-token') ?? '';
    try {
      return JSON.parse(tokenString);
    } catch (e) {
      return null;
    }
  }
}
