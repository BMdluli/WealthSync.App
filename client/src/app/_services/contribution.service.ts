import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ContributionService {
  apiUrl = 'https://localhost:7001/api/Contribution';

  constructor(private http: HttpClient) {}

  addSavings(formData: any) {
    return this.http.post(this.apiUrl, formData);
  }
}
