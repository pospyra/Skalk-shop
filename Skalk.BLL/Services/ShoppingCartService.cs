using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Skalk.BLL.Interfaces;
using Skalk.BLL.Services.Abstract;
using Skalk.Common.DTO.Product;
using Skalk.Common.DTO.ShoppingCart;
using Skalk.DAL.Entities;

namespace Skalk.BLL.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IMemoryCache _cache;
        public ShoppingCartService(SkalkContext context, 
            IMapper mapper,
            IUserService userService,
            IProductService productService,
            IMemoryCache cache) : base(context, mapper)
        {
            _userService = userService;
            _productService = productService;
            _cache = cache;
        }

        public async Task AddItemToCartAsync(NewItemCartDTO newItemDto)
        {
            var shoppingCartId = await GetShoppingCartIdAsync();

            var isExistingItem = await _context.ItemShoppingCarts
                .AnyAsync(item => item.Mpn == newItemDto.Mpn && item.ShoppingCartId == shoppingCartId);

            if (isExistingItem)
            {
                throw new InvalidOperationException("The product is already in the cart");
            }

            var newItem = _mapper.Map<ItemShoppingCart>(newItemDto);
            newItem.ShoppingCartId = shoppingCartId;

            _context.ItemShoppingCarts.Add(newItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ShoppingCartDTO> GetShoppingCartAsync()
        {
            var currentUserId = await _userService.GetCurrentUserIdAsync();

            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.ItemShoppingCarts)
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId)
                ?? throw new Exception("User doesn't have a cart");

            //if (_cache.TryGetValue("Cart", out ShoppingCartDTO? cachedCart))
            //{
            //    if (cachedCart is not null)
            //    {
            //        return cachedCart;
            //    }
            //}

            List<ItemCartDTO> itemShoppingCartDTOs = new();

            decimal totalCartPrice = 0;
            foreach (var itemShoppingCart in shoppingCart.ItemShoppingCarts)
            {
                ProductDTO product = await _productService.GetProductByItemCart(itemShoppingCart.Mpn, itemShoppingCart.OfferId);

                var itemShoppingCartDTO = new ItemCartDTO    
                {
                    Id = itemShoppingCart.Id,
                    Product = product,
                    Quantity = itemShoppingCart.Quantity,
                    Price = FoundPricing(itemShoppingCart.Quantity, product.Offers.SelectMany(offer => offer.Prices).ToList()),
                    ShoppingCartId = itemShoppingCart.ShoppingCartId
                };

                itemShoppingCartDTO.TotalAmount = itemShoppingCartDTO.Quantity * itemShoppingCartDTO.Price;

                if(itemShoppingCartDTO.Price != itemShoppingCart.Price)
                {
                    itemShoppingCart.Price = itemShoppingCartDTO.Price;
                    itemShoppingCart.TotalAmount = itemShoppingCartDTO.TotalAmount;

                    _context.ItemShoppingCarts.Update(itemShoppingCart);
                    await _context.SaveChangesAsync();
                }

                itemShoppingCartDTOs.Add(itemShoppingCartDTO);
                totalCartPrice += itemShoppingCartDTO.TotalAmount;
            }

            var shoppingCartDto = _mapper.Map<ShoppingCartDTO>(shoppingCart);
            shoppingCartDto.ItemShoppingCarts = itemShoppingCartDTOs;
            shoppingCartDto.TotalPrice = totalCartPrice;

            //if (shoppingCartDto is not null)
            //{
            //    var cacheOptions = new MemoryCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120)
            //    };

            //    _cache.Set("Cart", shoppingCartDto, cacheOptions);
            //}


            return shoppingCartDto;
        }

        private decimal FoundPricing(int selectedQuantity, List<PriceDTO> prices)
        {
            int lowerPriceIndex = 0;
            int higherPriceIndex = prices.Count - 1;

            for (int i = 0; i < prices.Count; i++)
            {
                if (prices[i].Quantity <= selectedQuantity)
                {
                    lowerPriceIndex = i;
                }
                else
                {
                    higherPriceIndex = i;
                    break;
                }
            }

            var lowerPrice = prices[lowerPriceIndex];

            return lowerPrice.PriceValue;
        }

        public async Task RemoveItemFromCartAsync(int itemId)
        {
            var shoppingCartId = await GetShoppingCartIdAsync();

            var item = await _context.ItemShoppingCarts
                .FirstOrDefaultAsync(i => i.ShoppingCartId == shoppingCartId
                && i.Id == itemId);

            if (item is not null)
            {
                _context.ItemShoppingCarts.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<int> GetShoppingCartIdAsync()
        {
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var shoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(x => x.UserId == currentUserId)
                ?? throw new Exception("User doesn't have a cart");

            return shoppingCart.Id;
        }
    }
}
