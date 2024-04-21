using AutoMapper;
using CheesyMart.Core.DomainModels;
using CheesyMart.Data.Entities;

namespace CheesyMart.Core.MappingProfiles;

public class ProductImageMappingProfile : Profile
{
    public ProductImageMappingProfile()
    {
        CreateMap<ProductImage,ProductImageModel>()
            .ForMember(dest => dest.MimeType, opts =>
                opts.MapFrom(src => src.ProductImageData.MimeType))
            .ForMember(dest => dest.Data, opts =>
                opts.MapFrom(src => src.ProductImageData.Data))
            .ForMember(dest => dest.AlternateText, opts =>
                opts.MapFrom(src => src.ProductImageData.AlternateText));

    }
}