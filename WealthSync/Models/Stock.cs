using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;
using WealthSync.Data;
using WealthSync.Models;

public class Stock
{
    public int Id { get; set; }



    [Required]
    public string Symbol { get; set; }

    [Required]
    public string Name { get; set; }
    
    public double Shares { get; set; }
    
    public double PurchasePrice { get; set; }

    public DateTime PurchaseDate { get; set; }

    public List<StockPrice> StockPrices { get; set; }
    public List<Dividend> Dividends { get; set; }
    
    [Required]
    public string UserId { get; set; }
    public AppUser AppUser { get; set; } // Assumes AppUser is your Identity user class

}