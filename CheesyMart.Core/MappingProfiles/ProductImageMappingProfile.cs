using AutoMapper;
using CheesyMart.Core.DomainModels;
using CheesyMart.Data.Entities;

namespace CheesyMart.Core.MappingProfiles;

public class ProductImageMappingProfile : Profile
{
    public ProductImageMappingProfile()
    {
        CreateMap<ProductImage,ProductImageModel>();
    }
}