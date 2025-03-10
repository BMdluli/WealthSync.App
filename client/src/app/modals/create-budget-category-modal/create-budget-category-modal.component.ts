import { Component, Input } from '@angular/core';
import { BudgetCategoryService } from '../../_services/budget-category.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-budget-category-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-budget-category-modal.component.html',
  styleUrl: './create-budget-category-modal.component.scss',
})
export class CreateBudgetCategoryModalComponent {
  @Input() id: string = '';
  isModalOpen = false;

  budgetCategoryModel = {
    budgetId: '',
    name: '',
    allocatedAmount: '',
  };

  constructor(
    private budgetCategoryService: BudgetCategoryService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.modalService
      .getModalState('budgetCategoryModal')
      .subscribe((isOpen) => {
        this.isModalOpen = isOpen;
      });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      this.budgetCategoryModel.budgetId = this.id;
      this.budgetCategoryService
        .createBudgetCategory(this.budgetCategoryModel)
        .subscribe({
          next: () => {
            console.log('Budget Category created successfully');
            this.closeModal();
          },
          error: (error) => console.error(error),
        });
      console.log(this.budgetCategoryModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('budgetCategoryModal');
  }
}
