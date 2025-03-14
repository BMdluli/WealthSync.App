import { Component } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { StockCardComponent } from './stock-card/stock-card.component';
import { Stock } from '../../_models/stock';
import { StocksService } from '../../_services/stocks.service';
import { CommonModule } from '@angular/common';
import { ModalService } from '../../_services/modal.service';
import { CreateStockModalComponent } from '../../modals/create-stock-modal/create-stock-modal.component';
import { LoaderComponent } from '../../loader/loader.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-stocks',
  standalone: true,
  imports: [
    HeaderComponent,
    StockCardComponent,
    CommonModule,
    CreateStockModalComponent,
    LoaderComponent,
  ],
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.scss',
})
export class StocksComponent {
  stocks: Stock[] = [];
  total: number = 0;
  gain: number = 0;
  annualIncome: number = 0;
  isLoading = false;

  isModalOpen = false;

  constructor(
    private stocksService: StocksService,
    private modalService: ModalService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getStocks();
    this.modalService.getModalState('stockModal').subscribe((isOpen) => {
      this.isModalOpen = isOpen;
    });
    this.toastr.info(
      'Please note that this works with AMERICAN COMPANIES and not ETFS or BONDS'
    );
  }

  getStocks() {
    this.isLoading = true;
    this.stocksService.getStocks().subscribe({
      next: (response) => {
        this.stocks = response;
        response.map((stock) => {
          this.total += stock.shares * stock.currentPrice;
          this.gain += stock.currentPrice - stock.purchasePrice;
          this.annualIncome += stock.currentPrice * stock.dividendYield;
        });
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);
      },
      complete: () => (this.isLoading = false),
    });
  }

  deleteStock(id: number) {
    this.isLoading = true;
    this.stocksService.deleteStock(id).subscribe({
      next: (_) => {
        this.toastr.success('stock deleted successfully');
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

  openModal() {
    this.modalService.openModal('stockModal');
  }

  getFrequency(frequency: string) {
    if (frequency === 'Monthly') return 12;

    if (frequency === 'Quarterly') return 4;

    if (frequency === 'Semi-Annually') return 2;

    return 4;
  }
}
