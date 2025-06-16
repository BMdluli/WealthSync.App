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
import { PieChartModule, BarChartModule } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-stocks',
  standalone: true,
  imports: [
    HeaderComponent,
    StockCardComponent,
    CommonModule,
    CreateStockModalComponent,
    LoaderComponent,
    PieChartModule,
    BarChartModule,
  ],
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.scss',
})
export class StocksComponent {
  stocks: Stock[] = [];
  portfolioAllocation: any[] = [];
  annualDividendIncomeData: any[] = [];
  total: number = 0;
  gain: number = 0;
  annualIncome: number = 0;
  isLoading = false;
  xAxisLabel = 'Stock';
  yAxisLabel = 'Percentage';

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

        this.total = 0;
        this.gain = 0;
        this.annualIncome = 0;

        this.stocks.forEach((stock) => {
          this.total += stock.shares * stock.currentPrice;

          this.gain +=
            (stock.currentPrice - stock.purchasePrice) * stock.shares;

          this.annualIncome +=
            stock.shares * (stock.currentPrice * (stock.dividendYield / 100));
        });
        this.prepareChartData();
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

  prepareChartData() {
    // Prepare data for the portfolio allocation donut chart
    this.portfolioAllocation = this.stocks.map((stock) => ({
      name: stock.symbol,
      value: stock.shares * stock.currentPrice,
    }));

    // Prepare data for the annual dividend income bar chart
    this.annualDividendIncomeData = this.stocks.map((stock) => ({
      name: stock.symbol,
      value: stock.dividendYield,
    }));
  }

  view: [number, number] = [700, 400];

  // options
  legendPosition: any = 'below';

  colorScheme: any = {
    domain: ['#4FCD55', '#217FFA', '#FE5F00', '#FEB903'],
  };

  onSelect(data: any): void {
    console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }

  onActivate(data: any): void {
    console.log('Activate', JSON.parse(JSON.stringify(data)));
  }

  onDeactivate(data: any): void {
    console.log('Deactivate', JSON.parse(JSON.stringify(data)));
  }
}
