using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.DTOs.CartDTO;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.User.Hosted.Pages
{
    public class ShopModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string CategoryKey = "_category";
        private string LoginKey = "_login";
        private string CartKey = "_cart";
        public List<Product>? Products { get; set; }
        public List<Category> Categories { get; set; }

        public ShopModel()
        {
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
            var service = String.Empty;
            try
            {
                HttpContext.Session.SetString(CategoryKey, string.Empty);
                var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
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
                if (!Request.Query["fstore"].ToString().Equals(string.Empty))
                {
                    service = Request.Query["fstore"].ToString();
                }

                if (service.Equals("AddToCart") && !string.IsNullOrEmpty(json))
                {

                    var account = JsonConvert.DeserializeObject<Account>(json);
                    var productId = Convert.ToInt32(Request.Query["productid"]);
                    var quantity = 1;
                    await _client.GetAsync(ApiUri + $"shopping/addtocart/{account.AccountId}&{productId}&{quantity}");
                }

                if (service.Equals("AddToCart") && string.IsNullOrEmpty(json))
                {
                    var productId = Convert.ToInt32(Request.Query["productid"]);
                    var quantity = 1;
                    Dictionary<int, Cart> cart;
                    var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;

                    if (string.IsNullOrEmpty(jsonCart))
                    {
                        cart = new Dictionary<int, Cart>();
                    }
                    else
                    {
                        cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                    }

                    if (cart.ContainsKey(productId))
                    {
                        cart[productId].Quantity += quantity;
                    }
                    else
                    {
                        var cartItem = new Cart
                        {
                            ProductId = productId,
                            Quantity = quantity
                        };
                        cart.Add(productId, cartItem);
                    }
                    jsonCart = JsonConvert.SerializeObject(cart);
                    HttpContext.Session.SetString(CartKey, jsonCart);

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
