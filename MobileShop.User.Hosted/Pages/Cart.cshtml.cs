using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.DTOs.CartDTO;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.User.Hosted.Pages
{
    public class CartModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string CartKey = "_cart";
        private string DiscountKey = "_discount";
        public List<OrderDetail> orderDetails { get; set; }
        public Dictionary<int, Cart> cart { get; set; }
        public string message { get; set; }

        public CartModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
                var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
                var service = string.Empty;

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                if (!string.IsNullOrEmpty(Request.Query["fstore"]))
                {
                    service = Request.Query["fstore"].ToString();
                }

                if (service.Equals("dequantityitem"))
                {
                    var pid = Convert.ToInt32(Request.Query["idp"].ToString());
                    if (!string.IsNullOrEmpty(json))
                    {
                        var oid = Convert.ToInt32(Request.Query["idb"].ToString());

                        var response2 = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetail-id/{oid}&{pid}");
                        var strData2 = await response2.Content.ReadAsStringAsync();
                        var detail = System.Text.Json.JsonSerializer.Deserialize<OrderDetail>(strData2, option);

                        detail.Quantity -= 1;

                        var jsonDetail = System.Text.Json.JsonSerializer.Serialize(detail);
                        var content = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                        await _client.PutAsync(ApiUri + $"orderdetail/put-orderdetail", content);
                    }
                    else
                    {
                        cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                        if (cart.ContainsKey(pid))
                        {
                            cart[pid].Quantity -= 1;
                        }
                        jsonCart = JsonConvert.SerializeObject(cart);
                        HttpContext.Session.SetString(CartKey, jsonCart);
                    }
                }

                if (service.Equals("incquantityitem"))
                {
                    var pid = Convert.ToInt32(Request.Query["idp"].ToString());

                    if (!string.IsNullOrEmpty(json))
                    {
                        var oid = Convert.ToInt32(Request.Query["idb"].ToString());

                        var response2 = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetail-id/{oid}&{pid}");
                        var strData2 = await response2.Content.ReadAsStringAsync();
                        var detail = System.Text.Json.JsonSerializer.Deserialize<OrderDetail>(strData2, option);

                        detail.Quantity += 1;

                        var jsonDetail = System.Text.Json.JsonSerializer.Serialize(detail);
                        var content = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                        await _client.PutAsync(ApiUri + $"orderdetail/put-orderdetail", content);
                    }
                    else
                    {
                        cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                        if (cart.ContainsKey(pid))
                        {
                            cart[pid].Quantity += 1;
                        }
                        jsonCart = JsonConvert.SerializeObject(cart);
                        HttpContext.Session.SetString(CartKey, jsonCart);
                    }
                }

                if (service.Equals("removeitem"))
                {
                    var pid = Convert.ToInt32(Request.Query["idp"].ToString());
                    if (!string.IsNullOrEmpty(json))
                    {
                        var oid = Convert.ToInt32(Request.Query["idb"].ToString());
                        await _client.DeleteAsync(ApiUri + $"orderdetail/delete-orderdetail/{oid}&{pid}");
                    }
                    else
                    {
                        cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                        if (cart.ContainsKey(pid))
                        {
                            cart.Remove(pid);
                        }
                        jsonCart = JsonConvert.SerializeObject(cart);
                        HttpContext.Session.SetString(CartKey, jsonCart);
                    }
                }

                if (!string.IsNullOrEmpty(json))
                {

                    var account = JsonConvert.DeserializeObject<Account>(json);

                    var response = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-customerid/{account.AccountId}");
                    var strData = await response.Content.ReadAsStringAsync();
                    orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData, option);
                    if (orderDetails == null)
                    {
                        return RedirectToPage("Shop");
                    }
                }
                else
                {
                    jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
                    cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                    if (cart == null)
                    {
                        return RedirectToPage("Shop");
                    }

                }

            }
            catch (Exception e)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!string.IsNullOrEmpty(json))
            {

                var account = JsonConvert.DeserializeObject<Account>(json);

                var response = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-customerid/{account.AccountId}");
                var strData = await response.Content.ReadAsStringAsync();
                orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData, option);
            }
            else
            {
                cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                if (cart == null)
                {
                    return RedirectToPage("Shop");
                }
            }

            if (!string.IsNullOrEmpty(Request.Form["discountcode"]))
            {
                try
                {
                    var response2 = await _client.GetAsync(ApiUri + $"coupon/get-coupon-code/{Request.Form["discountcode"]}");
                    var strData2 = await response2.Content.ReadAsStringAsync();
                    var coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData2, option);

                    if (coupon.ExpirationDate > DateTime.Now && coupon.IsDeleted == false)
                    {
                        var jsonCoupon = System.Text.Json.JsonSerializer.Serialize(coupon);
                        HttpContext.Session.SetString(DiscountKey, jsonCoupon);
                    }
                    else
                    {
                        message = "Invalid discount code";
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    message = "Invalid discount code";
                    return Page();
                }
            }
            else
            {
                var response2 = await _client.GetAsync(ApiUri + $"coupon/get-coupon-code/NORMAL");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(strData2, option);

                var jsonCoupon = System.Text.Json.JsonSerializer.Serialize(coupon);
                HttpContext.Session.SetString(DiscountKey, jsonCoupon);
            }

            return RedirectToPage("Checkout");
        }
    }
}
