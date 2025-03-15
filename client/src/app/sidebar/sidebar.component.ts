import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../_services/auth.service';

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
  @Input() isOpen = false;

  constructor(private router: Router, private authService: AuthService) {}

  @Output() isOpenChange = new EventEmitter<boolean>();

  closeSidebar() {
    this.isOpenChange.emit(false);
  }

  logout() {
    this.authService.clearToken();
    this.router.navigate(['login']);
  }
}
