using Skalk.Common.DTO.User;
using Skalk.DAL.Enums;

namespace Skalk.Common.DTO.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } 

        public int UserId { get; set; }
        public UserDTO? User { get; set; }
        public ICollection<ItemOrderDTO>? ItemsOrder { get; set; }


        public OrderStatus OrderStatus { get; set; }
    }
}
