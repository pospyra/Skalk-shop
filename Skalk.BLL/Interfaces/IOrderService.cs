using Skalk.Common.DTO.Order;
using Skalk.DAL.Entities;

namespace Skalk.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO newOrderDTO);
        Task<ICollection<OrderDTO>> GetOrderByCurrentUserAsync();
        Task<OrderContract> GetOrderContractByIdAsync(int orderId);
        Task<ICollection<OrderDTO>> GetOrdersAsync();
    }

}
