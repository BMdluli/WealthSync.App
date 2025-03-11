import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private modals = new Map<string, BehaviorSubject<boolean>>();
  private modalData = new Map<string, any>(); // Store additional modal data

  openModal(id: string, data: any = null) {
    if (!this.modals.has(id)) {
      this.modals.set(id, new BehaviorSubject<boolean>(false));
    }
    this.modalData.set(id, data); // Store the data when opening the modal
    this.modals.get(id)!.next(true);
  }

  closeModal(id: string) {
    if (this.modals.has(id)) {
      this.modals.get(id)!.next(false);
      this.modalData.delete(id); // Remove data on close
    }
  }

  getModalState(id: string) {
    if (!this.modals.has(id)) {
      this.modals.set(id, new BehaviorSubject<boolean>(false));
    }
    return this.modals.get(id)!.asObservable();
  }

  getModalData(id: string) {
    return this.modalData.get(id) || null;
  }
}
