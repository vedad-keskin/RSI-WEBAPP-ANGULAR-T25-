import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router} from '@angular/router';
import {MyAuthService} from '../services/auth-services/my-auth.service';

export class AuthGuardData {
  isAdmin?: boolean;
  isManager?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: MyAuthService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const guardData = route.data as AuthGuardData;  // Cast to AuthGuardData


    // Provjera da li je korisnik prijavljen
    /*
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/auth/login']);
      return false;
    }*/

    // Provjera prava pristupa za administratora
    if (guardData.isAdmin && !this.authService.isAdmin()) {
      this.router.navigate(['/unauthorized']);
      return false;
    }

    // Provjera prava pristupa za menadžera
    if (guardData.isManager && !this.authService.isManager()) {
      this.router.navigate(['/unauthorized']);
      return false;
    }

    return true; // Dozvoljen pristup
  }

}
