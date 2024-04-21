namespace CheesyMart.Data.Entities;

public class ProductImage : BaseEntity
{
    public int? CheeseProductId { get; set; }
    public CheeseProduct? CheeseProduct { get; set; }
    public required ProductImageData ProductImageData { get; set; }
}