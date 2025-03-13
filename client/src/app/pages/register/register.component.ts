import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../_services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  user = {
    name: '',
    username: '',
    email: '',
    password: '',
  };
  loading = false;

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  submitForm(form: any) {
    this.loading = true; // Set loading to true at the start
    if (form.valid) {
      this.authService.registerUser(this.user).subscribe({
        next: () => {
          this.toastr.success('User created successfully');
          this.router.navigate(['login']);
        },
        error: (error) => {
          // console.error(error);
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        },
      });
    } else {
      this.loading = false;
    }
  }
}
