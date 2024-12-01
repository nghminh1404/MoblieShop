using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileShop.Entity.Models;
using MobileShop.Shared.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using MobileShop.Shared.VNPay;

namespace MobileShop.User.Hosted.Pages
{
    public class VNPayModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;
        private string ApiUri = string.Empty;
        private string LoginKey = "_login";
        private string DiscountKey = "_discount";
        private string AddressKey = "_address";
        private string ErrorKey = "_error";
        private string GuestKey = "_guest";
        private string CartKey = "_cart";

        public VNPayModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = $"{UrlConstant.ApiBaseUrl}/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            var service = string.Empty;
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            var jsonGuest = HttpContext.Session.GetString(GuestKey) ?? string.Empty;
            var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;
            Account account = null;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!string.IsNullOrEmpty(Request.Query["fstore"]))
            {
                service = Request.Query["fstore"].ToString();
            }

            if (service.Equals("payment"))
            {
                string url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                string returnUrl = "https://localhost:7042/VNPay?fstore=paymentconfirm";
                string tmnCode = "JF3JL0X5";
                string hashSecret = "HEGRFWKYJRWZZYNMRQJZJDSSCSDARYTQ";
                PayLib pay = new PayLib();
                double price = 0;
                var products = string.Empty;
                //user
                if (!string.IsNullOrEmpty(json))
                {
                    account = JsonConvert.DeserializeObject<Account>(json);
                }
                else
                {
                    //guest
                    account = JsonConvert.DeserializeObject<Account>(jsonGuest);
                }
                var response2 = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-customerid/{account.AccountId}");
                var strData2 = await response2.Content.ReadAsStringAsync();
                var orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData2, option);

                foreach (var d in orderDetails)
                {
                    var response3 = await _client.GetAsync(ApiUri + $"product/get-product-id/{d.ProductId}");
                    var strData3 = await response3.Content.ReadAsStringAsync();
                    var product = System.Text.Json.JsonSerializer.Deserialize<Product>(strData3, option);
                    if (string.IsNullOrEmpty(products))
                    {
                        products = product.ProductName;
                    }
                    else
                    {
                        products = products + ", " + product.ProductName;
                    }
                    price += d.Quantity * product.Price;

                }

                if (!string.IsNullOrEmpty(jsonCoupon))
                {
                    var coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(jsonCoupon);
                    var discount = (coupon.DiscountPercent / 100) * price;
                    price = price - discount;
                }

                pay.AddRequestData("vnp_Version", "2.1.0");
                pay.AddRequestData("vnp_Command", "pay");
                pay.AddRequestData("vnp_TmnCode", tmnCode);
                pay.AddRequestData("vnp_Amount", Convert.ToString(100 * price));
                pay.AddRequestData("vnp_BankCode", "");
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", "VND");
                pay.AddRequestData("vnp_IpAddr", GetIpAddress());
                pay.AddRequestData("vnp_Locale", "vn");
                pay.AddRequestData("vnp_OrderInfo", $"Payment order {products}");
                pay.AddRequestData("vnp_OrderType", "other");
                pay.AddRequestData("vnp_ReturnUrl", returnUrl);
                pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

                return Redirect(paymentUrl);
            }

            if (service.Equals("paymentconfirm"))
            {
                if (Request.Query.Count > 0)
                {
                    string hashSecret = "HEGRFWKYJRWZZYNMRQJZJDSSCSDARYTQ";
                    var vnpayData = Request.Query;
                    PayLib pay = new PayLib();

                    foreach (var kvp in vnpayData)
                    {
                        string key = kvp.Key;
                        string value = kvp.Value;

                        if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                        {
                            pay.AddResponseData(key, value);
                        }
                    }

                    long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
                    long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                    string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                    string vnp_SecureHash = Request.Query["vnp_SecureHash"];

                    bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);

                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00")
                        {
                            if (!string.IsNullOrEmpty(json))
                            {
                                account = JsonConvert.DeserializeObject<Account>(json);
                            }
                            else
                            {
                                account = JsonConvert.DeserializeObject<Account>(jsonGuest);

                            }
                            var response = await _client.GetAsync(ApiUri + $"order/get-order-CustomerId/{account.AccountId}");
                            var strData = await response.Content.ReadAsStringAsync();
                            var order = System.Text.Json.JsonSerializer.Deserialize<Order>(strData, option);
                            /*status
                             * 0: add to cart
                             * 1: checkout
                             * 2: shipping
                             * 3: complete
                             */
                            order.Status = 1;
                            order.PaymentId = 1;

                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(AddressKey)))
                            {
                                order.Address = HttpContext.Session.GetString(AddressKey);

                            }

                            var jsonOrder = JsonConvert.SerializeObject(order);
                            var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                            await _client.PutAsync(ApiUri + $"order/put-order", content);

                            var coupon = System.Text.Json.JsonSerializer.Deserialize<Coupon>(jsonCoupon);

                            if (coupon.CouponId != 5)
                            {
                                coupon.IsDeleted = true;
                                jsonCoupon = JsonConvert.SerializeObject(coupon);
                                var content2 = new StringContent(jsonCoupon, Encoding.UTF8, "application/json");
                                await _client.PutAsync(ApiUri + $"coupon/put-coupon", content2);
                            }

                            var response2 = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-OrderID/{order.OrderId}");
                            var strData2 = await response2.Content.ReadAsStringAsync();
                            var orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData2, option);

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

                            HttpContext.Session.SetString(ErrorKey, "");
                            HttpContext.Session.SetString(CartKey, "");

                            return RedirectToPage("Purchase");

                        }
                        else
                        {
                            HttpContext.Session.SetString(ErrorKey, "An error occurred while processing the invoice " + orderId + " | Trading code: " + vnpayTranId + " | Error code: " + vnp_ResponseCode);
                            return RedirectToPage("Checkout");
                        }
                    }
                    else
                    {
                        HttpContext.Session.SetString(ErrorKey, "An error occurred during processing");
                        return RedirectToPage("Checkout");
                    }
                }

            }
            return Page();

        }

        public string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"];

                if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
                {
                    ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                }
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP: " + ex.Message;
            }

            return ipAddress;
        }

    }
}
