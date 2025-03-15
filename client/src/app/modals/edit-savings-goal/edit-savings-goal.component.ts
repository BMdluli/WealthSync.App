import { Component, input, Input } from '@angular/core';
import { SavingsService } from '../../_services/savings.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { Goal } from '../../_models/goal';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';

@Component({
  selector: 'app-edit-savings-goal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './edit-savings-goal.component.html',
  styleUrl: './edit-savings-goal.component.scss',
})
export class EditSavingsGoalComponent {
  id = 0;
  loading = false;
  isModalOpen = false;

  goalModel = {
    name: '',
    targetAmount: 0,
    startDate: Date(),
    targetDate: '',
  };

  constructor(
    private goalService: SavingsService,
    private modalService: ModalService,
    private toasr: ToastrService,
    private refreshService: RefreshService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('editGoalModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        const modalData = this.modalService.getModalData('editGoalModal');
        this.goalModel.name = modalData.name || '';
        this.goalModel.targetAmount = modalData.targetAmount || 50;
        this.goalModel.startDate = this.formatDate(modalData.startDate);
        this.goalModel.targetDate = this.formatDate(modalData.targetDate);
        this.id = modalData.id;

        console.log(modalData.startDate);
      }
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      this.loading = true;
      const savingsData = {
        ...this.goalModel,
        startDate: new Date(
          this.goalModel.startDate + 'T00:00:00Z'
        ).toISOString(),
        targetDate: new Date(
          this.goalModel.targetDate + 'T00:00:00Z'
        ).toISOString(),
      };
      this.goalService.updateSavingsGoal(this.id, savingsData).subscribe({
        next: () => {
          this.toasr.success('Goal updated successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (error) => {
          console.error(error);
          this.loading = false;
        },
        complete: () => (this.loading = false),
      });
      console.log(this.goalModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('editGoalModal');
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';

    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }
}
