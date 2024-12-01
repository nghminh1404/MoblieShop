using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        private IEncryptionService _encryptionService;
        private string _apiUri;
        public string message { get; set; }
        private const string LoginKey = "_login";

        public LoginModel(IEncryptionService encryptionService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _encryptionService = encryptionService;
        }

        public Task<IActionResult> OnGet()
        {
            return Task.FromResult<IActionResult>(Page());
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Request.Form["mail"]) || string.IsNullOrEmpty(Request.Form["password"]))
            {
                message = "Please, fill all infomations";
                return Page();
            }
            else
            {
                try
                {
                    var mail = Request.Form["mail"];
                    var password = _encryptionService.HashMD5(Request.Form["password"]!);
                    var response =
                        await _client.GetAsync(_apiUri + $"account/get-account-mail-password/{mail}&{password}");
                    var strData = await response.Content.ReadAsStringAsync();

                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    var account = System.Text.Json.JsonSerializer.Deserialize<Account>(strData, option);
                    if (account is { RoleId: 1 or 2 } and { Active: true, IsDeleted: false })
                    {
                        var json = JsonConvert.SerializeObject(account);
                        HttpContext.Session.SetString(LoginKey, json);
                        return RedirectToPage("Index");
                    }

                    else
                    {
                        message = "Login failed";
                        return Page();
                    }
                }
                catch (Exception)
                {
                    message = "Login failed";
                    return Page();
                }
            }
        }
    }
}