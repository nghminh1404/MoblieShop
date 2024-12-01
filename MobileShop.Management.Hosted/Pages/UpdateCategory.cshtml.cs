using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class UpdateCategoryModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        public string message { get; set; }
        public Category category { get; set; }

        public UpdateCategoryModel()
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
                int idc = Convert.ToInt32(Request.Query["idc"].ToString());
                var response = await _client.GetAsync(ApiUri + $"category/get-category-id/{idc}");
                var strData = await response.Content.ReadAsStringAsync();
                category = System.Text.Json.JsonSerializer.Deserialize<Category>(strData, option);
                return Page();
            }
            catch (Exception e)
            {
                message = "Update failded";
                return RedirectToPage("CategoriesManager");

            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idc = Convert.ToInt32(Request.Form["idcategory"]);

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _client.GetAsync(ApiUri + $"category/get-category-id/{idc}");
            var strData = await response.Content.ReadAsStringAsync();
            category = System.Text.Json.JsonSerializer.Deserialize<Category>(strData, option);


            if (Request.Form["name"].Equals(string.Empty))
            {
                message = "Please, fill all information!";
                return Page();
            }

            try
            {
                var response2 = await _client.GetAsync(ApiUri + $"category/get-category-name/{Request.Form["name"]}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var categoryCheck = System.Text.Json.JsonSerializer.Deserialize<Category>(strData2, option);

                if (categoryCheck != null && categoryCheck.CategoryId != category.CategoryId)
                {
                    message = "Category name exist, try again";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                category.CategoryName = Request.Form["name"];

                var jsonCategory = System.Text.Json.JsonSerializer.Serialize(category);
                var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                await _client.PutAsync(ApiUri + "category/put-category", content);
            }

            return RedirectToPage("CategoriesManager");
        }
    }
}
