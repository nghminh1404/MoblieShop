using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class AddCategoryModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public string message { get; set; }

        public AddCategoryModel()
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

                if (Request.Form["name"].Equals(string.Empty))
                {
                    message = "Please, fill all information!";
                    return Page();
                }
            try
            {
                var response = await _client.GetAsync(ApiUri + $"category/get-category-name/{Request.Form["name"]}");
                var strData = await response.Content.ReadAsStringAsync();
                var category = System.Text.Json.JsonSerializer.Deserialize<Category>(strData, option);

                if (category!=null)
                {
                    message = "Category name exist, try again";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                var catgoryPost = new Category
                {
                    CategoryName = Request.Form["name"],
                    IsDeleted = false
                };

                var jsonCategory = System.Text.Json.JsonSerializer.Serialize(catgoryPost);
                var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                await _client.PostAsync(ApiUri + $"category/add-category", content);
            }

            return RedirectToPage("CategoriesManager");

        }
    }
}
