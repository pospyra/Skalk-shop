using Microsoft.AspNetCore.Mvc;
using Skalk.BLL.Interfaces;
using Skalk.BLL.IServices;
using Skalk.Common.DTO.Product;

namespace SkalkWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICurrencyService _currencyService;

        public ProductController(IProductService productService, ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<ProductDTO>> GetProductAsync(string itemName)
        {
            var items = await _productService.GetProductsByFilters(itemName);

            return Ok(items);
        }
        //[HttpGet]
        //public async Task<ActionResult> GetCurrencyAsync()
        //{
        //    var items = await _currencyService.GetCurrenciesAsync();

        //    return Ok(items);
        //}
    }
}
