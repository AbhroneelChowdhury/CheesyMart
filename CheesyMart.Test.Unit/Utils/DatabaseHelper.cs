using CheesyMart.Data.Context;
using CheesyMart.Data.Entities;
using CheesyMart.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace CheesyMart.Test.Unit.Utils;

public static class DatabaseHelper
{
    /// <summary>
    /// Creates an empty in memory database ready for unit testing.
    /// </summary>
    /// <returns>Database context ready for unit testing.</returns>
    public static MainDbContext GenerateDatabase()
    {
        var optsBuilder = new DbContextOptionsBuilder<MainDbContext>();
        optsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()); //Using guid so no 2 tests get the same database.

        var db = new MainDbContext(optsBuilder.Options);
        return db;
    }
    
    public static ProductImage AddProductImage(this MainDbContext db,
        string alternateText = "Test alternate text", int? cheesyProductId = null)
    {
        var productImage = new ProductImage
        {
            ProductImageData = new ProductImageData
            {
                AlternateText = alternateText,
                Data = new byte[]{ 255,216,255,224},
                MimeType = "image/jpeg"
            },
            LastUpdated = DateTime.UtcNow,
        };

        if (cheesyProductId != null)
            productImage.CheeseProductId = cheesyProductId;

        db.ProductImages.Add(productImage);
        db.SaveChanges();

        return productImage;
    }
    
    public static CheeseProduct AddProduct(this MainDbContext db,
        string name, CheeseType cheeseType = CheeseType.SemiHard, CheeseColor cheeseColor = CheeseColor.Orange,
        decimal price = 4.56m, bool addImage = true)
    {
        var product = new CheeseProduct
        {
            Name = string.IsNullOrEmpty(name) ? "Test Product" : name,
            CheeseType = cheeseType,
            PricePerKilo = price,
            Color = cheeseColor,
            LastUpdated = DateTime.UtcNow,
        };

        if (addImage)
        {
            product.Images = new List<ProductImage>()
            {
                new()
                {
                    ProductImageData = new ProductImageData
                    {
                        AlternateText = "Test image",
                        Data = new byte[] { 255, 216, 255, 224 },
                        MimeType = "image/jpeg"
                    },
                    LastUpdated = DateTime.UtcNow,
                }
            };

        }

        db.CheeseProducts.Add(product);
        
        db.SaveChanges();

        return product;
    }
}


        


