using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.User.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public List<Product>? Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }

        public async Task<IActionResult> OnGet()
        {

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var service = string.Empty;
            if (!Request.Query["fstore"].ToString().Equals(string.Empty))
            {
                service = Request.Query["fstore"].ToString();
            }

            if (service.Equals("AddToCart") && !string.IsNullOrEmpty(json))
            {

                var account = JsonConvert.DeserializeObject<Account>(json);
                int productId = Convert.ToInt32(Request.Query["productid"]);
                int quantity = 1;
                await _client.GetAsync(ApiUri + $"shopping/addtocart/{account.AccountId}&{productId}&{quantity}");
            }
            var response = await _client.GetAsync(ApiUri + $"product/get-all-product");
            var strData = await response.Content.ReadAsStringAsync();
            Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData, option);

            return Page();

        }
    }
}
