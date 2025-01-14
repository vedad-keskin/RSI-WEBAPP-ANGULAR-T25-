import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {MyAuthInterceptor} from './services/auth-services/my-auth-interceptor.service';
import {MyAuthService} from './services/auth-services/my-auth.service';
import {SharedModule} from './modules/shared/shared.module';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MyErrorHandlingInterceptor} from './services/auth-services/my-error-handling-interceptor.service';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatOption, MatSelect} from '@angular/material/select';
import {CustomTranslateLoader} from './services/custom-translate-loader';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule, // Potrebno za animacije
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (http: HttpClient) => new CustomTranslateLoader(http),
        deps: [HttpClient]
      }
    }),
    MatFormField,
    MatSelect,
    MatOption,
    MatLabel

  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyAuthInterceptor,
      multi: true // Ensures multiple interceptors can be used if needed
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyErrorHandlingInterceptor,
      multi: true // Dodaje ErrorHandlingInterceptor u lanac
    },
    MyAuthService,
    provideAnimationsAsync() // Ensure MyAuthService is available for the interceptor
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
