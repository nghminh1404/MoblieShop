using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.CouponDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost("add-coupon")]
        public IActionResult AddAccount([FromBody] CreateCouponRequest coupon)
        {
            var result = _couponService.AddCoupon(coupon);

            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpGet("get-coupon-id/{id}")]
        public IActionResult GetCouponById(int id)
        {
            var coupon = _couponService.GetCouponById(id);

            if (coupon == null)
            {
                return NotFound("Coupon does not exist");
            }
            return Ok(coupon);
        }

        [HttpGet("get-all-coupon-id/{id}")]
        public IActionResult GetCouponById2(int id)
        {
            var coupon = _couponService.GetCouponById2(id);

            if (coupon == null)
            {
                return NotFound("Coupon does not exist");
            }
            return Ok(coupon);
        }


        [HttpGet("get-coupon-code/{code}")]
        public IActionResult GetCouponByCode(string code)
        {
            var coupon = _couponService.GetCouponByCode(code);

            if (coupon == null)
            {
                return NotFound("Coupon does not exist");
            }
            return Ok(coupon);
        }

        [HttpGet("get-all-coupon")]
        public IActionResult GetallCouponBy()
        {
            var coupons = _couponService.GetALLCoupon();

            if (coupons == null)
            {
                return NotFound("Coupon does not exist");
            }
            return Ok(coupons);
        }

        [HttpGet("get-coupons-key/{key}")]
        public IActionResult GetCouponByKey(string key)
        {
            var coupons = _couponService.GetCouponByKey(key);

            if (coupons == null)
            {
                return NotFound("Coupon does not exist");
            }
            return Ok(coupons);
        }

        [HttpPut("put-coupon")]
        public IActionResult UpdateCoupon(UpdateCouponRequest coupon)
        {
            var result = _couponService.UpdateCoupon(coupon);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpPut("delete-coupon/{id}")]
        public IActionResult DeleteCoupon(int id)
        {
            var result = _couponService.UpdateDeleteStatusCoupon(id);
            if (result == false)
            {
                return StatusCode(500);
            }
            return Ok("Delete coupon complete");
        }
    }
}
