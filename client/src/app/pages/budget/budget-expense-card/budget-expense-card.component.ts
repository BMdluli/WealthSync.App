import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ModalService } from '../../../_services/modal.service';
import { CreateExpenseModalComponent } from '../../../modals/create-expense-modal/create-expense-modal.component';
import { BudgetCategoryService } from '../../../_services/budget-category.service';

@Component({
  selector: 'app-budget-expense-card',
  standalone: true,
  imports: [CommonModule, CreateExpenseModalComponent],
  templateUrl: './budget-expense-card.component.html',
  styleUrl: './budget-expense-card.component.scss',
})
export class BudgetExpenseCardComponent {
  @Input() title: string = '';
  @Input() income: number = 0;
  @Input() subTitle: string = '';
  @Input() id: number = 0;

  isExpenseModalOpen = false;

  constructor(
    private modalService: ModalService,
    private budgetCategoryService: BudgetCategoryService
  ) {}

  ngOnInit(): void {
    this.modalService.getModalState('expenseModal').subscribe((isOpen) => {
      this.isExpenseModalOpen = isOpen;
    });
    console.log(this.id);
  }

  openExpenseModal() {
    this.modalService.openModal('expenseModal', { budgetCategoryId: this.id });
  }

  deleteBudgetCategory() {
    this.budgetCategoryService.deleteBudgetCategory(this.id).subscribe({
      next: (_) => console.log('Budget Category deleted successfully'),
      error: (err) => console.error(err),
    });
  }
}
