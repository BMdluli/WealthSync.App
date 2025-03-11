import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ExpenseService {
  apiUrl = 'https://localhost:7001/api/Expense';

  constructor(private http: HttpClient) {}

  createExpense(formData: any) {
    return this.http.post(this.apiUrl, formData);
  }
}
