using CheesyMart.Core.CommandModels;
using CheesyMart.Core.DomainModels;

namespace CheesyMart.Core.Interfaces;

public interface IProductImageService
{
    Task<ProductImageModel> AddProductImage(ProductImageCommandModel productImageModel);
    
    Task DeleteProductImage(int id);
    
    Task<ProductImageModel> GetProductImage(int id);
}