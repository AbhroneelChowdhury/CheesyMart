using CheesyMart.Core.DomainModels;

namespace CheesyMart.Core.QueryModels;

public class CheesyProductsModel
{
    public IList<CheesyProductModel> Products { get; init; }
}