import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ModalService } from '../../../_services/modal.service';
import { CreateExpenseModalComponent } from '../../../modals/create-expense-modal/create-expense-modal.component';
import { BudgetCategoryService } from '../../../_services/budget-category.service';
import { EditBudgetCategoryModalComponent } from '../../../modals/edit-budget-category-modal/edit-budget-category-modal.component';

@Component({
  selector: 'app-budget-expense-card',
  standalone: true,
  imports: [
    CommonModule,
    CreateExpenseModalComponent,
    EditBudgetCategoryModalComponent,
  ],
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
    this.modalService
      .getModalState('editBudgetCategoryModal')
      .subscribe((isOpen) => {
        this.isExpenseModalOpen = isOpen;
      });
    console.log(this.id);
  }

  openExpenseModal() {
    this.modalService.openModal('expenseModal', {
      budgetCategoryId: this.id,
    });
  }

  openBudgetCategoryModal() {
    this.modalService.openModal('editBudgetCategoryModal', {
      budgetCategoryId: this.id,
      name: this.title,
      amount: this.income,
    });
  }

  deleteBudgetCategory() {
    this.budgetCategoryService.deleteBudgetCategory(this.id).subscribe({
      next: (_) => console.log('Budget Category deleted successfully'),
      error: (err) => console.error(err),
    });
    console.log(this.id);
  }
}
