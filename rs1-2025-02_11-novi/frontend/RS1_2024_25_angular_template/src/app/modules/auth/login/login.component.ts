import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {AuthLoginEndpointService} from '../../../endpoints/auth-endpoints/auth-login-endpoint.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MyInputTextType} from '../../shared/my-reactive-forms/my-input-text/my-input-text.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false,
})
export class LoginComponent {
  form: FormGroup;
  protected readonly MyInputTextType = MyInputTextType;

  constructor(private fb: FormBuilder, private authLoginService: AuthLoginEndpointService, private router: Router) {

    this.form = this.fb.group({
      email: ['admin', [Validators.required, Validators.min(2), Validators.max(15)]],
      password: ['test', [Validators.required, Validators.min(2), Validators.max(30)]],
    });
  }

  onLogin(): void {
    if (this.form.invalid) return;

    this.authLoginService.handleAsync(this.form.value).subscribe({
      next: () => {
        console.log('Login successful');
        this.router.navigate(['/admin']); // Redirect to admin panel
      },
    });
  }
}
