import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
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
  isCollapsed = false;

  toggleCollapsed() {
    this.isCollapsed = !this.isCollapsed;
  }
}
