using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class OrderManagerModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string OrderKey = "_order";
        public string message { get; set; }
        public List<Order>? Orders { get; set; }

        public OrderManagerModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }
        /*status
         * 0: add to cart
         * 1: checkout
         * 2: shipping
         * 3: complete
         */
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

            if (!Request.Query["fstore"].ToString().Equals(""))
            {
                service = Request.Query["fstore"].ToString();
            }

            if (service.Equals("startship"))
            {
                var id = Convert.ToInt32(Request.Query["bid"].ToString());

                var response2 = await _client.GetAsync(ApiUri + $"order/get-order-id/{id}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData2, option);

                order.ShippingDate = DateTime.Now;
                order.Status = 2;

                var jsonOrder = System.Text.Json.JsonSerializer.Serialize(order);
                var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                await _client.PutAsync(ApiUri + $"order/put-order", content);

                message = "Start shiping!";
            }

            if (service.Equals("detailorder"))
            {
                var id = Convert.ToInt32(Request.Query["bid"].ToString());

                var response2 = await _client.GetAsync(ApiUri + $"order/get-order-id/{id}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData2, option);

                var jsonOrder = System.Text.Json.JsonSerializer.Serialize(order);

                HttpContext.Session.SetString(OrderKey, jsonOrder);

                return RedirectToPage("OrderDetails");
            }

            var response = await _client.GetAsync(ApiUri + $"order/get-all-order");
            var strData = await response.Content.ReadAsStringAsync();
            Orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(strData, option);

            return Page();
        }

        public async Task<IActionResult> OnPostFillter()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (string.IsNullOrEmpty(Request.Form["fname"]))
            {
                var response = await _client.GetAsync(ApiUri + $"order/get-all-order");
                var strData = await response.Content.ReadAsStringAsync();
                Orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(strData, option);
            }
            else
            {
                var response = await _client.GetAsync(ApiUri + $"order/get-orders-cutomername/{Request.Form["fname"]}");
                var strData = await response.Content.ReadAsStringAsync();
                Orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(strData, option);
            }

            return Page();
        }
    }
}
