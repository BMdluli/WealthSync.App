import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BudgetCategory } from '../_models/budgetCategory';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BudgetCategoryService {
  apiUrl = `${environment.apiUrl}/BudgetCategory`;

  constructor(private http: HttpClient) {}

  getBudgetCategories(id: string) {
    return this.http.get<BudgetCategory[]>(`${this.apiUrl}/Budget/${id}`);
  }

  createBudgetCategory(formData: any) {
    return this.http.post<BudgetCategory>(this.apiUrl, formData);
  }

  deleteBudgetCategory(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  editBudgetCategory(id: number, formData: any) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }
}
