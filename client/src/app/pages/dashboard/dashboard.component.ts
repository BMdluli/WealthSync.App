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
import {
  BarChartModule,
  LineChartModule,
  PieChartModule,
} from '@swimlane/ngx-charts';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    HeaderComponent,
    BudgetCardComponent,
    LoaderComponent,
    BarChartModule,
    LineChartModule,
    PieChartModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  totalSavings = 0;
  savings: Goal[] = [];
  budgetItems: Budget[] = [];
  stockTotal = 0;
  loading = true;

  // Chart Data
  savingsChartData: any[] = [];
  budgetChartData: any[] = [];
  stockChartData: any[] = [];

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
    }).subscribe({
      next: ({ savings, budgetItems, stocks }) => {
        this.savings = savings;
        this.budgetItems = budgetItems;

        this.totalSavings = savings.reduce(
          (sum, s) => sum + (s.currentAmount || 0),
          0
        );

        // Chart: Savings Progress
        this.savingsChartData = savings.map((s) => ({
          name: s.name,
          value: Math.round((s.currentAmount / s.targetAmount) * 100),
        }));

        // Chart: Budget Line Chart
        const sortedBudgets = [...budgetItems].sort(
          (a, b) =>
            new Date(a.startDate).getTime() - new Date(b.startDate).getTime()
        );

        this.budgetChartData = [
          {
            name: 'Income',
            series: sortedBudgets.map((b) => ({
              name: b.name,
              value: b.totalIncome,
            })),
          },
        ];

        // Chart: Stock Pie Chart
        this.stockChartData = stocks.map((stock) => ({
          name: stock.symbol,
          value: stock.shares * stock.currentPrice,
        }));

        this.stockTotal = this.stockChartData.reduce(
          (sum, s) => sum + s.value,
          0
        );
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
      complete: () => (this.loading = false),
    });
  }
}
