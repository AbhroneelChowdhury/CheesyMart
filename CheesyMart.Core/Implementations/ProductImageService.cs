using AutoMapper;
using CheesyMart.Core.CommandModels;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Interfaces;
using CheesyMart.Data.Context;
using CheesyMart.Data.Entities;
using CheesyMart.Infrastructure.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CheesyMart.Core.Implementations;

public class ProductImageService(MainDbContext mainDbContext,
    IValidator<ProductImageCommandModel> validator, IMapper mapper) : IProductImageService
{
    public async Task<ProductImageModel> AddProductImage(ProductImageCommandModel productImageCommandModel)
    {
        await validator.ValidateAndThrowAsync(productImageCommandModel);
        var productImage = new ProductImage
        {
            LastUpdated = DateTimeOffset.UtcNow,
            ProductImageData = new ProductImageData
            {
                AlternateText = productImageCommandModel.AlternateText,
                MimeType = productImageCommandModel.MimeType,
                Data = Convert.FromBase64String(productImageCommandModel.Data),
            },
            CheeseProductId = productImageCommandModel.CheesyProductId
        };
        await mainDbContext.ProductImages.AddAsync(productImage);
        await mainDbContext.SaveChangesAsync();
        return mapper.Map<ProductImageModel>(productImage);
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
        var imageRecord = await mainDbContext.ProductImages
            .Include(p => p.ProductImageData).FirstOrDefaultAsync(c => c.Id == id);
        if (imageRecord == null)
        {
            throw new NotFoundException("Requested image not found");
        }
        return mapper.Map<ProductImageModel>(imageRecord);
    }
}