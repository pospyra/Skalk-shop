using Skalk.Common.DTO.ShoppingCart;
using Skalk.DAL.Entities;

namespace Skalk.Common.DTO.Order
{
    public class OrderContractDTO
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? INN { get; set; }
        public string? KPP { get; set; }
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } 

        public ICollection<ItemOrderDTO>? ItemsOrder { get; set; }
    }
}
