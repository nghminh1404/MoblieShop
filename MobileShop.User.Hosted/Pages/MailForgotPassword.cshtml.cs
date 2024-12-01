using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.User.Hosted.Pages
{
    public class MailForgotPasswordModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly string JafKey = "_jaf"; //json account forgot
        private readonly string OtpKey = "_otp";
        private IMailService _mailService;
        private IValidateService _validateService;
        private string ApiUri = string.Empty;
        public string message { get; set; }

        public MailForgotPasswordModel(IMailService mailService, IValidateService validateService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _mailService = mailService;
            _validateService = validateService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {

            if (string.IsNullOrEmpty(Request.Form["email"]))
            {
                message = "Enter mail forgot password";
                return Page();
            }
            if (!_validateService.ValidateMail(Request.Form["email"]))
            {
                message = "Email is validate";
                return Page();
            }
            string mail = Request.Form["email"];

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            try
            {
                var response = await _client.GetAsync(ApiUri + $"account/get-account-mail/{mail}");
                string strData = await response.Content.ReadAsStringAsync();
                var acccount = System.Text.Json.JsonSerializer.Deserialize<Account>(strData, option);

                var otp = _mailService.GetOTP();

                HttpContext.Session.SetString(JafKey, strData);
                HttpContext.Session.SetString(OtpKey, otp);

                _mailService.Send(mail, otp, "Forgot password");
            }
            catch (Exception ex)
            {
                message = "Email does not exist";
            }

            return RedirectToPage("ForgotPassword");
        }
    }
}
