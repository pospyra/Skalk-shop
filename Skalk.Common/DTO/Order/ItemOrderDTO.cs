namespace Skalk.Common.DTO.Order
{
    public class ItemOrderDTO
    {
        public int Id { get; set; }
        public string? Mpn { get; set; }

        public int OfferId { get; set; }

        public string? ClickUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalAmount { get; set; }

        public int OrderId { get; set; }
    }
}
