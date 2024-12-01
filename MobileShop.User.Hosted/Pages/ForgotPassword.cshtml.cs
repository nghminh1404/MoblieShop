using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.User.Hosted.Pages
{
    public class ForgotPasswordModel : PageModel
    {

        private readonly HttpClient _client;
        private readonly string JafKey = "_jaf"; //json account forgot
        private readonly string OtpKey = "_otp";
        private IMailService _mailService;
        private IValidateService _validateService;
        private IEncryptionService _encryptionService;
        private string KeyService = "_service";
        private string ApiUri = string.Empty;
        public string message { get; set; }

        public ForgotPasswordModel(IMailService mailService, IValidateService validateService, IEncryptionService encryptionService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _mailService = mailService;
            _validateService = validateService;
            _encryptionService = encryptionService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var otp = HttpContext.Session.GetString(OtpKey) ?? "";
            var jaf = HttpContext.Session.GetString(JafKey) ?? "";
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (string.IsNullOrEmpty(otp))
            {
                message = "Please, get otp";
                return Page();
            }
            if (string.IsNullOrEmpty(Request.Form["newpassword"]) ||
                string.IsNullOrEmpty(Request.Form["repassword"]) ||
                string.IsNullOrEmpty(Request.Form["otp"]))
            {
                message = "Infomation must not null";
                return Page();
            }

            if (!otp.Equals(Request.Form["otp"]))
            {
                message = "Otp failded";
                return Page();
            }

            if (!Request.Form["newpassword"].Equals(Request.Form["repassword"]))
            {
                message = "Password is valid";
                return Page();
            }

            var account = System.Text.Json.JsonSerializer.Deserialize<Account>(jaf, option);
            account.Password = _encryptionService.HashMD5(Request.Form["newpassword"]);

            var json = System.Text.Json.JsonSerializer.Serialize(account);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync(ApiUri + $"account/put-account", content);

            return RedirectToPage("Login");
        }
    }
}
