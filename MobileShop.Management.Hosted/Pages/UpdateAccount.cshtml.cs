using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MobileShop.Management.Hosted.Pages
{
    public class UpdateAccountModel : PageModel
    {
        private readonly HttpClient _client;
        private IEncryptionService _encryptionService;
        private IValidateService _validateService;
        private string _apiUri = string.Empty;
        public string message { get; set; }
        private string LoginKey = "_login";
        public Account account { get; set; }

        public UpdateAccountModel(IEncryptionService encryptionService, IValidateService validateService)
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
            var id = 0;
            id = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["ida"])
                ? HttpContext.Session.GetString("ida")
                : Request.Query["ida"].ToString());

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

            account = JsonSerializer.Deserialize<Account>(strData, option)!;

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

                if (Request.Form["fullname"].Equals(string.Empty) || Request.Form["address"].Equals(string.Empty) ||
                    Request.Form["gender"].Equals(string.Empty) || Request.Form["phone"].Equals(string.Empty) ||
                    Request.Form["email"].Equals(string.Empty) || Request.Form["dob"].Equals(string.Empty) ||
                    Request.Form["role"].Equals(string.Empty))
                {
                    message = "Information must not null";
                    return RedirectToPage("UpdateAccount");
                }
                else
                {
                    var fullname = Request.Form["fullname"].ToString();
                    var address = Request.Form["address"].ToString();
                    var gender = (Convert.ToInt32(Request.Form["gender"]) == 1) ? true : false;
                    var phone = Request.Form["phone"].ToString();
                    var dob = Convert.ToDateTime(Request.Form["dob"]);
                    var email = Request.Form["email"].ToString();
                    var role = Convert.ToInt32(Request.Form["role"]);
                    //get account profile
                    var id = Convert.ToInt32(HttpContext.Session.GetString("ida"));
                    var response = await _client.GetAsync(_apiUri + $"account/get-account-id/{id}");
                    var strData = await response.Content.ReadAsStringAsync();

                    var accountRequest = System.Text.Json.JsonSerializer.Deserialize<Account>(strData, option);


                    if (!_validateService.ValidatePhone(phone))
                    {
                        message = "Phone numer must 10 characters";
                        return RedirectToPage("UpdateAccount");
                    }

                    if (!_validateService.ValidateMail(email))
                    {
                        message = "Email not exist";
                        return RedirectToPage("UpdateAccount");
                    }

                    if (!_validateService.ValidateName(fullname))
                    {
                        message = "Full name dont have number";
                        return RedirectToPage("UpdateAccount");
                    }


                    if (accountRequest != null)
                    {
                        accountRequest.FullName = fullname;
                        accountRequest.Mail = email;
                        accountRequest.Address = address;
                        accountRequest.Dob = dob;
                        accountRequest.Gender = gender;
                        accountRequest.Phone = phone;
                        accountRequest.RoleId = role;

                        json = JsonSerializer.Serialize(accountRequest);
                    }

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    await _client.PutAsync(_apiUri + $"account/put-account", content);


                    return RedirectToPage("Index");
                }
            }
            catch (Exception)
            {
                message = "Update profile failded";
                return RedirectToPage("UpdateAccount");
            }
        }
    }
}