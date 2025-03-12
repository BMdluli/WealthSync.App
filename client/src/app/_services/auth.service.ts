import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from '../_models/login';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenKey = 'auth_token';
  private apiUrl = 'https://localhost:7001/api/Auth';

  constructor(private http: HttpClient) {}

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;

    return !this.isTokenExpired(token);
  }

  private isTokenExpired(token: string): boolean {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expiry = payload.exp;
    return expiry * 1000 < Date.now(); // Expiry is in seconds, convert to milliseconds
  }

  registerUser(userData: any) {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  loginUser(userData: any) {
    return this.http.post<Login>(`${this.apiUrl}/login`, userData);
  }
}
