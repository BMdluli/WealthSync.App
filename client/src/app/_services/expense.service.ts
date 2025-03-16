import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ExpenseService {
  apiUrl = `${environment.apiUrl}/Expense`;

  constructor(private http: HttpClient) {}

  createExpense(formData: any) {
    return this.http.post(this.apiUrl, formData);
  }
}
