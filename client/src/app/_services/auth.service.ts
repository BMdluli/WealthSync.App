import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from '../_models/login';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7001/api/Auth';

  constructor(private http: HttpClient) {}

  registerUser(userData: any) {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  loginUser(userData: any) {
    return this.http.post<Login>(`${this.apiUrl}/login`, userData);
  }
}
