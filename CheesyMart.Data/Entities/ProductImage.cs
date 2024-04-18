namespace CheesyMart.Data.Entities;

public class ProductImage : BaseEntity
{
    public string AlternateText { get; set; }
    public byte[] Data { get; set; }
    public int? CheeseProductId { get; set; }
    public CheeseProduct? CheeseProduct { get; set; }
}