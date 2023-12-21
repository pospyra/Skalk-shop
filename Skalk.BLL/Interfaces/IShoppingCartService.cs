using Skalk.Common.DTO.ShoppingCart;

namespace Skalk.BLL.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddItemToCartAsync(NewItemCartDTO newItem);
        Task RemoveItemFromCartAsync(int productId);
        Task<ShoppingCartDTO> GetShoppingCartAsync();

        Task<ItemCartDTO> UpdateQuantityItem(UpdateItemCartDTO item);
        Task ClearShoppingCart();
    }
}
