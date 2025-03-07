import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private modalVisible = new BehaviorSubject<boolean>(false);
  modalVisible$ = this.modalVisible.asObservable();

  openModal() {
    this.modalVisible.next(true);
  }

  closeModal() {
    this.modalVisible.next(false);
  }
}
