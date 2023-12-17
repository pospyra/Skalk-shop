namespace Skalk.DAL.Entities
{
    public class ItemOrder
    {
        public int Id { get; set; }
        public string Mpn { get; set; }

        public int OfferId { get; set; }

        public string ClickUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalAmount { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
