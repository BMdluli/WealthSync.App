import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Stock } from '../_models/stock';
import { StockOnly } from '../_models/stockOnly';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class StocksService {
  private apiUrl = `${environment.apiUrl}/Stock`;

  constructor(private http: HttpClient) {}

  getStocks() {
    return this.http.get<Stock[]>(this.apiUrl);
  }

  getStockPrices() {
    return this.http.get<StockOnly[]>(`${this.apiUrl}/prices`);
  }

  addStock(userData: any) {
    return this.http.post(this.apiUrl, userData);
  }

  deleteStock(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
