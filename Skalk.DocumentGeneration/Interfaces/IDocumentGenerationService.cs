using Skalk.Common.DTO.Order;

namespace Skalk.DocumentGeneration.Interfaces
{
    public interface IDocumentGenerationService
    {
        Task<byte[]> CreateOrderAsync(OrderContractDTO newOrderDTO);
    }
}
