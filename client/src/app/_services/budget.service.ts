import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Budget, Count } from '../_models/budget';

@Injectable({
  providedIn: 'root',
})
export class BudgetService {
  private apiUrl = 'https://localhost:7001/api/Budget';
  // https://localhost:7001/api/Budget/GetBudgetCount

  constructor(private http: HttpClient) {}

  getbudgetItems() {
    return this.http.get<Budget[]>(this.apiUrl);
  }

  getbudgetItemsLimit() {
    return this.http.get<Budget[]>(`${this.apiUrl}?limit=6`);
  }

  getbudgetCount() {
    return this.http.get<Count>(`${this.apiUrl}/GetBudgetCount`);
  }

  createBudget(formData: any) {
    return this.http.post(this.apiUrl, formData);
  }

  updateBudget(id: any, formData: any) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }

  deleteBudget(id: any) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
