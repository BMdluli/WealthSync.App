import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { SavingsService } from '../../_services/savings.service';
import { Goal } from '../../_models/goal';
import { BudgetService } from '../../_services/budget.service';
import { Budget } from '../../_models/budget';
import { BudgetCardComponent } from '../budget/budget-card/budget-card.component';
import { forkJoin } from 'rxjs';
import { LoaderComponent } from '../../loader/loader.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HeaderComponent, BudgetCardComponent, LoaderComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  totalSavings = 0;
  savings: Goal[] = [];
  budgetItemCount: number = 0;
  budgetItems: Budget[] = [];
  loading = true;

  constructor(
    private savingsService: SavingsService,
    private budgetService: BudgetService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData() {
    forkJoin({
      savings: this.savingsService.getSavingsGoalLimit(),
      budgetItems: this.budgetService.getbudgetItemsLimit(),
    }).subscribe({
      next: ({ savings, budgetItems }) => {
        this.savings = savings;
        if (Array.isArray(savings)) {
          savings.forEach((item) => {
            if (item && item.currentAmount) {
              this.totalSavings += item.currentAmount;
            }
          });
        }

        this.budgetItemCount = budgetItems.length;
        this.budgetItems = budgetItems;
      },
      error: (err) => console.error(err),
      complete: () => (this.loading = false),
    });
  }
}
