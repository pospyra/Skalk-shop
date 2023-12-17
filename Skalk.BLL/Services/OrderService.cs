using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Skalk.BLL.Interfaces;
using Skalk.BLL.Services.Abstract;
using Skalk.Common.DTO.Order;
using Skalk.DAL.Entities;
using Skalk.DocumentGeneration.Interfaces;

namespace Skalk.BLL.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IDocumentGenerationService _documentGenerationService;
        public OrderService(SkalkContext context,
            IShoppingCartService shoppingCartService,
            IDocumentGenerationService documentGenerationService,
            IMapper mapper,
            IUserService userService,
            IProductService productService,

            IMemoryCache cache) : base(context, mapper)
        {
            _documentGenerationService = documentGenerationService;
            _userService = userService;
            _cache = cache;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO newOrderDTO)
        {
            var order = _mapper.Map<Order>(newOrderDTO);

            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var cart = await _context.ShoppingCarts
                .Include(s=>s.ItemShoppingCarts)
                .FirstOrDefaultAsync(x=>x.UserId == currentUserId);

            order.UserId = currentUserId;
            order.ItemsOrder = cart.ItemShoppingCarts.Select(itemCart => new ItemOrder
            {
                Mpn = itemCart.Mpn,
                OfferId = itemCart.OfferId,
                ClickUrl = itemCart.ClickUrl,
                Quantity = itemCart.Quantity,
                Price = itemCart.Price,
                TotalAmount = itemCart.TotalAmount
            }).ToList();


            _context.Orders.Add(order);
            await _context.SaveChangesAsync();


            var orderContractDTO = _mapper.Map<OrderContractDTO>(newOrderDTO);
            orderContractDTO.Id = order.Id;
            orderContractDTO.ItemsOrder = _mapper.Map<ICollection<ItemOrderDTO>>(order.ItemsOrder);
            orderContractDTO.CreatedAt = order.CreatedAt;
            var contractData = await _documentGenerationService.CreateOrderAsync(orderContractDTO);

            var orderContract = new OrderContract
            {
                OrderId = order.Id,
                Contract = contractData
            };

            _context.OrderContract.Add(orderContract);
            await _context.SaveChangesAsync();

            await _shoppingCartService.ClearShoppingCart();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderContract> GetOrderContractByIdAsync(int orderId)
        {
            var contract = await _context.OrderContract
                .FirstOrDefaultAsync(x=>x.OrderId == orderId);

            return contract;
        }

        public async Task<ICollection<OrderDTO>> GetOrderByCurrentUserAsync()
        {
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var orders = await _context.Orders
                .Include(i=>i.ItemsOrder)
                .Where(x => x.UserId == currentUserId).ToListAsync();

            return _mapper.Map<ICollection<OrderDTO>>(orders);
        }

        public async Task<ICollection<OrderDTO>> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o=>o.ItemsOrder)
                .Include(o=>o.User)
                .ToListAsync();

            return _mapper.Map<ICollection<OrderDTO>>(orders);
        }
    }
}
