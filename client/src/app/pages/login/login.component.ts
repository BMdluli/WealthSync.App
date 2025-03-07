import { Component } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  user = {
    email: '1@2.com',
    password: 'Password123$$',
  };

  constructor(private authService: AuthService, private router: Router) {}

  submitForm(form: any) {
    if (form.valid) {
      this.authService.loginUser(this.user).subscribe(
        (response) => {
          this.saveToken(response.token);
          this.router.navigate(['dashboard']);
        },
        (error) => {
          console.error(error);
        }
      );
    }
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }
}
