import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { HeaderComponent } from '../../header/header.component';
import { BudgetCardComponent } from './budget-card/budget-card.component';
import { BudgetService } from '../../_services/budget.service';
import { Budget } from '../../_models/budget';
import { ModalService } from '../../_services/modal.service';
import { CreateBudgetModalComponent } from '../../modals/create-budget-modal/create-budget-modal.component';
@Component({
  selector: 'app-budget',
  standalone: true,
  imports: [HeaderComponent, BudgetCardComponent, CreateBudgetModalComponent],
  templateUrl: './budget.component.html',
  styleUrl: './budget.component.scss',
})
export class BudgetComponent implements OnInit {
  budgetItems: Budget[] = [];
  constructor(
    private budgetService: BudgetService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.getBudgets();
  }

  getBudgets() {
    this.budgetService.getbudgetItems().subscribe({
      next: (response) => (this.budgetItems = response),
      error: (err) => console.error(err),
    });
  }

  openModal() {
    this.modalService.openModal('budgetModal');
  }
}
