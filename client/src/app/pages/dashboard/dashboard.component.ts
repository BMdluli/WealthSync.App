import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { SavingsService } from '../../_services/savings.service';
import { Goal } from '../../_models/goal';
import { SavingsCardComponent } from '../../savings-card/savings-card.component';
import { BudgetService } from '../../_services/budget.service';
import { Budget } from '../../_models/budget';
import { BudgetCardComponent } from '../budget/budget-card/budget-card.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    HeaderComponent,
    SidebarComponent,
    SavingsCardComponent,
    BudgetCardComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  totalSavings = 0;
  savings: Goal[] = [];
  budgetItemCount: number = 0;
  budgetItems: Budget[] = [];

  constructor(
    private savingsService: SavingsService,
    private budgetService: BudgetService
  ) {}

  ngOnInit(): void {
    this.getSavingsAmount();
    this.getBudgetItems();
  }

  getSavingsAmount() {
    this.savingsService.getSavingsGoalLimit().subscribe({
      next: (response) => {
        this.savings = response;
        if (Array.isArray(response)) {
          response.forEach((item) => {
            if (item && item.currentAmount) {
              this.totalSavings += item.currentAmount; // Add currentPrice of each item to totalSavings
            }
          });
        }
      },
      error: (err) => console.error(err),
    });
  }

  getBudgetItems() {
    this.budgetService.getbudgetItemsLimit().subscribe({
      next: (response) => {
        this.budgetItemCount = response.length;
        this.budgetItems = response;
      },
      error: (err) => console.error(err),
    });
  }
}
