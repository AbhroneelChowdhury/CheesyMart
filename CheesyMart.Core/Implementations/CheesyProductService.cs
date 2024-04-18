using AutoMapper;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Interfaces;
using CheesyMart.Core.QueryModels;
using CheesyMart.Data.Context;
using CheesyMart.Data.Entities;
using CheesyMart.Data.Enums;
using CheesyMart.Infrastructure.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CheesyMart.Core.Implementations;

public class CheesyProductService(MainDbContext mainDbContext,
    IValidator<CheesyProductModel> validator, IMapper mapper) : ICheesyProductService
{
    public async Task<CheesyProductModel> AddCheeseProductToCatalog(CheesyProductModel cheesyProductModel)
    {
        await validator.ValidateAndThrowAsync(cheesyProductModel);
        IList<ProductImage> images = [];
        
        if (!cheesyProductModel.ProductImages.IsNullOrEmpty())
        {
            images = await mainDbContext.ProductImages.Where(i =>
                cheesyProductModel.ProductImages.Contains(i.Id) && !i.CheeseProductId.HasValue).ToListAsync();
        }
        
        var product = new CheeseProduct
        {
            Name = cheesyProductModel.Name,
            CheeseType =  (CheeseType) Enum.Parse(typeof(CheeseType), cheesyProductModel.CheeseType),
            PricePerKilo = cheesyProductModel.PricePerKilo,
            LastUpdated = DateTimeOffset.UtcNow,
            Color = cheesyProductModel.Color != null ?
                (CheeseColor) Enum.Parse(typeof(CheeseColor), cheesyProductModel.Color) : null,
            Images = images
        };

        await mainDbContext.CheeseProducts.AddAsync(product);
        await mainDbContext.SaveChangesAsync();
        cheesyProductModel.Id = product.Id;
        return cheesyProductModel;
    }

    public async Task<CheesyProductModel> UpdateCheeseProductInCatalog(CheesyProductModel cheesyProductModel)
    {
        var product = await mainDbContext.CheeseProducts
            .Include(c => c.Images)
            .FirstOrDefaultAsync(c => c.Id == cheesyProductModel.Id);
        if (product == null)
        {
            throw new NotFoundException("Item not found");
        }

        product.Name = cheesyProductModel.Name;
        product.CheeseType = (CheeseType)Enum.Parse(typeof(CheeseType), cheesyProductModel.CheeseType);
        product.PricePerKilo = cheesyProductModel.PricePerKilo;
        product.LastUpdated = DateTimeOffset.UtcNow;
        if (cheesyProductModel.Color != null)
            product.Color = (CheeseColor)Enum.Parse(typeof(CheeseColor), cheesyProductModel.Color);
        await mainDbContext.SaveChangesAsync();
        return cheesyProductModel;
    }

    public async Task<CheesyProductModel> DeleteCheeseProductInCatalog(int id)
    {
        var product = await mainDbContext.CheeseProducts.FirstOrDefaultAsync(c => c.Id == id);
        if (product == null)
        {
            throw new NotFoundException("Item not found");
        }

        mainDbContext.CheeseProducts.Remove(product);
        await mainDbContext.SaveChangesAsync();
        return mapper.Map<CheesyProductModel>(product);
    }

    public async Task<CheesyProductModel> GetCheeseProductInCatalog(int id)
    {
        var product = await mainDbContext.CheeseProducts.FirstOrDefaultAsync(c => c.Id == id);
        if (product == null)
        {
            throw new NotFoundException("Item not found");
        }
        return mapper.Map<CheesyProductModel>(product);
    }

    public async Task<List<CheesyProductModel>> GetCheeseProductsInCatalog(SearchCheesyProductCatalogModel searchModel)
    {
        var searchRequest = mainDbContext.CheeseProducts.AsQueryable();
        if (!string.IsNullOrEmpty(searchModel.Color))
        {
            var searchColor = (CheeseColor)Enum.Parse(typeof(CheeseColor), searchModel.Color);
            searchRequest = searchRequest.Where(s => s.Color == searchColor);
        }
        if (!string.IsNullOrEmpty(searchModel.Name))
            searchRequest = searchRequest.Where(s => s.Name.Contains(searchModel.Name));
        
        if (!string.IsNullOrEmpty(searchModel.CheeseType))
        {
            var searchCheeseType = (CheeseType)Enum.Parse(typeof(CheeseType), searchModel.CheeseType);
            searchRequest = searchRequest.Where(s => s.CheeseType == searchCheeseType);
        }

        var searchResponse = await searchRequest.ToListAsync();
        return searchResponse.Select(product => mapper.Map<CheesyProductModel>(product)).ToList();
    }
}