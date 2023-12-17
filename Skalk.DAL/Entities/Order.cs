using Skalk.DAL.Enums;

namespace Skalk.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; } 
        public User User { get; set; }
        public ICollection<ItemOrder> ItemsOrder { get; set; }

        public int OrderContractId { get; set; }
        public OrderContract OrderContract { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
