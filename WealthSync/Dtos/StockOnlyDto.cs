namespace WealthSync.Dtos;

public class StockOnlyDto
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public double Shares { get; set; }
    public double PurchasePrice { get; set; }
    public double CurrentPrice { get; set; }
}