using Skalk.Common.DTO.Product;

namespace Skalk.BLL.Interfaces
{
    public interface IProductService
    {
        Task<ICollection<ProductDTO>> GetProductsByFilters(string itemName);
    }
}
