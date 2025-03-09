import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private modals = new Map<string, BehaviorSubject<boolean>>();

  openModal(id: string) {
    if (!this.modals.has(id)) {
      this.modals.set(id, new BehaviorSubject<boolean>(false));
    }
    this.modals.get(id)!.next(true);
  }

  closeModal(id: string) {
    if (this.modals.has(id)) {
      this.modals.get(id)!.next(false);
    }
  }

  getModalState(id: string) {
    if (!this.modals.has(id)) {
      this.modals.set(id, new BehaviorSubject<boolean>(false));
    }
    return this.modals.get(id)!.asObservable();
  }
}
