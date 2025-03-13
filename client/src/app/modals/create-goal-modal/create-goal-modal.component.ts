import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SavingsService } from '../../_services/savings.service';
import { ModalService } from '../../_services/modal.service';

@Component({
  selector: 'app-create-goal-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-goal-modal.component.html',
  styleUrl: './create-goal-modal.component.scss',
})
export class CreateGoalModalComponent implements OnInit {
  isModalOpen = false;

  goalModel = {
    name: '',
    targetAmount: '',
    startDate: Date(),
    targetDate: Date(),
  };

  constructor(
    private goalService: SavingsService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('goalModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      const goalData = {
        ...this.goalModel,
        startDate: new Date(this.goalModel.startDate).toISOString(),
        targetDate: new Date(this.goalModel.targetDate).toISOString(),
      };
      this.goalService.createSavingsGoal(goalData).subscribe({
        next: () => console.log('Goal Added successfully'),
        error: (error) => console.error(error),
      });
      console.log(this.goalModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('goalModal');
  }
}
