import { CommonModule } from '@angular/common';
import { Component, input, Input, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CreateExpenseModalComponent } from '../../../modals/create-expense-modal/create-expense-modal.component';
import { BudgetService } from '../../../_services/budget.service';
import { EditBudgetModalComponent } from '../../../modals/edit-budget-modal/edit-budget-modal.component';
import { ModalService } from '../../../_services/modal.service';

@Component({
  selector: 'app-budget-card',
  standalone: true,
  imports: [RouterModule, CommonModule, EditBudgetModalComponent],
  templateUrl: './budget-card.component.html',
  styleUrl: './budget-card.component.scss',
})
export class BudgetCardComponent implements OnInit {
  @Input() title: string = '';
  @Input() income: number = 0;
  @Input() startDate: Date = new Date();
  @Input() endDate: Date = new Date();
  @Input() subTitle: string = '';
  @Input() id: number = 0;
  @Input() isOnDashboard = false;

  isEditOpen = false;

  constructor(
    private budgetService: BudgetService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.modalService.getModalState('editBudgetModal').subscribe((isOpen) => {
      this.isEditOpen = isOpen;
    });
    console.log(this.id);
  }

  openModal() {
    this.modalService.openModal('editBudgetModal', {
      id: this.id,
      name: this.title,
      startDate: this.startDate,
      endDate: this.endDate,
      income: this.income,
    });
  }

  deleteBudget() {
    this.budgetService.deleteBudget(this.id).subscribe({
      next: (_) => console.log('Budget Deleted successfully'),
      error: (err) => console.error(err),
    });
  }
}
