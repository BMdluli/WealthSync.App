import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ModalService } from '../../../_services/modal.service';
import { CreateExpenseModalComponent } from '../../../modals/create-expense-modal/create-expense-modal.component';
import { BudgetCategoryService } from '../../../_services/budget-category.service';
import { EditBudgetCategoryModalComponent } from '../../../modals/edit-budget-category-modal/edit-budget-category-modal.component';
import { RefreshService } from '../../../_services/refresh.service';
import { SpinnerComponent } from '../../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-budget-expense-card',
  standalone: true,
  imports: [
    CommonModule,
    CreateExpenseModalComponent,
    EditBudgetCategoryModalComponent,
    SpinnerComponent,
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
  loading = false;

  constructor(
    private modalService: ModalService,
    private budgetCategoryService: BudgetCategoryService,
    private refreshService: RefreshService,
    private toastr: ToastrService
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
    this.loading = true;
    this.budgetCategoryService.deleteBudgetCategory(this.id).subscribe({
      next: () => {
        this.toastr.success('Budget Category deleted successfully');
        this.refreshService.refreshPage();
      },

      error: (err) => {
        console.error(err);
        this.loading = false;
      },
      complete: () => (this.loading = false),
    });
    console.log(this.id);
  }
}
