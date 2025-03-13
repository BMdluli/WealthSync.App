import { Component, Input } from '@angular/core';
import { BudgetCategoryService } from '../../_services/budget-category.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';
import { SpinnerComponent } from '../../spinner/spinner.component';

@Component({
  selector: 'app-create-budget-category-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './create-budget-category-modal.component.html',
  styleUrl: './create-budget-category-modal.component.scss',
})
export class CreateBudgetCategoryModalComponent {
  @Input() id: string = '';
  isModalOpen = false;
  loading = false;

  budgetCategoryModel = {
    budgetId: '',
    name: '',
    allocatedAmount: '',
  };

  constructor(
    private budgetCategoryService: BudgetCategoryService,
    private modalService: ModalService,
    private toasr: ToastrService,
    private refreshService: RefreshService
  ) {}

  ngOnInit() {
    this.modalService
      .getModalState('budgetCategoryModal')
      .subscribe((isOpen) => {
        this.isModalOpen = isOpen;
      });
  }

  handleSubmit(form: any) {
    this.loading = true;
    if (form.valid) {
      this.budgetCategoryModel.budgetId = this.id;
      this.budgetCategoryService
        .createBudgetCategory(this.budgetCategoryModel)
        .subscribe({
          next: () => {
            this.toasr.success('Budget Category created successfully');
            this.closeModal();
            this.refreshService.refreshPage();
          },
          error: (error) => console.error(error),
          complete: () => (this.loading = false),
        });
      console.log(this.budgetCategoryModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('budgetCategoryModal');
  }
}
