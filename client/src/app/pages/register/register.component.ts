import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../_services/auth.service';

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

  constructor(private authService: AuthService) {}

  submitForm(form: any) {
    if (form.valid) {
      this.authService.registerUser(this.user).subscribe(
        (response) => {
          console.log(response);
        },
        (error) => {
          console.error(error);
        }
      );
    }
  }
}
