namespace Skalk.Common.DTO.ShoppingCart
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<ItemCartDTO>? ItemShoppingCarts { get; set; }
    }
}
