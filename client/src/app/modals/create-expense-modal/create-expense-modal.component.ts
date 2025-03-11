import { Component, Input } from '@angular/core';
import { BudgetCategoryService } from '../../_services/budget-category.service';
import { FormsModule } from '@angular/forms';
import { ModalService } from '../../_services/modal.service';
import { ExpenseService } from '../../_services/expense.service';

@Component({
  selector: 'app-create-expense-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-expense-modal.component.html',
  styleUrl: './create-expense-modal.component.scss',
})
export class CreateExpenseModalComponent {
  @Input() id: string = '';
  isModalOpen = false;

  expenseModal = {
    budgetCategoryId: '',
    description: '',
    amount: '',
    date: new Date(),
  };

  constructor(
    private budgetCategoryService: BudgetCategoryService,
    private modalService: ModalService,
    private expenseService: ExpenseService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('expenseModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        const modalData = this.modalService.getModalData('expenseModal');
        this.expenseModal.budgetCategoryId = modalData?.budgetCategoryId || '';
        console.log('BUDGET CATEGORY ID:', this.expenseModal.budgetCategoryId);
      }
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      this.expenseService.createExpense(this.expenseModal).subscribe({
        next: () => {
          console.log('created created successfully');
          this.closeModal();
        },
        error: (error) => console.error(error),
      });
      console.log(this.expenseModal);
    }
  }

  closeModal() {
    this.modalService.closeModal('expenseModal');
  }
}
