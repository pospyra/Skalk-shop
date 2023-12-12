using Skalk.Common.DTO.Product;

namespace Skalk.Common.DTO.ShoppingCart
{
    public class ItemCartDTO
    {
        public int Id { get; set; }

        public ProductDTO? Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalAmount { get; set; }


        public int ShoppingCartId { get; set; }
    }
}
