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

        console.log(this.goalModel);
      }
    });
  }

  handleSubmit(form: any) {
    this.loading = true;
    if (form.valid) {
      const savingsData = {
        ...this.goalModel,
        startDate: new Date(this.goalModel.startDate).toISOString(),
        endDate: new Date(this.goalModel.targetDate).toISOString(),
      };
      this.goalService.updateSavingsGoal(this.id, savingsData).subscribe({
        next: (_) => {
          this.toasr.success('Goal updated successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (err) => console.error(err),
        complete: () => (this.loading = false),
      });
      console.log(this.goalModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('editGoalModal');
  }

  formatDate(dateString: string) {
    const date = new Date(dateString);
    const formattedDate = date.toISOString().split('T')[0];

    return formattedDate;
  }
}
