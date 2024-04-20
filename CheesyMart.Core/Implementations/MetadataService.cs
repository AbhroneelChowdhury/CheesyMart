using CheesyMart.Core.Interfaces;
using CheesyMart.Core.QueryModels;
using CheesyMart.Data.Enums;
using CheesyMart.Infrastructure.Exceptions;

namespace CheesyMart.Core.Implementations;

public class MetadataService : IMetadataService
{
    public Task<MetadataModel> GetMetadataByType(string type)
    {
        return Task.FromResult(type switch
        {
            nameof(CheeseColor) => new MetadataModel { Items = Enum.GetNames(typeof(CheeseColor)), Type = type, },
            nameof(CheeseType) => new MetadataModel { Items = Enum.GetNames(typeof(CheeseType)), Type = type },
            _ => throw new CheesyMartSystemValidationException("Metadata type invalid")
        });
    }
}