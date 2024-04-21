using CheesyMart.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheesyMart.Data.Configurations;

public class ProductImageDataConfiguration : IEntityTypeConfiguration<ProductImageData>
{
    public void Configure(EntityTypeBuilder<ProductImageData> builder)
    {
        builder.ToTable("ProductImages");

    }
}