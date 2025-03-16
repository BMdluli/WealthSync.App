import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ContributionService {
  apiUrl = `${environment.apiUrl}/Contribution`;

  constructor(private http: HttpClient) {}

  addSavings(formData: any) {
    return this.http.post(this.apiUrl, formData);
  }
}
