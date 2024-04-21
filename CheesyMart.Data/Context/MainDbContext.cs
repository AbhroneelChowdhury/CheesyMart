using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CheesyMart.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CheesyMart.Data.Context;

public partial class MainDbContext : DbContext
{
    public DbSet<CheeseProduct> CheeseProducts { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       base.OnConfiguring(optionsBuilder);
       optionsBuilder.LogTo(s =>
       {
           //Debug.WriteLine(s);
       });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
