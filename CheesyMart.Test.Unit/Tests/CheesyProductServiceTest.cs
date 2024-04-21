using AutoFixture;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Implementations;
using CheesyMart.Core.QueryModels;
using CheesyMart.Data.Context;
using CheesyMart.Infrastructure.Exceptions;
using CheesyMart.Test.Unit.Utils;

namespace CheesyMart.Test.Unit.Tests;

public class CheesyProductServiceTest : UnitTestBase
{
    
    [Fact]
    public async Task Get_WhenCheeseProductIdDoesNotExist_ThenNotFound()
    {
        var db = GetDbContext();
        var cheeseProduct = db.AddProduct("Test1");

        var sut = Fixture.Create<CheesyProductService>();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await sut.GetCheeseProductInCatalog(2);
        });

        Assert.Contains("Item not found", exception.Message);
    }
    
    [Fact]
    public async Task Get_WhenCheeseProductExist_ThenReturnModel()
    {
        var db = GetDbContext();
        var cheeseProduct = db.AddProduct("Test1");

        var sut = Fixture.Create<CheesyProductService>();

        var result =await sut.GetCheeseProductInCatalog(cheeseProduct.Id);
        
        Assert.Equal(result.Name, cheeseProduct.Name);
    }
    
    [Theory]
    [InlineData("SemiHard", "Orange", "test", 2)]
    [InlineData("SemiHard", "", "", 2)]
    [InlineData("", "Orange", "", 2)]
    [InlineData("", "", "", 2)]
    [InlineData("", "", "Test1", 1)]
    public async Task GetAll_WhenCheeseProductExist_ThenReturnModel(string cheeseType,
        string color, string name, int count)
    {
        var db = GetDbContext();
        var cheeseProduct1 = db.AddProduct("Test1");
        var cheeseProduct2 = db.AddProduct("Test2");

        var sut = Fixture.Create<CheesyProductService>();

        var result =await sut.GetCheeseProductsInCatalog(new SearchCheesyProductCatalogModel
        {
            CheeseType = cheeseType,
            Name = name,
            Color = color
        });
        
        Assert.Equal(result.Products.Count, count);
    }
    
    [Fact]
    public async Task Update_WhenCheeseProductIdExists_ThenReturnModel()
    {
        var db = GetDbContext();
        var cheeseProduct = db.AddProduct("Test1");

        var sut = Fixture.Create<CheesyProductService>();

         var result=   await sut.UpdateCheeseProductInCatalog(new CheesyProductModel
            {
                Id = cheeseProduct.Id,
                CheeseType = "SemiSoft",
                Name = "Test2",
                PricePerKilo = 4.67m,
                ProductImages = new List<int> {cheeseProduct.Images?.FirstOrDefault()?.CheeseProductId ?? 0}
            });


        Assert.Equal("Test2", result.Name);
        Assert.Equal(4.67m, result.PricePerKilo);
        Assert.Equal("SemiSoft", result.CheeseType);
        
    }
    
    [Fact]
    public async Task Delete_WhenCheeseProductIdExists_ThenDelete()
    {
        var db = GetDbContext();
        var cheeseProduct = db.AddProduct("Test1");

        var sut = Fixture.Create<CheesyProductService>();

        var result=   await sut.DeleteCheeseProductInCatalog(cheeseProduct.Id);
        
        Assert.False(db.CheeseProducts.Any(c => c.Id == cheeseProduct.Id));
    }
    
    [Fact]
    public async Task Delete_WhenCheeseProductIdDoesNotExist_ThenNotFound()
    {
        var db = GetDbContext();
        var cheeseProduct = db.AddProduct("Test1");

        var sut = Fixture.Create<CheesyProductService>();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await sut.DeleteCheeseProductInCatalog(2);
        });

        Assert.Contains("Item not found", exception.Message);
    }
    
    [Fact]
    public async Task Update_WhenCheeseProductIdNotExist_ThenNotFound()
    {
        var db = GetDbContext();
        var cheeseProduct = db.AddProduct("Test1");

        var sut = Fixture.Create<CheesyProductService>();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await sut.UpdateCheeseProductInCatalog(new CheesyProductModel
            {
                Id = 2,
                CheeseType = "SemiHard",
                Name = "Test2",
                PricePerKilo = 4.67m,
                ProductImages = new List<int> {cheeseProduct.Images?.FirstOrDefault()?.CheeseProductId ?? 0}
            });
        });

        Assert.Contains("Item not found", exception.Message);
    }
    
    [Fact]
    public async Task Add_WhenCheeseProductValidEntry_ThenReturnModel()
    {
        var db = GetDbContext();
        var productImage1 = db.AddProductImage();
        var productImage2 = db.AddProductImage();

        var sut = Fixture.Create<CheesyProductService>();

        var result=   await sut.AddCheeseProductToCatalog(new CheesyProductModel
        {
            CheeseType = "SemiSoft",
            Name = "Test2",
            PricePerKilo = 4.67m,
            Color = "Orange",
            ProductImages = new List<int> {productImage1.Id, productImage2.Id}
        });

        var imageIds = db.ProductImages.Where(p => p.CheeseProductId == result.Id)
            .Select(i => i.Id).ToList();
        
        Assert.Equal("Test2", result.Name);
        Assert.Equal(4.67m, result.PricePerKilo);
        Assert.Equal("SemiSoft", result.CheeseType);
        Assert.Equal("Orange", result.Color);
        Assert.Contains(imageIds, i => i == productImage1.Id);
        Assert.Contains(imageIds, i => i == productImage2.Id);
    }
}

  

    