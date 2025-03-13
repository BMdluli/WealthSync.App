import { Component } from '@angular/core';
import { BudgetService } from '../../_services/budget.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-budget-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-budget-modal.component.html',
  styleUrl: './create-budget-modal.component.scss',
})
export class CreateBudgetModalComponent {
  isModalOpen = false;

  budgetModal = {
    name: '',
    startDate: new Date(),
    endDate: new Date(),
    totalIncome: '',
  };

  constructor(
    private budgetService: BudgetService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('budgetModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      const budgetData = {
        ...this.budgetModal,
        startDate: new Date(this.budgetModal.startDate).toISOString(),
        endDate: new Date(this.budgetModal.endDate).toISOString(),
      };

      this.budgetService.createBudget(budgetData).subscribe({
        next: () => {
          console.log('Budget created successfully');
          this.closeModal();
        },
        error: (error) => console.error(error),
      });
      console.log(budgetData);
    }
  }

  closeModal() {
    this.modalService.closeModal('budgetModal');
  }
}
