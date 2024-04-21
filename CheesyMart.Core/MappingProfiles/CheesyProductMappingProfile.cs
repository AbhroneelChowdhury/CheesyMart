using AutoMapper;
using CheesyMart.Core.DomainModels;
using CheesyMart.Data.Entities;

namespace CheesyMart.Core.MappingProfiles;

public class CheesyProductMappingProfile : Profile
{
    public CheesyProductMappingProfile()
    {
        CreateMap<CheeseProduct, CheesyProductModel>()
            .ForMember(m => m.ProductImages,
                opts =>
                    opts.MapFrom(src => src.Images.Select(i => i.Id).ToList()));

    }
}