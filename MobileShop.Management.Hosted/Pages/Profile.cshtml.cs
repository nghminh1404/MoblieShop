using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MobileShop.Management.Hosted.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private IValidateService _validateService;
        public string message { get; set; }
        public Account Account { get; set; }

        public ProfileModel(IValidateService validateService)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
            _validateService = validateService;
        }

        public async Task<IActionResult> OnGet()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Index");
            }

            Account = JsonConvert.DeserializeObject<Account>(json);

            return Page();

        }

        public async Task<IActionResult> OnPost()
        {
            try
            {

                var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;

                if (json.Equals(""))
                {
                    return RedirectToPage("Index");
                }

                Account = JsonConvert.DeserializeObject<Account>(json);

                if (Request.Form["fullname"].Equals("") || Request.Form["address"].Equals("") ||
                    Request.Form["gender"].Equals("") || Request.Form["phone"].Equals("") ||
                    Request.Form["email"].Equals("") || Request.Form["dob"].Equals(""))
                {
                    message = "Infomation must not null";
                    return RedirectToPage("Profile");
                }
                else
                {
                    var fullname = Request.Form["fullname"];
                    var address = Request.Form["address"];
                    var gender = (Convert.ToInt32(Request.Form["gender"]) == 1) ? true : false;
                    var phone = Request.Form["phone"];
                    var dob = Convert.ToDateTime(Request.Form["dob"]);
                    var email = Request.Form["email"];

                    if (!_validateService.ValidatePhone(phone))
                    {
                        message = "Phone numer must 10 characters";
                        return RedirectToPage("Profile");
                    }

                    if (!_validateService.ValidateMail(email))
                    {
                        message = "Email not exist";
                        return RedirectToPage("Profile");
                    }
                    if (!_validateService.ValidateName(fullname))
                    {
                        message = "Full name dont have number";
                        return RedirectToPage("Profile");
                    }

                    Account.FullName = fullname;
                    Account.Mail = email;
                    Account.Address = address;
                    Account.Dob = dob;
                    Account.Gender = gender;
                    Account.Phone = phone;

                    json = System.Text.Json.JsonSerializer.Serialize(Account);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    await _client.PutAsync(ApiUri + $"account/put-account", content);
                    HttpContext.Session.SetString(LoginKey, json);
                    message = "Update profile complete";
                    return RedirectToPage("Profile");
                }
            }
            catch (Exception ex)
            {
                message = "Update profile failded";
                return RedirectToPage("Profile");

            }


        }
    }
}
