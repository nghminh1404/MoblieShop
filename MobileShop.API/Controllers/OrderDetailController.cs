using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.OrderDetailDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{

    [Route("api/orderdetail")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost("add-orderdetail")]
        public IActionResult AddOrderDetail([FromBody] CreateOrderDetailRequest detail)
        {
            var result = _orderDetailService.AddOrderDetail(detail);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpGet("get-orderdetail-id/{oid}&{pid}")]
        public IActionResult GetOrderDetailByOrderIdAndProductId(int oid, int pid)
        {
            var detail = _orderDetailService.GetOrderDetailByOrderIdAndProductId(oid, pid);
            if (detail == null)
            {
                return NotFound("Order detail does not exist");
            }
            return Ok(detail);
        }

        [HttpGet("get-orderdetails-customerid/{customerid}")]
        public IActionResult GetOrderDetailsProcessByCustomerId(int customerid)
        {
            var details = _orderDetailService.GetOrderDetailsProcessByCustomerId(customerid);
            if (details == null)
            {
                return NotFound("Order detail does not exist");
            }
            return Ok(details);
        }

        [HttpGet("get-all-orderdetail")]
        public IActionResult GetAllOrderDetails()
        {
            var order = _orderDetailService.GetAllOrderDetails();
            if (order == null)
            {
                return NotFound("Don't have order detail");
            }
            return Ok(order);

        }
        [HttpGet("get-orderdetails-OrderID/{oid}")]
        public IActionResult GetOrderDetailsByOrderID(int oid)
        {
            var order = _orderDetailService.GetOrderDetailsByOrderID(oid);
            if (order == null)
            {
                return NotFound("Don't have order detail");
            }
            return Ok(order);
        }


        [HttpPut("put-orderdetail")]
        public IActionResult UpdateOrderDetail(UpdateOrderDetailRequest detail)
        {
            var result = _orderDetailService.UpdateOrderDetail(detail);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpPut("put-delete-status/{oid}&{pid}")]
        public IActionResult UpdateDeleteStatusOrderDetail(int oid, int pid)
        {
            var result = _orderDetailService.UpdateDeleteStatusOrderDetail(oid, pid);
            if (result == false)
            {
                return StatusCode(500);
            }
            return Ok("Delete order detail complete");
        }

        [HttpDelete("delete-orderdetail/{oid}&{pid}")]
        public IActionResult DeleteOrderDetail(int oid, int pid)
        {
            var result = _orderDetailService.DeleteOrderDetail(oid, pid);
            if (result == false)
            {
                return StatusCode(500);
            }
            return Ok("Delete order detail complete");
        }
    }
}
