import { Component } from '@angular/core';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-stocks',
  standalone: true,
  imports: [SidebarComponent, HeaderComponent],
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.scss',
})
export class StocksComponent {}
