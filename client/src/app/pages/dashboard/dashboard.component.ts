import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { SavingsService } from '../../_services/savings.service';
import { Goal } from '../../_models/goal';
import { SavingsCardComponent } from '../../savings-card/savings-card.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HeaderComponent, SidebarComponent, SavingsCardComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  totalSavings = 0;
  savings: Goal[] = [];

  constructor(private savingsService: SavingsService) {}

  ngOnInit(): void {
    this.getSavingsAmount();
  }

  getSavingsAmount() {
    this.savingsService.getSavingsGoal().subscribe({
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
}
