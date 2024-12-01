using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class AddDiscountModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public string message { get; set; }

        public AddDiscountModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";

        }

        public async Task<IActionResult> OnGet()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (Request.Form["code"].Equals(string.Empty) || Request.Form["percent"].Equals(string.Empty)
                || Request.Form["expiration_date"].Equals(string.Empty))
            {
                message = "Please, fill all information!";
                return Page();
            }

            var date = Convert.ToDateTime(Request.Form["expiration_date"]);
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
                var response = await _client.GetAsync(ApiUri + $"coupon/get-coupon-code/{code}");
                var strData = await response.Content.ReadAsStringAsync();
                var discountCheck = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData, option);

                if (discountCheck != null)
                {
                    message = "Discount code exist, try again";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                var coupon = new Coupon
                {
                    Code = code,
                    DiscountPercent = percent,
                    ExpirationDate = date,
                    IsDeleted = false
                };

                var jsonCoupon = System.Text.Json.JsonSerializer.Serialize(coupon);
                var content = new StringContent(jsonCoupon, Encoding.UTF8, "application/json");
                await _client.PostAsync(ApiUri + $"coupon/add-coupon", content);
            }

            return RedirectToPage("DiscountManager");
        }
    }
}
