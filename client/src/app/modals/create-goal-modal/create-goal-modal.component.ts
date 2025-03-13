import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SavingsService } from '../../_services/savings.service';
import { ModalService } from '../../_services/modal.service';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';

@Component({
  selector: 'app-create-goal-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './create-goal-modal.component.html',
  styleUrl: './create-goal-modal.component.scss',
})
export class CreateGoalModalComponent implements OnInit {
  isModalOpen = false;
  loading = false;

  goalModel = {
    name: '',
    targetAmount: '',
    startDate: Date(),
    targetDate: Date(),
  };

  constructor(
    private goalService: SavingsService,
    private modalService: ModalService,
    private toasr: ToastrService,
    private refreshService: RefreshService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('goalModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  handleSubmit(form: any) {
    this.loading = true;
    if (form.valid) {
      const goalData = {
        ...this.goalModel,
        startDate: new Date(this.goalModel.startDate).toISOString(),
        targetDate: new Date(this.goalModel.targetDate).toISOString(),
      };
      this.goalService.createSavingsGoal(goalData).subscribe({
        next: () => {
          this.toasr.success('Goal Added successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (error) => console.error(error),
        complete: () => (this.loading = false),
      });
      console.log(this.goalModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('goalModal');
  }
}
