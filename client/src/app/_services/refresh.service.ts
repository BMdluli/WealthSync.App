import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RefreshService {
  refreshPage() {
    setTimeout(() => {
      location.reload();
    }, 500);
  }
}
