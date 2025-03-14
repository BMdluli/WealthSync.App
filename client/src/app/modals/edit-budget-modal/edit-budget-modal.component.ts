import { Component } from '@angular/core';
import { BudgetService } from '../../_services/budget.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';

@Component({
  selector: 'app-edit-budget-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './edit-budget-modal.component.html',
  styleUrl: './edit-budget-modal.component.scss',
})
export class EditBudgetModalComponent {
  isModalOpen = false;
  loading = false;
  id = 0;

  budgetModal = {
    name: '',
    startDate: '',
    endDate: '',
    totalIncome: '',
  };

  constructor(
    private budgetService: BudgetService,
    private modalService: ModalService,
    private toasr: ToastrService,
    private refreshService: RefreshService
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
      this.loading = true;
      const budgetData = {
        ...this.budgetModal,
        startDate: new Date(this.budgetModal.startDate).toISOString(),
        endDate: new Date(this.budgetModal.endDate).toISOString(),
      };
      this.budgetService.updateBudget(this.id, budgetData).subscribe({
        next: () => {
          this.toasr.success('Budget updated successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (error) => {
          console.error(error);
          this.loading = false;
        },
        complete: () => (this.loading = false),
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
