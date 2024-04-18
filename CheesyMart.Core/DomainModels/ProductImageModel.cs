namespace CheesyMart.Core.DomainModels;

public class ProductImageModel
{
    public int Id { get; set; }
    
    public int? CheesyProductId { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
    public string ImageData { get; set; }
    
    public string MimeType { get; set; }
    public string AltText { get; set; }
}