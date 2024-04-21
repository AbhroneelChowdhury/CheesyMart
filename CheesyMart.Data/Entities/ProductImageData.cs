namespace CheesyMart.Data.Entities;

public class ProductImageData
{
    public int Id { get; set; }
    
    public byte[] Data { get; set; }
    
    public string AlternateText { get; set; }
    
    public string MimeType { get; set; }
}