import { Component, Input } from '@angular/core';
import { StocksService } from '../../_services/stocks.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { ContributionService } from '../../_services/contribution.service';

@Component({
  selector: 'app-add-contribution-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-contribution-modal.component.html',
  styleUrl: './add-contribution-modal.component.scss',
})
export class AddContributionModalComponent {
  @Input() id: string = '';
  isModalOpen = false;

  contributionModel = {
    savingId: '',
    amount: '',
    description: '',
  };

  constructor(
    private modalService: ModalService,
    private contributionService: ContributionService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('contributionModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        const modalData = this.modalService.getModalData('contributionModal');
        this.contributionModel.savingId = modalData?.budgetCategoryId || '';
      }
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      this.contributionService.addSavings(this.contributionModel).subscribe({
        next: () => {
          console.log('Contribution Added successfully');
          this.closeModal();
        },
        error: (error) => console.error(error),
      });
      console.log(this.contributionModel);
    }
  }

  closeModal() {
    this.modalService.closeModal('contributionModal');
  }
}
