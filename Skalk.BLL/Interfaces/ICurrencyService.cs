using Skalk.Common.DTO.Currency;

namespace Skalk.BLL.IServices
{
    public interface ICurrencyService 
    {
        Task<ICollection<CurrencyDTO>> GetCurrenciesAsync();
    }
}
