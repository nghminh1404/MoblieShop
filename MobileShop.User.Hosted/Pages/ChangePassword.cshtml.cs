using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MobileShop.User.Hosted.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private readonly string OtpKey = "_otp";
        private IMailService _mailService;
        private IEncryptionService _encryptionService;
        public string message { get; set; }

        public ChangePasswordModel(IMailService mailService, IEncryptionService encryptionService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _mailService = mailService;
            _encryptionService = encryptionService;
        }

        public async Task<IActionResult> OnGet()
        {
            string json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            var account = JsonConvert.DeserializeObject<Account>(json);

            var otp = _mailService.GetOTP();
            HttpContext.Session.SetString(OtpKey, otp);
            _mailService.Send(account.Mail, otp, "Change password");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var json = HttpContext.Session.GetString(LoginKey) ?? "";
                var otp = HttpContext.Session.GetString(OtpKey) ?? "";

                if (string.IsNullOrEmpty(otp))
                {
                    message = "Please click button GET OTP";
                    return Page();
                }
                if (string.IsNullOrEmpty(json))
                {
                    return RedirectToPage("Login");
                }

                if (string.IsNullOrEmpty(Request.Form["oldpassword"]) || string.IsNullOrEmpty(Request.Form["newpassword"]) ||
                    string.IsNullOrEmpty(Request.Form["repassword"]) || string.IsNullOrEmpty(Request.Form["otp"]))
                {
                    message = "Infomation must not null";
                    return Page();
                }

                var account = JsonConvert.DeserializeObject<Account>(json);
                var oldPassword = Request.Form["oldpassword"];
                var newPassword = Request.Form["newpassword"];
                var RePassword = Request.Form["repassword"];

                if (!otp.Equals(Request.Form["otp"]))
                {
                    message = "Otp failded";
                    return Page();
                }

                if (!_encryptionService.HashMD5(oldPassword).Equals(account.Password))
                {
                    message = "Old password is failded";
                    return Page();
                }

                if (!newPassword.Equals(RePassword))
                {
                    message = "New password must the same re-password";
                    return Page();
                }

                account.Password = _encryptionService.HashMD5(newPassword);

                json = System.Text.Json.JsonSerializer.Serialize(account);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _client.PutAsync(ApiUri + $"account/put-account", content);
                HttpContext.Session.SetString(LoginKey, json);

                return RedirectToPage("Login");
            }
            catch (Exception ex)
            {
                message = "Change password failded";
                return Page();
            }
        }
    }
}
