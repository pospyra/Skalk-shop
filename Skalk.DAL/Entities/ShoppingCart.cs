using System.ComponentModel.DataAnnotations.Schema;

namespace Skalk.DAL.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public ICollection<ItemShoppingCart> ItemShoppingCarts { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
