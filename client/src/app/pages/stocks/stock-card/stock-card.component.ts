import { Component, input, Input, OnInit } from '@angular/core';
import { StocksService } from '../../../_services/stocks.service';
import { Stock } from '../../../_models/stock';

@Component({
  selector: 'app-stock-card',
  standalone: true,
  imports: [],
  templateUrl: './stock-card.component.html',
  styleUrl: './stock-card.component.scss',
})
export class StockCardComponent {
  @Input() title: string = '';
  @Input() value: string = '';
  @Input() imgaeUrl: string = '';
}
