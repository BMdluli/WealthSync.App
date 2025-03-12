import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Goal } from '../_models/goal';

@Injectable({
  providedIn: 'root',
})
export class SavingsService {
  private apiUrl = 'https://localhost:7001/api/SavingsGoal';

  constructor(private http: HttpClient) {}

  createSavingsGoal(userData: any) {
    return this.http.post<Goal>(this.apiUrl, userData);
  }

  getSavingsGoal() {
    return this.http.get<Goal[]>(this.apiUrl);
  }

  updateSavingsGoal(id: any, formData: any) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }

  deleteSavingsGoal(id: any) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
