import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {ForgetPasswordComponent} from './forget-password/forget-password.component';
import {TwoFactorComponent} from './two-factor/two-factor.component';
import {AdminErrorPageComponent} from '../admin/admin-error-page/admin-error-page.component';
import {AuthLayoutComponent} from './auth-layout/auth-layout.component';
import {LogoutComponent} from './logout/logout.component';

const routes: Routes = [
  {
    path: '', component: AuthLayoutComponent, children: [
      {path: '', redirectTo: 'login', pathMatch: 'full'},
      {path: 'login', component: LoginComponent},
      {path: 'logout', component: LogoutComponent},
      {path: 'forget-password', component: ForgetPasswordComponent},
      {path: 'two-factor', component: TwoFactorComponent},
      {path: '**', component: AdminErrorPageComponent}  // Default ruta koja vodi na public
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule {
}
