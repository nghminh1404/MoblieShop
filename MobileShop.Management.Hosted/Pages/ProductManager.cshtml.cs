using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class ProductManagerModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string CategoryKey = "_category";
        public List<Product>? Products { get; set; }
        public List<Category> Categories { get; set; }

        public ProductManagerModel()
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
                HttpContext.Session.SetString(CategoryKey, string.Empty);

                if (!Request.Query["categoryid"].Equals(string.Empty))
                {
                    var categoryId = Request.Query["categoryid"].ToString();
                    /*
                    var response3 = await _client.GetAsync(ApiUri + $"product/get-product-category/{categoryId}");
                    var strData3 = await response3.Content.ReadAsStringAsync();
                    */
                    var response4 = await _client.GetAsync(ApiUri + $"category/get-all-category");
                    var strData4 = await response4.Content.ReadAsStringAsync();

                    var response5 = await _client.GetAsync(ApiUri + $"category/get-category-id/{categoryId}");
                    var strData5 = await response5.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString(CategoryKey, strData5);

                    Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData4, option);
                    /*
                    Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData3, option);
                    */
                    //  return Page();
                }

                var response = await _client.GetAsync(ApiUri + $"product/get-all-product");
                var response2 = await _client.GetAsync(ApiUri + $"category/get-all-category");
                var strData = await response.Content.ReadAsStringAsync();
                var strData2 = await response2.Content.ReadAsStringAsync();

                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData2, option);
                Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData, option);

                return Page();
            }
            catch (Exception e)
            {
                var response = await _client.GetAsync(ApiUri + $"product/get-all-product");
                var response2 = await _client.GetAsync(ApiUri + $"category/get-all-category");
                var strData = await response.Content.ReadAsStringAsync();
                var strData2 = await response2.Content.ReadAsStringAsync();
                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData2, option);
                Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData, option);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostFillter()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var json = HttpContext.Session.GetString(CategoryKey) ?? string.Empty;

            var productName = Request.Form["nameproduct"];

            if (string.IsNullOrEmpty(productName) && string.IsNullOrEmpty(json))
            {
                var response = await _client.GetAsync(ApiUri + $"product/get-all-product");
                var response2 = await _client.GetAsync(ApiUri + $"category/get-all-category");
                var strData = await response.Content.ReadAsStringAsync();
                var strData2 = await response2.Content.ReadAsStringAsync();

                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData2, option);
                Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData, option);
                return Page();
            }
            else if (string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(json))
            {
                var category = JsonConvert.DeserializeObject<Category>(json);

                var response = await _client.GetAsync(ApiUri + $"category/get-all-category");
                var strData = await response.Content.ReadAsStringAsync();

                var response2 = await _client.GetAsync(ApiUri + $"product/get-product-category/{category.CategoryId}");
                var strData2 = await response2.Content.ReadAsStringAsync();

                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData, option);
                Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData2, option);
                return Page();
            }
            else if (!string.IsNullOrEmpty(productName) && string.IsNullOrEmpty(json))
            {

                var response = await _client.GetAsync(ApiUri + $"category/get-all-category");
                var strData = await response.Content.ReadAsStringAsync();

                var response2 = await _client.GetAsync(ApiUri + $"product/get-product-keyword/{productName}");
                var strData2 = await response2.Content.ReadAsStringAsync();

                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData, option);
                Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData2, option);
                return Page();
            }
            else
            {
                var category = JsonConvert.DeserializeObject<Category>(json);

                var response = await _client.GetAsync(ApiUri + $"category/get-all-category");
                var strData = await response.Content.ReadAsStringAsync();

                var response2 = await _client.GetAsync(ApiUri + $"product/get-product-keyword&category/{productName}&{category.CategoryId}");
                var strData2 = await response2.Content.ReadAsStringAsync();

                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData, option);
                Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(strData2, option);
                return Page();
            }

        }
    }
}