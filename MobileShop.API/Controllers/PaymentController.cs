using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.PaymentDTO;
using MobileShop.Service;


namespace MobileShop.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("add-payment")]
        public IActionResult AddPayment([FromBody] CreatePaymentRequest payment)
        {
            var result = _paymentService.AddPayment(payment);

            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpGet("get-payment-id/{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);

            if (payment == null)
            {
                return NotFound("Payment does not exist");
            }
            return Ok(payment);
        }

        [HttpGet("get-all-payment")]
        public IActionResult GetAllPaymentBy()
        {
            var payments = _paymentService.GetAllPayment();

            if (payments == null)
            {
                return NotFound("Payment does not exist");
            }
            return Ok(payments);
        }
        [HttpPut("put-payment")]
        public IActionResult UpdatePayment(UpdatePaymentRequest payment)
        {
            var result = _paymentService.UpdatePayment(payment);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpPut("delete-payment/{id}")]
        public IActionResult DeletePayment(int id)
        {
            var result = _paymentService.UpdateDeleteStatusPayment(id);
            if (result == false)
            {
                return StatusCode(500);
            }
            return Ok("Delete payment complete");
        }

    }
}
