namespace CheesyMart.Core.CommandModels;

public class ProductImageCommandModel
{
    public int Id { get; set; }
    public int? CheesyProductId { get; set; }
    public string AlternateText { get; set; }
    public string MimeType { get; set; }
    public string Data { get; set; }
}