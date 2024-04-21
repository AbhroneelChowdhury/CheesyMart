using CheesyMart.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheesyMart.Data.Configurations;

public class ProductImageConfiguration: IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasOne(p => p.ProductImageData)
            .WithOne()
            .HasForeignKey<ProductImageData>(p => p.Id);

        builder.Navigation(p => p.ProductImageData).IsRequired();

    }
}