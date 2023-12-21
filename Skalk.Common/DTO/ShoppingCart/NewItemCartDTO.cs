namespace Skalk.Common.DTO.ShoppingCart
{
    public class NewItemCartDTO
    {
        public string Mpn { get; set; } = string.Empty;

        public int OfferId { get; set; }

        public string? ClickUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
