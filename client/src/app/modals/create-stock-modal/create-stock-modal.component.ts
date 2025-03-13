import { Component } from '@angular/core';
import { StocksComponent } from '../../pages/stocks/stocks.component';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { StocksService } from '../../_services/stocks.service';
import { ToastrService } from 'ngx-toastr';
import { RefreshService } from '../../_services/refresh.service';
import { SpinnerComponent } from '../../spinner/spinner.component';

@Component({
  selector: 'app-create-stock-modal',
  standalone: true,
  imports: [FormsModule, SpinnerComponent],
  templateUrl: './create-stock-modal.component.html',
  styleUrl: './create-stock-modal.component.scss',
})
export class CreateStockModalComponent {
  isModalOpen = false;
  loading = false;

  stockModel = {
    symbol: '',
    shares: 0,
    purchasePrice: 0,
  };

  constructor(
    private stockService: StocksService,
    private modalService: ModalService,
    private toasr: ToastrService,
    private refreshService: RefreshService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('stockModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  handleSubmit(form: any) {
    this.loading = true;
    if (form.valid) {
      this.stockService.addStock(this.stockModel).subscribe({
        next: () => {
          this.toasr.success('Stock Added successfully');
          this.closeModal();
          this.refreshService.refreshPage();
        },
        error: (error) => console.error(error),
        complete: () => (this.loading = false),
      });
    }
  }

  closeModal() {
    this.modalService.closeModal('stockModal');
  }
}
