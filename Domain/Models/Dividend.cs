namespace WealthSync.Data;

public class Dividend
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public Stock Stock { get; set; }
    
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}