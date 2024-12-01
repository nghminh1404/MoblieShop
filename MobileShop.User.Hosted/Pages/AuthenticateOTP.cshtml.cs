using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text;

namespace MobileShop.User.Hosted.Pages
{
    public class AuthenticateOTPModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly string OtpKey = "_otp";
        private readonly string JarKey = "_jar";
        private string ApiUri = string.Empty;
        public string message { get; set; }

        public AuthenticateOTPModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            var otp = HttpContext.Session.GetString(OtpKey) ?? string.Empty;
            var json = HttpContext.Session.GetString(JarKey) ?? string.Empty;
            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(json))
            {
                return RedirectToPage("AuthenticateOTP");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var otp = HttpContext.Session.GetString(OtpKey) ?? string.Empty;
            var json = HttpContext.Session.GetString(JarKey) ?? string.Empty;

            if (string.IsNullOrEmpty(Request.Form["otp"]))
            {
                message = "OTP must not null";
                return Page();
            }
            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(json))
            {
                return RedirectToPage("AuthenticateOTP");
            }
            var getotp = Request.Form["otp"];

            if (!otp.Equals(getotp))
            {
                message = "OTP failded";
                return Page();
            }

            //call api post new account
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync(ApiUri + $"account/add-account", content);
            HttpContext.Session.Clear();
            return RedirectToPage("Login");
        }
    }
}
