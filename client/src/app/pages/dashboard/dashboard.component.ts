import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { SavingsService } from '../../_services/savings.service';
import { Goal } from '../../_models/goal';
import { BudgetService } from '../../_services/budget.service';
import { Budget } from '../../_models/budget';
import { BudgetCardComponent } from '../budget/budget-card/budget-card.component';
import { forkJoin } from 'rxjs';
import { LoaderComponent } from '../../loader/loader.component';
import { StocksService } from '../../_services/stocks.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    HeaderComponent,
    BudgetCardComponent,
    LoaderComponent,
    CommonModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  totalSavings = 0;
  savings: Goal[] = [];
  // budgetItemCount: number = 0;
  budgetItems: Budget[] = [];
  stockTotal: number = 0;
  loading = true;

  constructor(
    private savingsService: SavingsService,
    private budgetService: BudgetService,
    private stockService: StocksService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData() {
    forkJoin({
      savings: this.savingsService.getSavingsGoalLimit(),
      budgetItems: this.budgetService.getbudgetItemsLimit(),
      stocks: this.stockService.getStockPrices(),
      // count: this.budgetService.getbudgetCount(),
    }).subscribe({
      next: ({ savings, budgetItems, stocks }) => {
        this.savings = savings;
        if (Array.isArray(savings)) {
          savings.forEach((item) => {
            if (item && item.currentAmount) {
              this.totalSavings += item.currentAmount;
            }
          });
        }

        stocks.forEach((stock) => {
          this.stockTotal += stock.currentPrice * stock.shares;
        });

        // this.budgetItemCount = count.count;
        this.budgetItems = budgetItems;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
      complete: () => (this.loading = false),
    });
  }
}
