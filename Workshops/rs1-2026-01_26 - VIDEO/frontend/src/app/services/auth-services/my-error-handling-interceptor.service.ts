import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {catchError} from 'rxjs/operators';
import {Observable, throwError} from 'rxjs';
import {MySnackbarHelperService} from '../../modules/shared/snackbars/my-snackbar-helper.service';

@Injectable()
export class MyErrorHandlingInterceptor implements HttpInterceptor {
  constructor(private snackBar: MySnackbarHelperService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        // Prikaži grešku korisniku
        this.handleError(error);

        // Propustiti grešku dalje ako je potrebno
        return throwError(() => error);
      })
    );
  }

  private handleError(error: HttpErrorResponse): void {
    if (error.error instanceof ErrorEvent) {
      // Client-side greška
      this.snackBar.showMessage(`Client error: ${error.error.message}`);
    } else {
      // Server-side greška
      this.snackBar.showMessage(
        `Server error: ${error.status} - ${error.message}`,
        5000
      );
    }
  }
}
