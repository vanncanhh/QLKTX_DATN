using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TECH.Data.DatabaseEntity;

namespace TECH.Controllers
{
    public class PaymentController : Controller
    {
        private readonly DataBaseEntityContext _context;

        public PaymentController(DataBaseEntityContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult PaymentVNPay(string nguoiDong, decimal tienDong, string ghiChu, int hoaDonId)
        {
            var vnpay = new VnPayLibrary();

            string vnp_Returnurl = "https://localhost:7127/Payment/PaymentReturn";
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
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán hóa đơn cho {nguoiDong} | hoaDonId={hoaDonId}");
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

            string inputHash = Request.Query["vnp_SecureHash"];
            string vnp_HashSecret = "RLDACN88A1LORYXOB6VG97KDO4UX8RMI";
            bool isValid = vnpay.ValidateSignature(inputHash, vnp_HashSecret);

            if (isValid && vnpay.GetResponseData("vnp_ResponseCode") == "00")
            {
                // ✅ Lấy thông tin hóa đơn ID từ OrderInfo
                string orderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                var match = Regex.Match(orderInfo, @"hoaDonId=(\d+)");
                if (!match.Success)
                {
                    TempData["Message"] = "⚠ Không tìm thấy mã hóa đơn.";
                    return RedirectToAction("Index", "HoaDon");
                }

                int hoaDonId = int.Parse(match.Groups[1].Value);

                // ✅ Cập nhật hóa đơn trong DB
                var hoaDon = _context.hoaDons.FirstOrDefault(h => h.Id == hoaDonId);
                if (hoaDon != null)
                {
                    hoaDon.TrangThai = 1;
                    hoaDon.NgayDongTien = DateTime.Now;
                    hoaDon.TienDong = decimal.Parse(vnpay.GetResponseData("vnp_Amount")) / 100;
                    _context.SaveChanges();

                    TempData["Message"] = "✅ Thanh toán thành công và đã cập nhật hóa đơn!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "⚠ Không tìm thấy hóa đơn trong hệ thống.";
                    return RedirectToAction("Index", "HoaDon");
                }
            }
            else
            {
                TempData["Message"] = "❌ Thanh toán thất bại hoặc bị hủy.";
                return RedirectToAction("Index", "HoaDon");
            }
        }
    }
}
