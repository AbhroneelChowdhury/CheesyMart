using CheesyMart.Core.Interfaces;
using CheesyMart.Core.QueryModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace CheesyMart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController(IMetadataService metadataService) : ControllerBase
    {
        [HttpGet("{type}")]
        [SwaggerOperation(OperationId = "Metadata_GetValuesByType")]
        [ProducesResponseType(typeof(MetadataModel), StatusCodes.Status200OK)]
        public async Task<MetadataModel> Get(string type)
        {
            return await metadataService.GetMetadataByType(type);
        }
    }
}
