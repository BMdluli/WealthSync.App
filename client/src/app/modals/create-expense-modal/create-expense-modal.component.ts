import { Component, Input } from '@angular/core';
import { BudgetCategoryService } from '../../_services/budget-category.service';
import { FormsModule } from '@angular/forms';
import { ModalService } from '../../_services/modal.service';
import { ExpenseService } from '../../_services/expense.service';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';

@Component({
  selector: 'app-create-expense-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './create-expense-modal.component.html',
  styleUrl: './create-expense-modal.component.scss',
})
export class CreateExpenseModalComponent {
  @Input() id: string = '';
  isModalOpen = false;
  loading = false;

  expenseModal = {
    budgetCategoryId: '',
    description: '',
    amount: '',
    date: new Date(),
  };

  constructor(
    private modalService: ModalService,
    private expenseService: ExpenseService,
    private toasr: ToastrService,
    private refreshService: RefreshService
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
      this.loading = true;
      const expenseData = {
        ...this.expenseModal,
        startDate: new Date(this.expenseModal.date).toISOString(),
      };
      this.expenseService.createExpense(expenseData).subscribe({
        next: () => {
          this.toasr.success('created created successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (error) => {
          console.error(error);
          this.loading = false;
        },
        complete: () => (this.loading = false),
      });
      console.log(this.expenseModal);
    }
  }

  closeModal() {
    this.modalService.closeModal('expenseModal');
  }
}
