using Microsoft.Extensions.Caching.Memory;
using Skalk.BLL.Interfaces;
using Skalk.BLL.IServices;
using Skalk.Common.DTO.Currency;
using Skalk.Common.DTO.Product;
using Skalk.Common.Filters;
using Skalk.Common.GraphQLQuery;
using Skalk.DAL.SupplyTypes;

namespace Skalk.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMemoryCache _cache;
        private readonly ICurrencyService _currencyService;

        public ProductService(IMemoryCache cache, ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _cache = cache;
        }

        public async Task<ProductDTO> GetProductByItemCart(string itemName, int offerId)
        {
            var productsList = await GetProductsByFilters(itemName);

            var product = productsList.FirstOrDefault(x => x.Offers.Any(o => o.Id == offerId));

            return product;
        }

        public async Task<ICollection<ProductDTO>> GetProductsByFilters(string itemName)
        {
            ICollection<ProductDTO> ProductDTO = new List<ProductDTO>();
            if (_cache.TryGetValue("ProductDTO", out ICollection<ProductDTO>? cachedCurrenies))
            {
                if (cachedCurrenies is not null)
                {
                    ProductDTO = cachedCurrenies;
                }
            }
            else
            {
                string query = Query.FindPricesQuery;

                using var supplyClient = new SupplyClient();

                var request = new Request
                {
                    Query = query,
                    Variables = new Dictionary<string, object>
                {
                    { "itemName", itemName },
                }
                };

                var result = await supplyClient.RunQueryAsync(request);

                ProductDTO = MapToProductDTO(result?.Data.SupSearch.Results.Select(r => r?.Part).ToList());

                if (ProductDTO.Any())
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120)
                    };

                    _cache.Set("ProductDTO", ProductDTO, cacheOptions);
                }
            }
            return ProductDTO;
        }


        private decimal CalculatePrice(string charCode, decimal price)
        {
            const decimal coefficient = 2;   
            var convertedPrice = ConvertionCurrency(charCode, price);
            var totalPriceBeforeRounding = convertedPrice * coefficient;
            var roundedTotalPrice = Math.Round(totalPriceBeforeRounding, 2, MidpointRounding.AwayFromZero);

            return roundedTotalPrice;
        }


        private decimal ConvertionCurrency(string charCode, decimal price)
        {
            ICollection<CurrencyDTO> currencies = new List<CurrencyDTO>();
            if (_cache.TryGetValue("Currencies", out ICollection<CurrencyDTO>? cachedCurrenies))
            {
                if (cachedCurrenies is not null)
                {
                    currencies = cachedCurrenies;
                }
            }
            else
            {
                currencies = _currencyService.GetCurrenciesAsync().Result;

            }

            if (currencies.Any())
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                };

                _cache.Set("Currencies", currencies, cacheOptions);
            }

            var currency = currencies.FirstOrDefault(x => x.CharCode == charCode);

            var convertedPrice = decimal.Parse(currency.VunitRate) * price;
            return convertedPrice;
        }

        private ICollection<ProductDTO> MapToProductDTO(List<Part>? results)
        {
            if (results == null)
            {
                return new List<ProductDTO>();
            }

            var products = results
                .Where(res => res?.Mpn != null)
                .SelectMany(res => res.Sellers.SelectMany(seller =>
                    seller.Offers.GroupBy(offer => offer.Id).Select(groupedOffer => new ProductDTO
                    {
                        Id = res.Id,
                        Mpn = res.Mpn,
                        ManufacturerName = res.Manufacturer?.Name,
                        Company = new CompanyDTO
                        {
                            Id = seller.Company.Id,
                            Name = seller.Company.Name,
                        },
                        Offers = groupedOffer.Select(offer =>
                            new OfferDTO
                            {
                                Id = offer.Id,
                                ClickUrl = offer.ClickUrl,
                                InventoryLevel = offer.InventoryLevel,
                                Moq = offer.Moq,
                                Prices = offer.Prices
                                    .Select(price => new PriceDTO
                                    {
                                        PriceValue = CalculatePrice(price.Currency, price.PriceValue),
                                        Currency = "RUB",
                                        Quantity = price.Quantity
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })))
                .ToList();

            var companyNamesToFilter = ProductDataFilter.CompanyNamesToFilter;
            var filteredProducts = products.Where(product => companyNamesToFilter.Contains(product?.Company?.Name)).ToList();
            return filteredProducts;
        }
    }
}