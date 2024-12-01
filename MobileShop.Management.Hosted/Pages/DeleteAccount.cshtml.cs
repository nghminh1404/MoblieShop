using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class DeleteAccountModel : PageModel
    {
        private readonly HttpClient _client;
        private IEncryptionService _encryptionService;
        private IValidateService _validateService;
        private string _apiUri = string.Empty;
        public string message { get; set; }
        private string LoginKey = "_login";
        public Account account { get; set; }

        public DeleteAccountModel(IEncryptionService encryptionService, IValidateService validateService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _encryptionService = encryptionService;
            _validateService = validateService;
        }

        public async Task<IActionResult> OnGet()
        {
            int id = 0;
            if (string.IsNullOrEmpty(Request.Query["ida"]))
            {
                id = Convert.ToInt32(HttpContext.Session.GetString("ida"));
            }
            else
            {
                id = Convert.ToInt32(Request.Query["ida"].ToString());
            }

            HttpContext.Session.SetString("ida", id.ToString());

            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Login");
            }

            var response = await _client.GetAsync(_apiUri + $"account/get-account-id/{id}");
            var strData = await response.Content.ReadAsStringAsync();

            account = JsonSerializer.Deserialize<Account>(strData, option);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

                if (json.Equals(""))
                {
                    return RedirectToPage("Login");
                }

                var id = Convert.ToInt32(HttpContext.Session.GetString("ida"));
                var response = await _client.GetAsync(_apiUri + $"account/get-account-id/{id}");
                var strData = await response.Content.ReadAsStringAsync();

                var accountRequest = System.Text.Json.JsonSerializer.Deserialize<Account>(strData, option);

                if (accountRequest != null)
                {
                    accountRequest.IsDeleted = true;

                    json = System.Text.Json.JsonSerializer.Serialize(accountRequest);
                }

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _client.PutAsync(_apiUri + $"account/put-account", content);

                return RedirectToPage("Index");
            }
            catch (Exception)
            {
                message = "Delete profile failed";
                return RedirectToPage("DeleteAccount");
            }
        }
    }
}