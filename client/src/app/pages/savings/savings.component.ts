import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { SavingsCardComponent } from '../../savings-card/savings-card.component';
import { CreateGoalModalComponent } from '../../modals/create-goal-modal/create-goal-modal.component';
import { ModalService } from '../../_services/modal.service';

@Component({
  selector: 'app-savings',
  standalone: true,
  imports: [
    HeaderComponent,
    SidebarComponent,
    SavingsCardComponent,
    CreateGoalModalComponent,
  ],
  templateUrl: './savings.component.html',
  styleUrl: './savings.component.scss',
})
export class SavingsComponent implements OnInit {
  isModalOpen = false;

  constructor(private modalService: ModalService) {}

  ngOnInit() {
    this.modalService.modalVisible$.subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  openModal() {
    this.modalService.openModal();
  }
}
