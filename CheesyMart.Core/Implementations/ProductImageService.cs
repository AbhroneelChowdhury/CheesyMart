using AutoMapper;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Interfaces;
using CheesyMart.Data.Context;
using CheesyMart.Data.Entities;
using CheesyMart.Infrastructure.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CheesyMart.Core.Implementations;

public class ProductImageService(MainDbContext mainDbContext,
    IValidator<ProductImageModel> validator, IMapper mapper) : IProductImageService
{
    public async Task<ProductImageModel> AddProductImage(ProductImageModel productImageModel)
    {
        await validator.ValidateAndThrowAsync(productImageModel);
        var productImage = new ProductImage
        {
            LastUpdated = DateTimeOffset.UtcNow,
            Data = Convert.FromBase64String(productImageModel.ImageData),
            AlternateText = productImageModel.AltText,
            CheeseProductId = productImageModel.CheesyProductId
        };
        await mainDbContext.ProductImages.AddAsync(productImage);
        await mainDbContext.SaveChangesAsync();
        productImageModel.Id = productImage.Id;
        return productImageModel;
    }

    public async Task DeleteProductImage(int id)
    {
        var imageRecord = await mainDbContext.ProductImages.FirstOrDefaultAsync(c => c.Id == id);
        if (imageRecord == null)
        {
            throw new NotFoundException("Requested image not found");
        }

        mainDbContext.ProductImages.Remove(imageRecord);
        await mainDbContext.SaveChangesAsync();
    }

    public async Task<ProductImageModel> GetProductImage(int id)
    {
        var imageRecord = await mainDbContext.ProductImages.FirstOrDefaultAsync(c => c.Id == id);
        if (imageRecord == null)
        {
            throw new NotFoundException("Requested image not found");
        }
        return mapper.Map<ProductImageModel>(imageRecord);
    }
}