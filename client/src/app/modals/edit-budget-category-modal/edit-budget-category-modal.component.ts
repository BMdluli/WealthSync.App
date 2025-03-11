import { Component, Input } from '@angular/core';
import { BudgetCategoryService } from '../../_services/budget-category.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-edit-budget-category-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-budget-category-modal.component.html',
  styleUrl: './edit-budget-category-modal.component.scss',
})
export class EditBudgetCategoryModalComponent {
  isModalOpen = false;

  budgetCategoryModel = {
    budgetId: '',
    name: '',
    allocatedAmount: 0,
  };

  id = this.route.snapshot.paramMap.get('id');
  budgetCategoryId = 0;

  constructor(
    private budgetCategoryService: BudgetCategoryService,
    private modalService: ModalService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.modalService
      .getModalState('editBudgetCategoryModal')
      .subscribe((isOpen) => {
        this.isModalOpen = isOpen;
        if (isOpen) {
          const modalData = this.modalService.getModalData(
            'editBudgetCategoryModal'
          );
          this.budgetCategoryModel.budgetId = this.id! || '';
          this.budgetCategoryModel.name = modalData?.name || '';
          this.budgetCategoryModel.allocatedAmount = modalData?.amount || '';
          this.budgetCategoryId = modalData.budgetCategoryId;
        }
      });

    // console.log('ID _> ', this.id);
    // console.log('Budget Category ID ', this.budgetCategoryModel.budgetId);
  }

  handleSubmit(form: any) {
    if (form.valid) {
      // this.budgetCategoryModel.budgetId = this.id!;
      this.budgetCategoryService
        .editBudgetCategory(this.budgetCategoryId, this.budgetCategoryModel)
        .subscribe({
          next: () => {
            console.log('Budget Category Edited successfully');
            this.closeModal();
          },
          error: (error) => console.error(error),
        });

      console.log(this.budgetCategoryId, this.budgetCategoryModel.budgetId);
    }
  }

  closeModal() {
    this.modalService.closeModal('editBudgetCategoryModal');
  }
}
