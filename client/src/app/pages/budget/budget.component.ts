import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { HeaderComponent } from '../../header/header.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { BudgetCardComponent } from './budget-card/budget-card.component';
import { BudgetService } from '../../_services/budget.service';
import { Budget } from '../../_models/budget';
@Component({
  selector: 'app-budget',
  standalone: true,
  imports: [
    SidebarComponent,
    HeaderComponent,
    NgxChartsModule,
    BudgetCardComponent,
  ],
  templateUrl: './budget.component.html',
  styleUrl: './budget.component.scss',
})
export class BudgetComponent implements OnInit {
  budgetItems: Budget[] = [];
  constructor(private budgetService: BudgetService) {}

  ngOnInit(): void {
    this.getBudgets();
  }

  getBudgets() {
    this.budgetService.getbudgetItems().subscribe({
      next: (response) => (this.budgetItems = response),
      error: (err) => console.error(err),
    });
  }
}
