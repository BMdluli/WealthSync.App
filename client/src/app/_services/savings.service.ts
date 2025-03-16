import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Goal } from '../_models/goal';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SavingsService {
  private apiUrl = `${environment.apiUrl}/SavingsGoal`;

  constructor(private http: HttpClient) {}

  createSavingsGoal(userData: any) {
    return this.http.post<Goal>(this.apiUrl, userData);
  }

  getSavingsGoal() {
    return this.http.get<Goal[]>(this.apiUrl);
  }

  getSavingsGoalLimit() {
    return this.http.get<Goal[]>(`${this.apiUrl}?limit=6`);
  }

  updateSavingsGoal(id: any, formData: any) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }

  deleteSavingsGoal(id: any) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
