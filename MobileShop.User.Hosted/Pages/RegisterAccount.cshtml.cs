using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileShop.User.Hosted.Pages
{
    public class RegisterAccountModel : PageModel
    {
        private readonly HttpClient _client;
        private IValidateService _validateService;
        private IMailService _mailService;
        private readonly string OtpKey = "_otp";
        private readonly string JarKey = "_jar";
        private string ApiUri = string.Empty;
        public string message { get; set; }

        public RegisterAccountModel(IValidateService validateService,
                                    IMailService mailService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _validateService = validateService;
            _mailService = mailService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (Request.Form["fullname"].Equals(string.Empty)
                || Request.Form["address"].Equals(string.Empty) || Request.Form["gender"].Equals(string.Empty)
                || Request.Form["dob"].Equals(string.Empty) || Request.Form["phone"].Equals(string.Empty)
                || Request.Form["email"].Equals(string.Empty) || Request.Form["password"].Equals(string.Empty)
                || Request.Form["repassword"].Equals(string.Empty))
            {
                message = "Please, fill all information!";
                return Page();
            }

            var fullname = Request.Form["fullname"];
            var gender = false;
            if (Convert.ToInt32(Request.Form["gender"]) == 1)
            {
                gender = true;
            }
            //int gender = Convert.ToInt32(Request.Form["Gender"]);
            var mail = Request.Form["email"];
            var phone = Request.Form["phone"];
            var dob = Convert.ToDateTime(Request.Form["dob"]);
            var address = Request.Form["address"];
            var password = Request.Form["password"];
            var repass = Request.Form["repassword"];
            if (!_validateService.ValidatePhone(phone))
            {
                message = "Phone numer must 10 characters";
                return Page();
            }

            if (!password.Equals(repass))
            {
                message = "Password must the same repassword";
                return Page();
            }

            if (!_validateService.ValidateMail(mail))
            {
                message = "Email not exist";
                return Page();
            }
            if (!_validateService.ValidateName(fullname))
            {
                message = "Full name is Failed";
                return Page();
            }
            try
            {
                var response = await _client.GetAsync(ApiUri + $"account/get-account-mail/{mail}");
                string strData = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var acccountCheck = System.Text.Json.JsonSerializer.Deserialize<Account>(strData, option);
                if (acccountCheck != null)
                {
                    message = "Email register exist, try again";
                    return Page();
                }
            }
            catch (Exception ex) { }
            // var pass = _encryptionService.HashMD5(password);
            var newAccount = new Account();
            newAccount.FullName = fullname;
            newAccount.Mail = mail;
            newAccount.Address = address;
            newAccount.Dob = dob;
            newAccount.Gender = gender;
            newAccount.Phone = phone;
            newAccount.Password = password;
            newAccount.Active = true;
            newAccount.RoleId = 3;
            newAccount.IsDeleted = false;

            //Create otp
            var otp = _mailService.GetOTP();
            HttpContext.Session.SetString(OtpKey, otp);
            var json = JsonSerializer.Serialize(newAccount);
            HttpContext.Session.SetString(JarKey, json);

            try
            {
                //send otp to mail register
                _mailService.Send(mail, otp, "Authenticate account");
            }
            catch (Exception ex)
            {
                message = "Error!";
            }
            /*
            //call api post new account
            var jsonAccount = System.Text.Json.JsonSerializer.Serialize(newAccount);
            var content = new StringContent(jsonAccount, Encoding.UTF8, "application/json");
            await _client.PostAsync(ApiUri + $"account/add-account", content);
            */
            return RedirectToPage("AuthenticateOTP");
        }
    }
}
