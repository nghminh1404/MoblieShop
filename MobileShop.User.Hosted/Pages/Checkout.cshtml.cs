using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.DTOs.CartDTO;
using MobileShop.Entity.Models;
using MobileShop.Service;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MobileShop.User.Hosted.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string DiscountKey = "_discount";
        private string AddressKey = "_address";
        private string CartKey = "_cart";
        private string GuestKey = "_guest";
        private IValidateService _validateService;

        public Coupon coupon { get; set; }
        public List<Payment> payments { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
        public Dictionary<int, Cart> cart { get; set; }
        public string message { get; set; }

        public CheckoutModel(IValidateService validateService)
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
            var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;

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

                if (orderDetails == null)
                {
                    return RedirectToPage("Shop");
                }
            }
            else
            {
                cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);
                if (cart == null)
                {
                    return RedirectToPage("Shop");
                }
            }

            if (!string.IsNullOrEmpty(jsonCoupon))
            {
                coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(jsonCoupon);
            }

            var response2 = await _client.GetAsync(ApiUri + $"payment/get-all-payment");
            var strData2 = await response2.Content.ReadAsStringAsync();
            payments = System.Text.Json.JsonSerializer.Deserialize<List<Payment>>(strData2, option);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!string.IsNullOrEmpty(json))
            {
                var account = System.Text.Json.JsonSerializer.Deserialize<Account>(json);
                if (string.IsNullOrEmpty(Request.Form["address"]))
                {
                    message = "Adress must not null";

                    var response2 = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-customerid/{account.AccountId}");
                    var strData2 = await response2.Content.ReadAsStringAsync();
                    orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData2, option);

                    if (orderDetails == null)
                    {
                        return RedirectToPage("Shop");
                    }
                    if (!string.IsNullOrEmpty(jsonCoupon))
                    {
                        coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(jsonCoupon);
                    }

                    var response3 = await _client.GetAsync(ApiUri + $"payment/get-all-payment");
                    var strData3 = await response3.Content.ReadAsStringAsync();
                    payments = System.Text.Json.JsonSerializer.Deserialize<List<Payment>>(strData3, option);

                    return Page();

                }

                if (Request.Form["payment"].Equals("2"))
                {
                    var response = await _client.GetAsync(ApiUri + $"order/get-order-CustomerId/{account.AccountId}");
                    var strData = await response.Content.ReadAsStringAsync();
                    var order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData, option);

                    var response2 = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-customerid/{account.AccountId}");
                    var strData2 = await response2.Content.ReadAsStringAsync();
                    orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData2, option);

                    if (!string.IsNullOrEmpty(jsonCoupon))
                    {
                        coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(jsonCoupon);
                    }

                    /*status
                     * 0: add to cart
                     * 1: checkout
                     * 2: shipping
                     * 3: complete
                     */
                    order.Status = 1;
                    order.PaymentId = 2;
                    order.CouponId = coupon.CouponId;
                    order.Address = Request.Form["address"];

                    var jsonOrder = JsonConvert.SerializeObject(order);
                    var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                    await _client.PutAsync(ApiUri + $"order/put-order", content);

                    if (coupon.CouponId != 5)
                    {
                        coupon.IsDeleted = true;
                        jsonCoupon = JsonConvert.SerializeObject(coupon);
                        var content2 = new StringContent(jsonCoupon, Encoding.UTF8, "application/json");
                        await _client.PutAsync(ApiUri + $"coupon/put-coupon", content2);
                    }

                    foreach (var item in orderDetails)
                    {
                        var response6 = await _client.GetAsync(ApiUri + $"product/get-product-id/{item.ProductId}");
                        var strData6 = await response6.Content.ReadAsStringAsync();
                        var product = System.Text.Json.JsonSerializer.Deserialize<Product>(strData6, option);

                        product.Quantity -= item.Quantity;

                        var jsonProduct = System.Text.Json.JsonSerializer.Serialize(product);
                        var content7 = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
                        await _client.PutAsync(ApiUri + $"product/put-product", content7);
                    }
                    
                    return RedirectToPage("Purchase");
                }

                if (Request.Form["payment"].Equals("1"))
                {
                    HttpContext.Session.SetString(AddressKey, Request.Form["address"]);
                    return Redirect("/VNPay?fstore=payment");
                }

            }
            else
            {
                cart = JsonConvert.DeserializeObject<Dictionary<int, Cart>>(jsonCart);

                var response = await _client.GetAsync(ApiUri + $"payment/get-all-payment");
                var strData = await response.Content.ReadAsStringAsync();
                payments = System.Text.Json.JsonSerializer.Deserialize<List<Payment>>(strData, option);

                if (cart == null)
                {
                    return RedirectToPage("Shop");
                }


                if (!string.IsNullOrEmpty(jsonCoupon))
                {
                    coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(jsonCoupon);
                }

                if (string.IsNullOrEmpty(Request.Form["fullname"]) || string.IsNullOrEmpty(Request.Form["address"])
                    || string.IsNullOrEmpty(Request.Form["payment"]) || string.IsNullOrEmpty(Request.Form["phone"])
                    || string.IsNullOrEmpty(Request.Form["email"]))
                {
                    message = "All Infomations must not null";

                    return Page();
                }

                if (!_validateService.ValidateName(Request.Form["fullname"]))
                {
                    message = "Fullname is failed";

                    return Page();
                }

                if (!_validateService.ValidateMail(Request.Form["email"]))
                {
                    message = "Email is failed";

                    return Page();
                }

                if (!_validateService.ValidatePhone(Request.Form["phone"]))
                {
                    message = "Phone numer must 10 characters";

                    return Page();
                }

                try
                {
                    var response2 = await _client.GetAsync(ApiUri + $"account/get-account-mailguest/{Request.Form["email"]}");
                    string strData2 = await response2.Content.ReadAsStringAsync();

                    var acccountCheck = System.Text.Json.JsonSerializer.Deserialize<Account>(strData2, option);
                    if (acccountCheck != null)
                    {
                        message = "Email register exist, try again";
                        return Page();
                    }
                }
                catch (Exception ex) { }

                var account = new Account
                {
                    FullName = Request.Form["fullname"],
                    Address = Request.Form["address"],
                    Mail = Request.Form["email"],
                    Phone = Request.Form["phone"],
                    Active = false,
                    RoleId = 1002,
                    IsDeleted = false
                };

                var jsonGuest = System.Text.Json.JsonSerializer.Serialize(account);
                var content = new StringContent(jsonGuest, Encoding.UTF8, "application/json");
                await _client.PostAsync(ApiUri + $"account/add-account", content);

                var response3 = await _client.GetAsync(ApiUri + $"account/get-account-mailguest/{Request.Form["email"]}");
                string strData3 = await response3.Content.ReadAsStringAsync();
                var guest = System.Text.Json.JsonSerializer.Deserialize<Account>(strData3, option);

                if (Request.Form["payment"].Equals("2"))
                {
                    /*status
                    * 0: add to cart
                    * 1: checkout
                    * 2: shipping
                    * 3: complete
                    */

                    var order = new Order
                    {
                        CustomerId = guest.AccountId,
                        Address = guest.Address,
                        CreateDate = Convert.ToDateTime(_validateService.DateNow()),
                        Status = 1,
                        PaymentId = 2, //defaul payment is cod
                        CouponId = coupon.CouponId,
                        IsDeleted = false
                    };
                    //add new order
                    var jsonOrder = System.Text.Json.JsonSerializer.Serialize(order);
                    var content2 = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                    await _client.PostAsync(ApiUri + $"order/add-order", content2);

                    var response4 = await _client.GetAsync(ApiUri + $"order/get-order-GuestId/{guest.AccountId}");
                    var strData4 = await response4.Content.ReadAsStringAsync();
                    order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData4, option);

                    //add new oder detail
                    foreach (var item in cart)
                    {
                        var detail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.Value.ProductId,
                            Quantity = item.Value.Quantity,
                            IsDeleted = false
                        };
                        var response6 = await _client.GetAsync(ApiUri + $"product/get-product-id/{item.Value.ProductId}");
                        var strData6 = await response6.Content.ReadAsStringAsync();
                        var product = System.Text.Json.JsonSerializer.Deserialize<Product>(strData6, option);

                        product.Quantity -= item.Value.Quantity;

                        var jsonProduct = System.Text.Json.JsonSerializer.Serialize(product);
                        var content7 = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
                        await _client.PutAsync(ApiUri + $"product/put-product", content7);

                        var jsonDetail = System.Text.Json.JsonSerializer.Serialize(detail);
                        var content5 = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                        await _client.PostAsync(ApiUri + $"orderdetail/add-orderdetail", content5);

                    }
                    jsonGuest = System.Text.Json.JsonSerializer.Serialize(guest);
                    HttpContext.Session.SetString(GuestKey, jsonGuest);
                    HttpContext.Session.SetString(CartKey, "");

                    return RedirectToPage("Purchase");


                }

                if (Request.Form["payment"].Equals("1"))
                {
                    var order = new Order
                    {
                        CustomerId = guest.AccountId,
                        Address = guest.Address,
                        CreateDate = Convert.ToDateTime(_validateService.DateNow()),
                        Status = 0,
                        PaymentId = 2, //defaul payment is cod
                        CouponId = coupon.CouponId,
                        IsDeleted = false
                    };

                    //add new order
                    var jsonOrder = System.Text.Json.JsonSerializer.Serialize(order);
                    var content2 = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                    await _client.PostAsync(ApiUri + $"order/add-order", content2);

                    var response4 = await _client.GetAsync(ApiUri + $"order/get-order-CustomerId/{guest.AccountId}");
                    var strData4 = await response4.Content.ReadAsStringAsync();
                    order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData4, option);

                    //add new oder detail
                    foreach (var item in cart)
                    {
                        var detail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.Value.ProductId,
                            Quantity = item.Value.Quantity,
                            IsDeleted = false
                        };

                        var jsonDetail = System.Text.Json.JsonSerializer.Serialize(detail);
                        var content5 = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                        await _client.PostAsync(ApiUri + $"orderdetail/add-orderdetail", content5);

                    }
                    jsonGuest = System.Text.Json.JsonSerializer.Serialize(guest);
                    HttpContext.Session.SetString(GuestKey, jsonGuest);
                    return Redirect("/VNPay?fstore=payment");
                }


            }

            return RedirectToPage("Shop");
        }
    }
}
