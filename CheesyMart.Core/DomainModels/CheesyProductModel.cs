using CheesyMart.Data.Enums;

namespace CheesyMart.Core.DomainModels;

public class CheesyProductModel
{
    public int Id { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
    
    public string Name { get; set; }
    
    public string CheeseType { get; set; }
    
    public string? Color { get; set; }
    
    public decimal PricePerKilo { get; set; }
    
    public IList<int> ProductImages { get; set; }
}