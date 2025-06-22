namespace WealthSync.Dtos;

public class StockDetailDto : StockDto
{
    public List<StockPriceDto> Prices { get; set; }
    public List<DividendDto> Dividends { get; set; }
}