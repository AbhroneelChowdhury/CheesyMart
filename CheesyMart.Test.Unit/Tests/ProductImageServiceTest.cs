using AutoFixture;
using CheesyMart.Core.CommandModels;
using CheesyMart.Core.Implementations;
using CheesyMart.Infrastructure.Exceptions;
using CheesyMart.Test.Unit.Utils;

namespace CheesyMart.Test.Unit.Tests;

public class ProductImageServiceTest : UnitTestBase
{
    [Fact]
    public async Task Delete_WhenProductImageIdDoesNotExist_ThenNotFound()
    {
        var db = GetDbContext();
        var productImage = db.AddProductImage("Test1");

        var sut = Fixture.Create<ProductImageService>();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await sut.DeleteProductImage(2);
        });

        Assert.Contains("Item not found", exception.Message);
    }
    
    [Fact]
    public async Task Delete_WhenProductImageIdExist_ThenOk()
    {
        var db = GetDbContext();
        var productImage = db.AddProductImage("Test1");

        var sut = Fixture.Create<ProductImageService>();
        
        await sut.DeleteProductImage(productImage.Id);
        
        Assert.DoesNotContain(db.ProductImages.Select(p => p.Id).ToList()
            , i => i == productImage.Id);
    }
    
    [Fact]
    public async Task Get_WhenProductImageIdDoesNotExist_ThenNotFound()
    {
        var db = GetDbContext();
        var productImage = db.AddProductImage("Test1");

        var sut = Fixture.Create<ProductImageService>();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await sut.GetProductImage(2);
        });

        Assert.Contains("Item not found", exception.Message);
    }
    
    [Fact]
    public async Task Get_WhenProductImageIdExist_ThenNotOk()
    {
        var db = GetDbContext();
        var productImage = db.AddProductImage("Test1");

        var sut = Fixture.Create<ProductImageService>();

        var result = await sut.GetProductImage(productImage.Id);
        
        Assert.Equal(productImage.Id, result.Id);
        Assert.Equal("Test1", result.AlternateText);
    }
    
    [Fact]
    public async Task Add_NonLinkedProductImageEntry_ThenReturnModel()
    {
        var db = GetDbContext();
        var sut = Fixture.Create<ProductImageService>();

        var result=   await sut.AddProductImage(new ProductImageCommandModel
        {
            AlternateText = "TestAlternate",
            Data = "Y2FzY3NhY2FzY2Fz",
            MimeType = "image/jpeg"
        });
        
        Assert.Equal("TestAlternate", result.AlternateText);
    }
    
    [Fact]
    public async Task Add_LinkedProductImageEntry_ThenReturnModel()
    {
        var db = GetDbContext();
        var product = db.AddProduct("Test1");
        var sut = Fixture.Create<ProductImageService>();

        var result=   await sut.AddProductImage(new ProductImageCommandModel
        {
            AlternateText = "TestAlternate",
            Data = "Y2FzY3NhY2FzY2Fz",
            MimeType = "image/jpeg",
            CheesyProductId = product.Id
        });
        
        Assert.Equal("TestAlternate", result.AlternateText);
        Assert.Equal(2,
            db.ProductImages.Where(p => p.CheeseProductId == product.Id)
                .Select(p => p.Id).ToList().Count
            );
    }
}