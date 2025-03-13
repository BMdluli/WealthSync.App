import { Component, Input } from '@angular/core';
import { StocksService } from '../../_services/stocks.service';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { ContributionService } from '../../_services/contribution.service';
import { SpinnerComponent } from '../../spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { SavingsService } from '../../_services/savings.service';
import { RefreshService } from '../../_services/refresh.service';

@Component({
  selector: 'app-add-contribution-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './add-contribution-modal.component.html',
  styleUrl: './add-contribution-modal.component.scss',
})
export class AddContributionModalComponent {
  @Input() id: string = '';
  isModalOpen = false;
  loading = false;

  contributionModel = {
    savingId: '',
    amount: '',
    description: '',
  };

  constructor(
    private modalService: ModalService,
    private refeshService: RefreshService,
    private contributionService: ContributionService,
    private toastr: ToastrService
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
    this.loading = true;
    if (form.valid) {
      this.contributionService.addSavings(this.contributionModel).subscribe({
        next: () => {
          this.toastr.success('Contribution Added successfully');
          this.closeModal();
          this.refeshService.refreshPage();
        },
        error: (error) => console.error(error),
        complete: () => (this.loading = false),
      });
    }
  }

  closeModal() {
    this.modalService.closeModal('contributionModal');
  }
}
