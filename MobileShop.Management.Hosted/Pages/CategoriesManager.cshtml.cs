using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class CategoriesManagerModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public List<Category> categories { get; set; }

        public CategoriesManagerModel()
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

            if (service.Equals("delete"))
            {
                try
                {
                    var idc = Convert.ToInt32(Request.Query["idc"].ToString());

                    var response2 = await _client.GetAsync(ApiUri + $"category/get-category-id/{idc}");
                    var strData2 = await response2.Content.ReadAsStringAsync();
                    var categoryPut = System.Text.Json.JsonSerializer.Deserialize<Category>(strData2, option);

                    categoryPut.IsDeleted = true;

                    var jsonCategory = System.Text.Json.JsonSerializer.Serialize(categoryPut);
                    var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                    await _client.PutAsync(ApiUri + "category/put-category", content);
                }
                catch (Exception e)
                {

                }
            }
            var response = await _client.GetAsync(ApiUri + $"category/get-all-category");
            var strData = await response.Content.ReadAsStringAsync();
            categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData, option);

            return Page();
        }
    }
}
