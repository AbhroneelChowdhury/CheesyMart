using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using CheesyMart.Core.MappingProfiles;
using CheesyMart.Data.Context;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CheesyMart.Test.Unit.Utils;

public abstract class UnitTestBase
{
    protected IFixture Fixture { get; }
    protected IMapper _mapper;

    protected UnitTestBase()
    {
        var autoMoqCustomization = new AutoMoqCustomization();
        Fixture = new Fixture().Customize(autoMoqCustomization);

        var db = DatabaseHelper.GenerateDatabase();
        var configuration = new MapperConfiguration(cfg => {
            cfg.AddProfile(new CheesyProductMappingProfile());
            cfg.AddProfile(new ProductImageMappingProfile());
        });
        _mapper = configuration.CreateMapper();
        Fixture.Inject(_mapper);
        Fixture.Inject(db);
    }

    protected MainDbContext GetDbContext()
    {
        var db = Fixture.Create<MainDbContext>();
        var mock = new Mock<IDbContextFactory<MainDbContext>>();
        mock.Setup(m => m.CreateDbContext()).Returns(db);
        return db;
    }
}

