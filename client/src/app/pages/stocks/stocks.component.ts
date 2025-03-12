import { Component } from '@angular/core';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { HeaderComponent } from '../../header/header.component';
import { StockCardComponent } from './stock-card/stock-card.component';
import { Stock } from '../../_models/stock';
import { StocksService } from '../../_services/stocks.service';
import { CommonModule } from '@angular/common';
import { ModalService } from '../../_services/modal.service';
import { CreateStockModalComponent } from '../../modals/create-stock-modal/create-stock-modal.component';

@Component({
  selector: 'app-stocks',
  standalone: true,
  imports: [
    SidebarComponent,
    HeaderComponent,
    StockCardComponent,
    CommonModule,
    CreateStockModalComponent,
  ],
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.scss',
})
export class StocksComponent {
  stocks: Stock[] = [];
  total: number = 5000;
  isLoading = false;

  isModalOpen = false;

  constructor(
    private stocksService: StocksService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.getStocks();
    this.modalService.getModalState('stockModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
  }

  getStocks() {
    this.isLoading = true;
    this.stocksService.getStocks().subscribe({
      next: (response) => (this.stocks = response),
      error: (err) => console.error(err),
      complete: () => (this.isLoading = false),
    });
  }

  deleteStock(id: number) {
    this.stocksService.deleteStock(id).subscribe({
      next: (_) => {
        console.log('deleted successfully');
        this.getStocks();
      },
      error: (err) => console.error(err),
      complete: () => (this.isLoading = false),
    });
  }

  getProfitLossPercentage(stock: Stock): number {
    if (!stock.purchasePrice || !stock.currentPrice) return 0;
    return (
      ((stock.currentPrice - stock.purchasePrice) / stock.purchasePrice) * 100
    );
  }

  calculateSummaryValues() {
    let temp = 0;
    if (this.stocks.length !== 0) {
      this.stocks.map((stock) => {
        temp += stock.dividendYield;
        // this.total += stock.currentPrice * stock.shares;
      });
    }
    let avg = temp / this.stocks.length;

    return avg;
  }

  calculateAnnualIncome() {
    let temp = 0;
    if (this.stocks.length !== 0) {
      this.stocks.map((stock) => (temp += stock.currentPrice));
    }

    return this.calculateSummaryValues() * temp;
  }

  openModal() {
    this.modalService.openModal('stockModal');
    // alert('Clicked');
  }
}
