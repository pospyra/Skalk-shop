using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skalk.BLL.Interfaces;
using Skalk.Common.DTO.Order;
using Skalk.DAL.Entities;

namespace SkalkWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
             _orderService = orderService;
        }

      //  [Authorize(Policy = "ManagerOnly")]
        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("by-current-user")]
        public async Task<ActionResult> GetOrdersByCurrenUser()
        {
            var orders = await _orderService.GetOrderByCurrentUserAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderDTO item)
        {
            await _orderService.CreateOrderAsync(item);
            return Ok();
        }

        [HttpGet("download-contract/{orderId}")]
        public async Task<IActionResult> DownloadContract(int orderId)
        {
            OrderContract orderContract = await _orderService.GetOrderContractByIdAsync(orderId);

            if (orderContract == null || orderContract.Contract == null || orderContract.Contract.Length == 0)
            {
                return NotFound("Contract not found");
            }

            return File(orderContract.Contract, "application/pdf", "contract.pdf");
        }
    }
}
