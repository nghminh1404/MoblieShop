using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class AddAccountModel : PageModel
    {
        private readonly HttpClient _client;
        private IEncryptionService _encryptionService;
        private IValidateService _validateService;
        private string _apiUri;
        public string message { get; set; }
        public const string LoginKey = "_login";

        public AddAccountModel(IEncryptionService encryptionService, IValidateService validateService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _encryptionService = encryptionService;
            _validateService = validateService;
        }

        public Task<IActionResult> OnGet()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;


            if (string.IsNullOrEmpty(json))
            {
                return Task.FromResult<IActionResult>(RedirectToPage("Login"));
            }

            var account = JsonConvert.DeserializeObject<Account>(json);
            if (account != null && account.RoleId != 1)
            {
                return Task.FromResult<IActionResult>(RedirectToPage("Login"));
            }

            return Task.FromResult<IActionResult>(Page());
        }

        public async Task<IActionResult> OnPost()
        {
            if (Request.Form["fullname"].Equals(string.Empty) || Request.Form["role"].Equals(string.Empty)
                                                              || Request.Form["address"].Equals(string.Empty) ||
                                                              Request.Form["gender"].Equals(string.Empty)
                                                              || Request.Form["dob"].Equals(string.Empty) ||
                                                              Request.Form["phone"].Equals(string.Empty)
                                                              || Request.Form["email"].Equals(string.Empty) ||
                                                              Request.Form["password"].Equals(string.Empty)
                                                              || Request.Form["repassword"].Equals(string.Empty))
            {
                message = "Please, fill all information!";
                return Page();
            }


            var fullname = Request.Form["fullname"].ToString();
            var gender = Convert.ToInt32(Request.Form["gender"]) == 1;
            //int gender = Convert.ToInt32(Request.Form["Gender"]);
            var mail = Request.Form["email"].ToString();
            var phone = Request.Form["phone"].ToString();
            var dob = Convert.ToDateTime(Request.Form["dob"]);
            var address = Request.Form["address"].ToString();
            var password = Request.Form["password"].ToString();
            var repass = Request.Form["repassword"].ToString();
            var role = Convert.ToInt32(Request.Form["role"]);

            if (!_validateService.ValidatePhone(phone))
            {
                message = "Phone number must 10 characters";
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
                var response = await _client.GetAsync(_apiUri + $"account/get-account-mail/{mail}");
                var strData = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var accountCheck = System.Text.Json.JsonSerializer.Deserialize<Account>(strData, option);
                if (accountCheck != null)
                {
                    message = "Email register exist, try again";
                    return Page();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var newAccount = new Account
            {
                FullName = fullname,
                Mail = mail,
                Address = address,
                Dob = dob,
                Gender = gender,
                Phone = phone,
                Password = password,
                Active = true,
                RoleId = role,
                IsDeleted = false
            };

            //call api post new account
            var jsonAccount = System.Text.Json.JsonSerializer.Serialize(newAccount);
            var content = new StringContent(jsonAccount, Encoding.UTF8, "application/json");
            await _client.PostAsync(_apiUri + $"account/add-account", content);

            return RedirectToPage("Index");
        }
    }
}