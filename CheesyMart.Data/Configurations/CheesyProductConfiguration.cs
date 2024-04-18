using CheesyMart.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheesyMart.Data.Configurations;

public class CheesyProductConfiguration : IEntityTypeConfiguration<CheeseProduct>
{
    public void Configure(EntityTypeBuilder<CheeseProduct> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Color)
            .HasConversion<string>();
        
        builder.Property(t => t.CheeseType)
            .HasConversion<string>();
    }
}