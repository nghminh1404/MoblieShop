using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class UpdateDiscountModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public string message { get; set; }
        public Coupon coupon { get; set; }

        public UpdateDiscountModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";

        }

        public async Task<IActionResult> OnGet()
        {

            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Login");
            }
            try
            {
                int idc = Convert.ToInt32(Request.Query["idc"].ToString());
                var response = await _client.GetAsync(ApiUri + $"coupon/get-coupon-id/{idc}");
                var strData = await response.Content.ReadAsStringAsync();
                coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData, option);
                return Page();
            }
            catch (Exception e)
            {
                message = "Update failded";
                return RedirectToPage("DiscountManager");

            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idc = Convert.ToInt32(Request.Form["iddiscount"]);

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _client.GetAsync(ApiUri + $"coupon/get-coupon-id/{idc}");
            var strData = await response.Content.ReadAsStringAsync();
            coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData, option);


            if (Request.Form["code"].Equals(string.Empty) || Request.Form["percent"].Equals(string.Empty)
                || Request.Form["expiration_date"].Equals(string.Empty))
            {
                message = "Please, fill all information!";
                return Page();
            }

            var date = DateTime.Now;
            var code = Request.Form["code"];
            var percent = 0;
            try
            {

                percent = Convert.ToInt32(Request.Form["percent"]);
            }
            catch (Exception e)
            {
                message = "Discount percent must is number";
            }
            try
            {
                date = Convert.ToDateTime(Request.Form["expiration_date"]);
            }
            catch (Exception e)
            {
                message = "Date invalid";
            }


            if (date < DateTime.Now)
            {
                message = "Expiration date must in future";
                return Page();
            }

            if (percent < 0 || percent > 100)
            {
                message = "Discount percent invalid";
                return Page();
            }

            try
            {
                var response2 = await _client.GetAsync(ApiUri + $"coupon/get-coupon-code/{code}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var discountCheck = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData2, option);

                if (discountCheck != null && discountCheck.CouponId != coupon.CouponId)
                {
                    message = "Discount code exist, try again";
                    return Page();
                }
                else
                {
                    coupon.Code = code;
                    coupon.DiscountPercent = percent;
                    coupon.ExpirationDate = date;

                    var jsonCoupon = System.Text.Json.JsonSerializer.Serialize(coupon);
                    var content = new StringContent(jsonCoupon, Encoding.UTF8, "application/json");
                    await _client.PutAsync(ApiUri + "coupon/put-coupon", content);
                }
            }
            catch (Exception ex)
            {
                coupon.Code = code;
                coupon.DiscountPercent = percent;
                coupon.ExpirationDate = date;

                var jsonCoupon = System.Text.Json.JsonSerializer.Serialize(coupon);
                var content = new StringContent(jsonCoupon, Encoding.UTF8, "application/json");
                await _client.PutAsync(ApiUri + "coupon/put-coupon", content);
            }

            return RedirectToPage("DiscountManager");

        }
    }
}
