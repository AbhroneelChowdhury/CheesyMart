﻿namespace CheesyMart.Core.DomainModels;

public class ProductImageModel
{
    public int Id { get; set; }
    public int? CheesyProductId { get; set; }
    public string AlternateText { get; set; }
    public string MimeType { get; set; }
    public byte[] Data { get; set; }
}