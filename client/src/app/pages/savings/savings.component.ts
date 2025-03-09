import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { SavingsCardComponent } from '../../savings-card/savings-card.component';
import { CreateGoalModalComponent } from '../../modals/create-goal-modal/create-goal-modal.component';
import { ModalService } from '../../_services/modal.service';
import { SavingsService } from '../../_services/savings.service';
import { Goal } from '../../_models/goal';

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
  savings: Goal[] = [];

  constructor(
    private modalService: ModalService,
    private savingsService: SavingsService
  ) {}

  ngOnInit() {
    this.getSavingsGoals();
    this.modalService.getModalState('goalModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  openModal() {
    this.modalService.openModal('goalModal');
  }

  getSavingsGoals() {
    this.savingsService.getSavingsGoal().subscribe({
      next: (response) => (this.savings = response),
      error: (err) => console.error(err),
    });
  }
}
