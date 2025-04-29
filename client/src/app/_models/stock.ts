export interface Stock {
  id: number;
  currentPrice: number;
  dividendYield: number;
  name: string;
  purchaseDate: Date;
  purchasePrice: number;
  shares: number;
  symbol: string;
  dividendFrequency: string;
}
