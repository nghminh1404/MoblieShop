using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class OrderDetailsModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string OrderKey = "_order";
        public List<OrderDetail> orderDetails { get; set; }

        public OrderDetailsModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var jsonOder = HttpContext.Session.GetString(OrderKey) ?? string.Empty;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Login");
            }
            var order = System.Text.Json.JsonSerializer.Deserialize<Order>(jsonOder);

            var response = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-OrderID/{order.OrderId}");
            var strData = await response.Content.ReadAsStringAsync();
            orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData, option);

            return Page();
        }
    }
}
