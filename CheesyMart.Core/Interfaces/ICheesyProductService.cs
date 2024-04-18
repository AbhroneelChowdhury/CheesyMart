using CheesyMart.Core.DomainModels;
using CheesyMart.Core.QueryModels;

namespace CheesyMart.Core.Interfaces;

public interface ICheesyProductService
{
    Task<CheesyProductModel> AddCheeseProductToCatalog(CheesyProductModel cheesyProductModel);
    
    Task<CheesyProductModel> UpdateCheeseProductInCatalog(CheesyProductModel cheesyProductModel);
    
    Task<CheesyProductModel> DeleteCheeseProductInCatalog(int id);
    
    Task<CheesyProductModel> GetCheeseProductInCatalog(int id);
    
    
    Task<List<CheesyProductModel>> GetCheeseProductsInCatalog(SearchCheesyProductCatalogModel searchModel);
}