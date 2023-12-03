using Microsoft.Extensions.Caching.Memory;
using Skalk.BLL.Interfaces;
using Skalk.BLL.IServices;
using Skalk.Common.DTO.Currency;
using Skalk.Common.DTO.Product;
using Skalk.Common.GraphQLQuery;
using Skalk.DAL.SupplyTypes;

namespace Skalk.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMemoryCache _cache;
        private readonly ICurrencyService _currencyService;

        private readonly string clientId;
        private readonly string clientSecret;


        public ProductService(IMemoryCache cache, ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _cache = cache;

            clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID")
                ?? throw new InvalidOperationException("Please set environment variable 'NEXAR_CLIENT_ID'");
            clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET")
                ?? throw new InvalidOperationException("Please set environment variable 'NEXAR_CLIENT_SECRET'");
        }


        public async Task<ICollection<ProductDTO>> GetProductsByFilters(string itemName)
        {
            if (_cache.TryGetValue("Products", out ICollection<ProductDTO>? cachedProducts))
            {
                if (cachedProducts is not null)
                {
                    return cachedProducts;
                }
            }

            string query = Query.FindPricesQuery;
            
            using var supplyClient = new SupplyClient(clientId, clientSecret);

            var request = new Request
            {
                Query = query,
                Variables = new Dictionary<string, object>
                {
                    { "itemName", itemName },
                }
            };

             var result = await supplyClient.RunQueryAsync(request);

            var products = MapToProductDTO(result?.Data?.SupSearch?.Results);


            if (products.Any())
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120)
                };

                _cache.Set("Products", products, cacheOptions);
            }

            return products;
        }

        private decimal ConvertionCurrency(string charCode, decimal price)
        {
            ICollection<CurrencyDTO> currencies = new List<CurrencyDTO>();
            if (_cache.TryGetValue("Currencies", out ICollection<CurrencyDTO>? cachedCurrenies))
            {
                if (cachedCurrenies is not null)
                {
                    currencies =  cachedCurrenies;
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

            var currency = currencies.FirstOrDefault(x=>x.CharCode == charCode);

            var convertedPrice = decimal.Parse(currency.VunitRate) * price;
            return convertedPrice;
        }

        private ICollection<ProductDTO> MapToProductDTO(List<Result>? results)
        {
            if (results == null)
            {
                return new List<ProductDTO>();
            }

            var products = results
                .Where(res => res?.Part?.Mpn != null)
                .SelectMany(res => res.Part.Sellers.SelectMany(seller =>
                    seller.Offers.GroupBy(offer => offer.Id).Select(groupedOffer => new ProductDTO
                    {
                        Id = res.Part.Id,
                        Mpn = res.Part.Mpn,
                        ManufacturerName = res.Part.Manufacturer?.Name,
                        CompanyName = seller.Company?.Name,
                        Offers = groupedOffer.Select(offer =>
                            new OfferDTO
                            {
                                Id = offer.Id,
                                ClickUrl = offer.ClickUrl,
                                InventoryLevel = offer.InventoryLevel,
                                Prices = offer.Prices
                                    .Select(price => new PriceDTO
                                    {
                                        PriceValue = ConvertionCurrency(price.Currency, price.PriceValue),
                                        Currency = "RUB",
                                        Quantity = price.Quantity
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })))
                .ToList();

            return products;
        }
    }
}