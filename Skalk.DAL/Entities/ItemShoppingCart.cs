namespace Skalk.DAL.Entities
{
    public class ItemShoppingCart
    {
        public int Id { get; set; }

        public int ShoppingCartId { get; set; }

        /// <summary>
        /// ProductId from Nexar API
        /// </summary>
        public int ProductId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
