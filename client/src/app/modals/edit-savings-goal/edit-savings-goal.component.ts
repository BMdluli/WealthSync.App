import { Component, input, Input } from '@angular/core';
import { SavingsService } from '../../_services/savings.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { Goal } from '../../_models/goal';

@Component({
  selector: 'app-edit-savings-goal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-savings-goal.component.html',
  styleUrl: './edit-savings-goal.component.scss',
})
export class EditSavingsGoalComponent {
  id = 0;

  isModalOpen = false;

  goalModel = {
    name: '',
    targetAmount: 0,
    startDate: Date(),
    targetDate: '',
  };

  constructor(
    private goalService: SavingsService,
    private modalService: ModalService
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
    if (form.valid) {
      this.goalService.updateSavingsGoal(this.id, this.goalModel).subscribe({
        next: (_) => {
          console.log('Goal updated successfully');
          this.closeModal();
        },
        error: (err) => console.error(err),
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
