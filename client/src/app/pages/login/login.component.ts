import { Component } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SpinnerComponent } from '../../spinner/spinner.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  user = {
    email: '1@2.com',
    password: 'Password123$$',
  };
  loading = false;
  constructor(
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  submitForm(form: any) {
    this.loading = true;
    if (form.valid) {
      this.authService.loginUser(this.user).subscribe({
        next: (response) => {
          this.authService.setToken(response.token);
          this.toastr.success('Login Successful');
          this.router.navigate(['dashboard']);
        },
        error: (error) => {
          console.error(error);
          this.loading = false;
        },
        complete: () => (this.loading = false),
      });
    } else {
      this.loading = false;
    }
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }
}
