import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Budget } from '../_models/budget';

@Injectable({
  providedIn: 'root',
})
export class BudgetService {
  private apiUrl = 'https://localhost:7001/api/Budget';

  constructor(private http: HttpClient) {}

  getbudgetItems() {
    return this.http.get<Budget[]>(this.apiUrl);
  }

  createBudget(formData: any) {
    return this.http.post(this.apiUrl, formData);
  }
}
