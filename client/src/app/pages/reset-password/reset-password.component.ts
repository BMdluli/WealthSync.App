import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../_services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { SpinnerComponent } from '../../spinner/spinner.component';

@Component({
  selector: 'app-reset-password',
  imports: [FormsModule, SpinnerComponent, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss',
})
export class ResetPasswordComponent {
  email: string = '';
  token: string = '';
  newPassword: string = '';
  isSuccess: boolean = false;
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.email = params['email'] || '';
      this.token = params['token'] || '';
      if (!this.email || !this.token) {
        this.toastr.error('Invalid reset link');
      }
    });
  }

  onSubmit() {
    this.loading = true;
    let data = {
      email: this.email,
      token: this.token,
      newPassword: this.newPassword,
    };
    this.authService.resetPassword(data).subscribe({
      next: () => {
        this.toastr.success('Password reset successfully');
        this.isSuccess = true;
      },
      error: (err) => {
        this.loading = false;
      },
      complete: () => (this.loading = false),
    });
  }
}
