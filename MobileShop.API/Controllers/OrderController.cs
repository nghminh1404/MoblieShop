using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.OrderDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add-order")]
        public IActionResult AddOrder([FromBody] CreateOrderRequest order)
        {
            var result = _orderService.AddOrder(order);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpGet("get-order-id/{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound("Order does not exist");
            }
            return Ok(order);
        }

        [HttpGet("get-order-CustomerId/{id}")]
        public IActionResult GetOrderByCustomerId(int id)
        {
            var order = _orderService.GetOrderZeroStatusByCustomerId(id);
            if (order == null)
            {
                return NotFound("Order does not exist");
            }
            return Ok(order);
        }

        [HttpGet("get-order-GuestId/{id}")]
        public IActionResult GetOrderByGuestId(int id)
        {
            var order = _orderService.GetOrderByGuestId(id);
            if (order == null)
            {
                return NotFound("Order does not exist");
            }
            return Ok(order);
        }

        [HttpGet("get-all-order")]
        public IActionResult GetAllOrder()
        {
            var order = _orderService.GetAllOrders();
            if (order == null)
            {
                return NotFound("Don't have order");
            }
            return Ok(order);
        }

        [HttpGet("get-orders-checkout/{cid}")]
        public IActionResult GetAllOrderCheckoutByCustomerID(int cid)
        {
            var order = _orderService.GetOrderCheckoutByCustomerId(cid);
            if (order == null)
            {
                return NotFound("Don't have order");
            }
            return Ok(order);
        }

        [HttpGet("get-orders-cutomername/{keyName}")]
        public IActionResult GetAllOrdersByCustomerName(string keyName)
        {
            var order = _orderService.GetAllOrdersByCustomerName(keyName);
            if (order == null)
            {
                return NotFound("Don't have order");
            }
            return Ok(order);
        }

        [HttpPut("put-order")]
        public IActionResult UpdateOrder(UpdateOrderRequest order)
        {
            var result = _orderService.UpdateOrder(order);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpPut("delete-order/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var result = _orderService.UpdateDeleteStatusOrder(id);
            if (result == false)
            {
                return StatusCode(500);
            }
            return Ok("Delete order complete");
        }

    }
}
