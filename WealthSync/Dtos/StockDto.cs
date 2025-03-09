namespace WealthSync.Dtos;

public class StockDto
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public double Shares { get; set; }
    public double PurchasePrice { get; set; }
    public DateTime PurchaseDate { get; set; }
    public double CurrentPrice { get; set; }
    public double DividendYield { get; set; }
    public double TotalDividends { get; set; }
    public string DividendFrequency { get; set; }
}