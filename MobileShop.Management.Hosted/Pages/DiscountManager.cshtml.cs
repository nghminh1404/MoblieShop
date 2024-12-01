using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class DiscountManagerModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public List<Coupon> coupons { get; set; }

        public DiscountManagerModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var service = string.Empty;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Login");
            }

            if (!Request.Query["fstore"].ToString().Equals(string.Empty))
            {
                service = Request.Query["fstore"].ToString();
            }

            if (service.Equals("unavailable"))
            {
                try
                {
                    var idc = Convert.ToInt32(Request.Query["idc"].ToString());

                    var response2 = await _client.GetAsync(ApiUri + $"coupon/get-coupon-id/{idc}");
                    var strData2 = await response2.Content.ReadAsStringAsync();
                    var couponPut = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData2, option);

                    couponPut.IsDeleted = true;

                    var jsonCoupon = System.Text.Json.JsonSerializer.Serialize(couponPut);
                    var content = new StringContent(jsonCoupon, Encoding.UTF8, "application/json");
                    await _client.PutAsync(ApiUri + "coupon/put-coupon", content);
                }
                catch (Exception e)
                {

                }
            }

            var response = await _client.GetAsync(ApiUri + $"coupon/get-all-coupon");
            var strData = await response.Content.ReadAsStringAsync();
            coupons = System.Text.Json.JsonSerializer.Deserialize<List<Coupon>>(strData, option);

            return Page();

        }

        public async Task<IActionResult> OnPostFillter()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (string.IsNullOrEmpty(Request.Form["codeDiscount"]))
            {
                var response = await _client.GetAsync(ApiUri + $"coupon/get-all-coupon");
                var strData = await response.Content.ReadAsStringAsync();
                coupons = System.Text.Json.JsonSerializer.Deserialize<List<Coupon>>(strData, option);
            }
            else
            {
                var response = await _client.GetAsync(ApiUri + $"coupon/get-coupons-key/{Request.Form["codeDiscount"]}");
                var strData = await response.Content.ReadAsStringAsync();
                coupons = System.Text.Json.JsonSerializer.Deserialize<List<Coupon>>(strData, option);
            }

            return Page();
        }
    }
}
