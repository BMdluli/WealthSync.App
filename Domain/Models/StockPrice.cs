namespace WealthSync.Data;

public class StockPrice
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public Stock Stock { get; set; }
    
    public double Price { get; set; }
    public DateTime Timestamp { get; set; }
}