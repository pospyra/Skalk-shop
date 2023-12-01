using Microsoft.AspNetCore.Mvc;
using Skalk.BLL.Interfaces;
using Skalk.Common.DTO.Product;

namespace SkalkWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<ProductDTO>> GetProductAsync(string itemName)
        {
            var items = await _productService.GetProductsByFilters(itemName);

            return Ok(items);
        }
    }
}
