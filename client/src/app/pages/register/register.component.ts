import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../_services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
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

  constructor(
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  submitForm(form: any) {
    if (form.valid) {
      this.authService.registerUser(this.user).subscribe(
        (response) => {
          console.log(response);
          this.toastr.success('User created successfully');
        },
        (error) => {
          console.error(error);
        }
      );
    }
  }
}
