using CheesyMart.Core.QueryModels;

namespace CheesyMart.Core.Interfaces;

public interface IMetadataService
{
    Task<MetadataModel> GetMetadataByType(string type);
}