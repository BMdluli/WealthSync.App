import { Component } from '@angular/core';
import { StocksComponent } from '../../pages/stocks/stocks.component';
import { ModalService } from '../../_services/modal.service';
import { FormsModule } from '@angular/forms';
import { StocksService } from '../../_services/stocks.service';

@Component({
  selector: 'app-create-stock-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-stock-modal.component.html',
  styleUrl: './create-stock-modal.component.scss',
})
export class CreateStockModalComponent {
  isModalOpen = false;

  stockModel = {
    symbol: '',
    shares: 0,
    purchasePrice: 0,
  };

  constructor(
    private stockService: StocksService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.modalService.getModalState('stockModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  handleSubmit(form: any) {
    if (form.valid) {
      this.stockService.addStock(this.stockModel).subscribe({
        next: () => console.log('Stock Added successfully'),
        error: (error) => console.error(error),
      });
    }
  }

  closeModal() {
    this.modalService.closeModal('stockModal');
  }
}
