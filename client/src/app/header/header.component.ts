import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  navItems = [
    {
      id: 0,
      title: 'Dashboard',
      url: '/dashboard',
      imageUel: 'assets/icon-home.png',
    },
    {
      id: 1,
      title: 'Savings',
      url: '/savings',
      imageUel: 'assets/icon-wallet.png',
    },
    {
      id: 2,
      title: 'Budget',
      url: '/budget',
      imageUel: 'assets/icon-credit-card.png',
    },
    {
      id: 3,
      title: 'Stocks',
      url: '/stocks',
      imageUel: 'assets/icon-bar-chart.png',
    },
  ];

  constructor(private router: Router) {}

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['login']);
  }
}
