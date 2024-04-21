using CheesyMart.Core.CommandModels;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CheesyMart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController(IProductImageService productImageService,
        ILogger<ProductImageController> logger) : ControllerBase
    {
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "ProductImage_Get")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var productImage = await productImageService.GetProductImage(id);
            return File(fileContents: productImage.Data, productImage.MimeType);
        }


        [HttpPost]
        [SwaggerOperation(OperationId = "ProductImage_Create")]
        [ProducesResponseType(typeof(ProductImageModel), StatusCodes.Status200OK)]
        public async Task<ProductImageModel> Create([FromBody] ProductImageCommandModel productImageCommandModel)
        {
            return await productImageService.AddProductImage(productImageCommandModel);
        }

        
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "ProductImage_Delete")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await productImageService.DeleteProductImage(id);
            return Ok();
        }
    }
}
