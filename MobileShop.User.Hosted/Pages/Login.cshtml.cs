using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.Json;

namespace MobileShop.User.Hosted.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        private IEncryptionService _encryptionService;
        private string ApiUri = string.Empty;
        public string message { get; set; }
        private string loginKey = "_login";

        public LoginModel(IEncryptionService encryptionService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _encryptionService = encryptionService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Request.Form["mail"]) || string.IsNullOrEmpty(Request.Form["password"]))
            {
                message = "Please, fill all infomations";
                return Page();
            }


            try
            {
                var mail = Request.Form["mail"];
                var password = _encryptionService.HashMD5(Request.Form["password"]!);

                var response = await _client.GetAsync(ApiUri + $"account/get-account-mail-password/{mail}&{password}");
                var strData = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var account = JsonSerializer.Deserialize<Account>(strData, option);
                if (account != null &&
                    account.RoleId == 3 &&
                     account.Active == true && account.IsDeleted == false)
                {
                    var json = JsonConvert.SerializeObject(account);
                    HttpContext.Session.SetString(loginKey, json);
                    return RedirectToPage("Index");
                }
                else
                {
                    message = "Login failed";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                message = "Login failed";
                return Page();
            }


        }
    }
}
