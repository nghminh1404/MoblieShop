using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.OrderDetailDTO;
using MobileShop.Entity.DTOs.OrderDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{
    [Route("api/shopping")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IValidateService _validateService;

        public ShoppingController(IProductService productService,
                                  IAccountService accountService,
                                  IOrderService orderService,
                                  IOrderDetailService orderDetailService,
                                  IValidateService validateService)
        {
            _productService = productService;
            _accountService = accountService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _validateService = validateService;
        }

        [HttpGet("addtocart/{customerId}&{productId}&{quantity}")]
        public IActionResult AddToCart(int customerId, int productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            var customer = _accountService.GetAccountById(customerId);
            var order = _orderService.GetOrderZeroStatusByCustomerId(customerId);
            if (product == null)
            {
                return NotFound();
            }

            if (order == null)
            {
                var newOrder = new CreateOrderRequest
                {
                    CustomerId = customerId,
                    Address = customer.Address,
                    CreateDate = Convert.ToDateTime(_validateService.DateNow()),
                    Status = 0,
                    PaymentId = 2, //defaul payment is cod
                    CouponId = 5, //defaul coupon is normal (sale 0%)
                    IsDeleted = false
                };
                _orderService.AddOrder(newOrder);
                order = _orderService.GetOrderZeroStatusByCustomerId(customerId);
                var detail = _orderDetailService.GetOrderDetailByOrderIdAndProductId(order.OrderId, productId);
                if (detail == null)
                {
                    var newDetail = new CreateOrderDetailRequest
                    {
                        OrderId = order.OrderId,
                        ProductId = productId,
                        Quantity = quantity,
                        IsDeleted = false
                    };
                    _orderDetailService.AddOrderDetail(newDetail);
                }
                else
                {
                    var newDetail = new UpdateOrderDetailRequest
                    {
                        OrderId = order.OrderId,
                        ProductId = productId,
                        Quantity = quantity + detail.Quantity,
                        IsDeleted = false
                    };
                    _orderDetailService.UpdateOrderDetail(newDetail);
                }
            }
            else
            {
                var detail = _orderDetailService.GetOrderDetailByOrderIdAndProductId(order.OrderId, productId);
                if (detail == null)
                {
                    var newDetail = new CreateOrderDetailRequest
                    {
                        OrderId = order.OrderId,
                        ProductId = productId,
                        Quantity = quantity,
                        IsDeleted = false
                    };
                    _orderDetailService.AddOrderDetail(newDetail);
                }
                else
                {
                    var newDetail = new UpdateOrderDetailRequest
                    {
                        OrderId = order.OrderId,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity + quantity,
                        IsDeleted = detail.IsDeleted
                    };
                    var s = _orderDetailService.UpdateOrderDetail(newDetail);
                    return Ok(s);
                }

            }
            return Ok();
        }


    }
}
