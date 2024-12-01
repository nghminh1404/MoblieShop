using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.User.Hosted.Pages
{
    public class PurchaseModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string GuestKey = "_guest";
        public List<Order> orders { get; set; }

        public PurchaseModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var jsonGuest = HttpContext.Session.GetString(GuestKey) ?? string.Empty;
            var service = string.Empty;
            Account account = null;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!string.IsNullOrEmpty(json))
            {
                account = JsonConvert.DeserializeObject<Account>(json);

            }
            else
            {
                if (!string.IsNullOrEmpty(jsonGuest))
                {
                    account = JsonConvert.DeserializeObject<Account>(jsonGuest);

                }
                else
                {
                    return RedirectToPage("Shop");

                }
            }

            if (!Request.Query["fstore"].ToString().Equals(string.Empty))
            {
                service = Request.Query["fstore"].ToString();
            }

            if (service.Equals("Confirm"))
            {
                int bid = Convert.ToInt32(Request.Query["bid"].ToString());

                var response2 = await _client.GetAsync(ApiUri + $"order/get-order-id/{bid}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData2, option);

                order.RequiredDate = DateTime.Now;
                order.Status = 3;

                var jsonOrder = System.Text.Json.JsonSerializer.Serialize(order);
                var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                await _client.PutAsync(ApiUri + $"order/put-order", content);

            }

            var response = await _client.GetAsync(ApiUri + $"order/get-orders-checkout/{account.AccountId}");
            var strData = await response.Content.ReadAsStringAsync();
            orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(strData, option);

            return Page();

        }
    }
}
