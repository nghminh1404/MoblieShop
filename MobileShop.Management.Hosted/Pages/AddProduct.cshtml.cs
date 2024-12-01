using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Image = MobileShop.Entity.Models.Image;

namespace MobileShop.Management.Hosted.Pages
{
    public class AddProductModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private string _apiUri = string.Empty;
        private string LoginKey = "_login";
        public string message { get; set; }
        public List<Product>? Products { get; set; }
        public List<Category> Categories { get; set; }
        private IValidateService _validateService;
        private IWebHostEnvironment _environment;
        [Required(ErrorMessage = "Please choose at least one file")]
        [DataType(DataType.Upload)]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public AddProductModel(IWebHostEnvironment environment, IValidateService validateService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _environment = environment;
            _validateService = validateService;
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

            var response = await _client.GetAsync(_apiUri + $"category/get-all-category");
            var strData = await response.Content.ReadAsStringAsync();
            Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData, option);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _client.GetAsync(_apiUri + $"category/get-all-category");
            var strData = await response.Content.ReadAsStringAsync();
            Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(strData, option);

            var filename = string.Empty;
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                foreach (var fileUpload in FileUploads)
                {
                    var extension = Path.GetExtension(fileUpload.FileName);
                    if (allowedExtensions.Contains(extension.ToLower()))
                    {
                        var basePath = _environment.ContentRootPath[.._environment.ContentRootPath.IndexOf("MobileShop.Management.Hosted", StringComparison.Ordinal)];
                        var fileAdmin = basePath + @"MobileShop.Management.Hosted\wwwroot\image\" + fileUpload.FileName;
                        var fileUser = basePath + @"MobileShop.User.Hosted\wwwroot\image\" + fileUpload.FileName;
                        filename = fileUpload.FileName;
                        await using (var fileStream = new FileStream(fileAdmin, FileMode.Create))
                        {
                            await fileUpload.CopyToAsync(fileStream);
                        }

                        await using (var fileStream = new FileStream(fileUser, FileMode.Create))
                        {
                            await fileUpload.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Only the following file types are allowed: {string.Join(", ", allowedExtensions)}");
                    }
                }
            }

            if (Request.Form["pname"].Equals(string.Empty)
                || Request.Form["department"].Equals(string.Empty)
                || Request.Form["quantity"].Equals(string.Empty)
                || Request.Form["price"].Equals(string.Empty)
                || Request.Form["description"].Equals(string.Empty))
            {
                message = "Add failed, check all information";
                return Page();
            }

            Image? newImage;
            try
            {

                var response3 = await _client.GetAsync(_apiUri + $"image/get-image-link/{filename}");
                var strData3 = await response3.Content.ReadAsStringAsync();
                newImage = System.Text.Json.JsonSerializer.Deserialize<Image>(strData3, option);
            }
            catch (Exception)
            {
                var requestImage = new Image
                {
                    ImageLink = filename,
                    CreateDate = DateTime.Now,
                    IsDeleted = false
                };

                var json = System.Text.Json.JsonSerializer.Serialize(requestImage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _client.PostAsync(_apiUri + "image/add-image", content);

                var response2 = await _client.GetAsync(_apiUri + $"image/get-image-link/{filename}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                newImage = System.Text.Json.JsonSerializer.Deserialize<Image>(strData2, option);
            }

            var accountJson = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var account = JsonConvert.DeserializeObject<Account>(accountJson);

            if (newImage == null) return RedirectToPage("ProductManager");
            if (account == null) return RedirectToPage("ProductManager");
            var requestProduct = new Product
            {
                ProductName = Request.Form["pname"].ToString(),
                Price = Convert.ToDouble(Request.Form["price"]),
                Quantity = Convert.ToInt32(Request.Form["quantity"]),
                Description = Request.Form["description"],
                CategoryId = Convert.ToInt32(Request.Form["department"]),
                ImageId = newImage.ImageId,
                CreateDate = DateTime.Now,
                CreateBy = account.AccountId,
                IsDeleted = false
            };

            var productJson = System.Text.Json.JsonSerializer.Serialize(requestProduct);
            var content2 = new StringContent(productJson, Encoding.UTF8, "application/json");
            await _client.PostAsync(_apiUri + "product/add-product", content2);

            return RedirectToPage("ProductManager");
        }
    }
}
