import { Component } from '@angular/core';
import { BudgetService } from '../../_services/budget.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-budget-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-budget-modal.component.html',
  styleUrl: './edit-budget-modal.component.scss',
})
export class EditBudgetModalComponent {
  isModalOpen = false;
  id = 0;

  budgetModal = {
    name: '',
    startDate: '',
    endDate: '',
    totalIncome: '',
  };

  constructor(
    private budgetService: BudgetService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('editBudgetModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        const modalData = this.modalService.getModalData('editBudgetModal');
        this.budgetModal.name = modalData.name || '';
        this.budgetModal.startDate = this.formatDate(modalData.startDate);
        this.budgetModal.endDate = this.formatDate(modalData.endDate);
        this.budgetModal.totalIncome = modalData.income;
        this.id = modalData.id;
      }
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      // this.budgetService.createBudget(this.budgetModal).subscribe({
      //   next: () => {
      //     console.log('Budget created successfully');
      //     this.closeModal();
      //   },
      //   error: (error) => console.error(error),
      // });
      this.budgetService.updateBudget(this.id, this.budgetModal).subscribe({
        next: () => {
          console.log('Budget updated successfully');
          this.closeModal();
        },
        error: (err) => console.error(err),
      });
      console.log(this.budgetModal);
    }
  }

  closeModal() {
    this.modalService.closeModal('editBudgetModal');
  }

  formatDate(dateString: string) {
    const date = new Date(dateString);
    const formattedDate = date.toISOString().split('T')[0];

    return formattedDate;
  }
}
