import {Injectable} from "@angular/core";
import {HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {MyAuthService} from "./my-auth.service";
import {MyPageProgressbarService} from '../../modules/shared/progressbars/my-page-progressbar.service';
import {finalize} from 'rxjs';

@Injectable()
export class MyAuthInterceptor implements HttpInterceptor {

  constructor(private auth: MyAuthService,
              private progressBarService: MyPageProgressbarService
  ) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    // Get the auth token from the service.
    const authToken = this.auth.getLoginToken()?.token ?? "";
    this.progressBarService.show();
    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    const authReq = req.clone({
      headers: req.headers.set('my-auth-token', authToken)
    });

    // send cloned request with header to the next handler.
    return next.handle(authReq).pipe(
      finalize(() => {
        this.progressBarService.hide();
      })
    );
  }
}
