import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  services = [
    {
      title: 'Keep Track stocks',
      description: 'Keep track of stocks as well as the dividends they pay.',
      imageUrl: 'assets/stocks-page.png',
    },
    {
      title: 'Track Savings Goals',
      description:
        'Set up monthly budgets with budget categories to control your spending.',
      imageUrl: 'assets/savings-page.png',
    },
    {
      title: 'Track Budget',
      description:
        'Set goals that you want to save for and slowly add contributions towards them.',
      imageUrl: 'assets/budget-page.png',
    },
  ];
}
