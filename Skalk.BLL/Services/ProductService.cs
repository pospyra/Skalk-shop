using Microsoft.Extensions.Caching.Memory;
using Skalk.BLL.Interfaces;
using Skalk.Common.DTO.Product;
using Skalk.Common.GraphQLQuery;
using Skalk.DAL.SupplyTypes;

namespace Skalk.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMemoryCache _cache;
        private readonly string clientId;
        private readonly string clientSecret;


        public ProductService(IMemoryCache cache)
        {
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

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120)
            };

            if (products.Any())
            {
                _cache.Set("Products", products, cacheOptions);
            }


            return products;
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
                        seller.Offers.Select(offer => new ProductDTO
                        {
                            Id = res.Part.Id,
                            Mpn = res.Part.Mpn,
                            ManufacturerName = res.Part.Manufacturer?.Name,
                            CompanyName = seller.Company?.Name,
                            Offers = offer.Prices
                                
                                .Select(price => new OfferDTO
                                {
                                    Id = offer.Id,
                                    ClickUrl = offer.ClickUrl,
                                    InventoryLevel = offer.InventoryLevel,
                                    Prices = new List<PriceDTO>
                                    {
                                        new PriceDTO
                                        {
                                            PriceValue = price.PriceValue,
                                            Currency = price.Currency,
                                            Quantity = price.Quantity
                                        }
                                    }
                                })
                                .ToList()
                        })
                    )).ToList();

            return products;
        }
    }
}