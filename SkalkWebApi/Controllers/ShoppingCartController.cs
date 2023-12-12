
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skalk.BLL.Interfaces;
using Skalk.Common.DTO.ShoppingCart;

namespace SkalkWebApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToCart(NewItemCartDTO item)
        {
            await _shoppingCartService.AddItemToCartAsync(item);
            return Ok();
        }

        [HttpDelete("{itemId}")]
        public async Task<ActionResult> RemoveItemFromCart(int itemId)
        {
            await _shoppingCartService.RemoveItemFromCartAsync(itemId);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartDTO>> GetShoppintCartCart()
        {
            var cart = await _shoppingCartService.GetShoppingCartAsync();
            return Ok(cart);
        }
    }
}
