import { Component } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { FormsModule } from '@angular/forms';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent {
  email: string = '';
  message: string = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  onForgotPassword() {
    this.loading = true;
    this.authService.forgotPassword({ email: this.email }).subscribe({
      next: () => {
        this.toastr.success('Reset link sent to your email!');
      },
      error: (err) => {
        this.loading = false;
      },
      complete: () => (this.loading = false),
    });
  }
}
