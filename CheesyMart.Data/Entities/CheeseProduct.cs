using CheesyMart.Data.Enums;

namespace CheesyMart.Data.Entities;

public class CheeseProduct : BaseEntity
{
    public required string Name { get; set; }
    public required CheeseType CheeseType { get; set; }
    public CheeseColor? Color { get; set; }
    public required decimal PricePerKilo { get; set; }
    public IList<ProductImage> Images { get; set; } = new List<ProductImage>();
}