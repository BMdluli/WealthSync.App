import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Stock } from '../_models/stock';

@Injectable({
  providedIn: 'root',
})
export class StocksService {
  private apiUrl = 'https://localhost:7001/api/Stock';

  constructor(private http: HttpClient) {}

  // registerUser(userData: any) {
  //   return this.http.post(`${this.apiUrl}/register`, userData);
  // }

  // loginUser(userData: any) {
  //   return this.http.post<Login>(`${this.apiUrl}/login`, userData);
  // }

  getStocks() {
    return this.http.get<Stock[]>(this.apiUrl);
  }

  addStock(userData: any) {
    return this.http.post(this.apiUrl, userData);
  }
}
