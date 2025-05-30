using Microsoft.AspNetCore.Mvc;
using TECH;

namespace TECH.Controllers
{
    public class PaymentController : Controller
    {
        [HttpPost]
        public IActionResult PaymentVNPay(string nguoiDong, decimal tienDong, string ghiChu)
        {
            var vnpay = new VnPayLibrary();

            string vnp_Returnurl = "https://localhost:5001/Payment/PaymentReturn";
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string vnp_TmnCode = "SA4T20OD";
            string vnp_HashSecret = "RLDACN88A1LORYXOB6VG97KDO4UX8RMI";

            string orderId = DateTime.Now.Ticks.ToString();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            string createDate = vnTime.ToString("yyyyMMddHHmmss");
            string expireDate = vnTime.AddMinutes(15).ToString("yyyyMMddHHmmss");

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((long)tienDong * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", createDate);
            vnpay.AddRequestData("vnp_ExpireDate", expireDate);
            vnpay.AddRequestData("vnp_CurrCode", "VND");

            string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            ip = ip == "::1" ? "127.0.0.1" : ip;
            vnpay.AddRequestData("vnp_IpAddr", ip);

            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán hóa đơn cho {nguoiDong}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", orderId);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Content(paymentUrl);
        }

        [HttpGet]
        public IActionResult PaymentReturn()
        {
            var vnpay = new VnPayLibrary();

            foreach (var (key, value) in Request.Query)
            {
                vnpay.AddResponseData(key, value);
            }

            // Lấy vnp_SecureHash từ query string
            string inputHash = Request.Query["vnp_SecureHash"];
            string vnp_HashSecret = "RLDACN88A1LORYXOB6VG97KDO4UX8RMI";

            bool isValid = vnpay.ValidateSignature(inputHash, vnp_HashSecret);

            if (isValid && vnpay.GetResponseData("vnp_ResponseCode") == "00")
            {
                ViewBag.Message = "✅ Thanh toán thành công!";
            }
            else
            {
                ViewBag.Message = "❌ Thanh toán thất bại hoặc bị hủy.";
            }

            return View();
        }
    }
}
