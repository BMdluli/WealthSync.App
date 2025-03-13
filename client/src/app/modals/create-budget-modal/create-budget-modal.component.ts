import { Component } from '@angular/core';
import { BudgetService } from '../../_services/budget.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { RefreshService } from '../../_services/refresh.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-budget-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './create-budget-modal.component.html',
  styleUrl: './create-budget-modal.component.scss',
})
export class CreateBudgetModalComponent {
  isModalOpen = false;
  loading = false;

  budgetModal = {
    name: '',
    startDate: new Date(),
    endDate: new Date(),
    totalIncome: '',
  };

  constructor(
    private budgetService: BudgetService,
    private modalService: ModalService,
    private toasr: ToastrService,
    private refreshService: RefreshService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('budgetModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  handleSubmit(form: any) {
    this.loading = true;
    if (form.valid) {
      const budgetData = {
        ...this.budgetModal,
        startDate: new Date(this.budgetModal.startDate).toISOString(),
        endDate: new Date(this.budgetModal.endDate).toISOString(),
      };

      this.budgetService.createBudget(budgetData).subscribe({
        next: () => {
          this.toasr.success('Budget created successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (error) => console.error(error),
        complete: () => (this.loading = false),
      });
      console.log(budgetData);
    }
  }

  closeModal() {
    this.modalService.closeModal('budgetModal');
  }
}
