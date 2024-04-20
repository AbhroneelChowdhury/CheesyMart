using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Interfaces;
using CheesyMart.Core.QueryModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CheesyMart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheeseProductController(ICheesyProductService cheesyProductService,
        ILogger<CheeseProductController> logger) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(OperationId = "CheesyProductCatalog_GetAll")]
        [ProducesResponseType(typeof(CheesyProductsModel), StatusCodes.Status200OK)]
        public async Task<CheesyProductsModel> Get(string? name, string? cheeseType, string? cheeseColor)
        {
            return await cheesyProductService.GetCheeseProductsInCatalog(new SearchCheesyProductCatalogModel
            {
                Color = cheeseColor,
                CheeseType = cheeseType,
                Name = name
            });
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "CheesyProductCatalog_Get")]
        [ProducesResponseType(typeof(CheesyProductModel), StatusCodes.Status200OK)]
        public async Task<CheesyProductModel> Get(int id)
        {
            return await cheesyProductService.GetCheeseProductInCatalog(id);
        }
        
        [HttpPost]
        [SwaggerOperation(OperationId = "CheesyProductCatalog_Create")]
        [ProducesResponseType(typeof(CheesyProductModel), StatusCodes.Status200OK)]
        public async Task<CheesyProductModel> Create([FromBody] CheesyProductModel cheesyProductModel)
        {
            return await cheesyProductService.AddCheeseProductToCatalog(cheesyProductModel);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(OperationId = "CheesyProductCatalog_Update")]
        [ProducesResponseType(typeof(CheesyProductModel), StatusCodes.Status200OK)]
        public async Task<CheesyProductModel> Put(int id, [FromBody] CheesyProductModel cheesyProductModel)
        {
            return await cheesyProductService.UpdateCheeseProductInCatalog(cheesyProductModel);
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "CheesyProductCatalog_Delete")]
        [ProducesResponseType(typeof(CheesyProductModel), StatusCodes.Status200OK)]
        public async Task<CheesyProductModel> Delete(int id)
        {
            return await cheesyProductService.DeleteCheeseProductInCatalog(id);
        }
    }
}
