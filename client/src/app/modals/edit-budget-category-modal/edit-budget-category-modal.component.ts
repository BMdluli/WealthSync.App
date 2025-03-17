import { Component, Input } from '@angular/core';
import { BudgetCategoryService } from '../../_services/budget-category.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';

@Component({
  selector: 'app-edit-budget-category-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './edit-budget-category-modal.component.html',
  styleUrl: './edit-budget-category-modal.component.scss',
})
export class EditBudgetCategoryModalComponent {
  isModalOpen = false;
  loading = false;

  budgetCategoryModel = {
    budgetId: '',
    name: '',
    allocatedAmount: 0,
  };

  budgetCategoryId = 0;

  constructor(
    private budgetCategoryService: BudgetCategoryService,
    private modalService: ModalService,
    private route: ActivatedRoute,
    private toasr: ToastrService,
    private refreshService: RefreshService
  ) {}

  ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');
    this.modalService
      .getModalState('editBudgetCategoryModal')
      .subscribe((isOpen) => {
        this.isModalOpen = isOpen;
        if (isOpen) {
          const modalData = this.modalService.getModalData(
            'editBudgetCategoryModal'
          );
          this.budgetCategoryModel.budgetId = id! || '';
          this.budgetCategoryModel.name = modalData?.name || '';
          this.budgetCategoryModel.allocatedAmount = modalData?.amount || '';
          this.budgetCategoryId = modalData.budgetCategoryId;
        }
      });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      this.loading = true;
      this.budgetCategoryService
        .editBudgetCategory(this.budgetCategoryId, this.budgetCategoryModel)
        .subscribe({
          next: () => {
            this.toasr.success('Budget Category Edited successfully');
            this.closeModal();
            this.refreshService.refreshPage();
          },
          error: (error) => {
            console.error(error);
            this.loading = false;
          },
          complete: () => (this.loading = false),
        });

      console.log(this.budgetCategoryId, this.budgetCategoryModel.budgetId);
    }
  }

  closeModal() {
    this.modalService.closeModal('editBudgetCategoryModal');
  }
}
