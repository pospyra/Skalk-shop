namespace Skalk.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public int INN { get; set; }

        public int KPP { get; set; }

        public string Address { get; set; }

        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
