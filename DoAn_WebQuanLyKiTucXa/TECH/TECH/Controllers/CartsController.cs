﻿//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.Diagnostics;
//using System.Net.Mail;
//using TECH.Areas.Admin.Models;
//using TECH.Areas.Admin.Models.Search;
//using TECH.Models;
//using TECH.Service;

//namespace TECH.Controllers
//{
//    public class CartsController : Controller
//    {
//        private readonly ICartsService _cartsService;
//        private readonly IOrdersService _ordersService;
//        private readonly IProductsService _productsService;
//        public IHttpContextAccessor _httpContextAccessor;
//        public CartsController(ICartsService cartsService,
//            IHttpContextAccessor httpContextAccessor,
//            IOrdersService ordersService,
//            //IReviewsService reviewsService,
//            //IImagesService imagesService,
//            ISizesService sizesService,
//        //    IProductQuantityService productQuantityService,
//        //IProductsImagesService productsImagesService,
//        IProductsService productsService)
//        {
//            _cartsService = cartsService;
//            _ordersService = ordersService;
//            _httpContextAccessor = httpContextAccessor;
//            _productsService = productsService;
//            //_reviewsService = reviewsService;
//            //_imagesService = imagesService;
//            //_productsImagesService = productsImagesService;
//            //_sizesService = sizesService;
//            //_productQuantityService = productQuantityService;
//        }

//        [HttpPost]
//        public JsonResult AddOrder(OrdersModelView OrdersModelView)
//        {
//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            if (userString != null)
//            {
//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                OrdersModelView.user_id = user.id;
//                OrdersModelView.code = "MDH00" + user.id.ToString() + DateTime.Now.Second.ToString();
//                var result = _ordersService.AddOrder(OrdersModelView);

//                if (result > 0)
//                {
//                    var model = _cartsService.GetAllCart(user.id);
//                    if (model != null && model.Count > 0)
//                    {
//                        foreach (var item in model)
//                        {
//                            var ordersDetailModelView = new OrdersDetailModelView();
//                            ordersDetailModelView.order_id = result;
//                            ordersDetailModelView.product_id = item.product_id;
//                            ordersDetailModelView.color = item.color;
//                            //ordersDetailModelView.sizeId = item.sizeId;
//                            var product = _productsService.GetByid(item.product_id.Value);
//                            ordersDetailModelView.price = Convert.ToInt32(product.price_sell);
//                            ordersDetailModelView.quantity = item.quantity;
//                            int totalQuantitySell = 0;
//                            if (product.quantitysell.HasValue && product.quantitysell.Value > 0)
//                            {
//                                totalQuantitySell = product.quantitysell.Value;
//                            }
//                            totalQuantitySell += item.quantity.Value;
//                            _productsService.UpdateQuantitySell(item.product_id.Value, totalQuantitySell);
//                            _ordersService.AddOrderDetail(ordersDetailModelView);
//                            _cartsService.Deleted(item.id);
//                            //_ordersService.Save();
//                        }
//                        return Json(new
//                        {
//                            success = true
//                        });
//                    }
//                }

//            }

//            return Json(new
//            {
//                success = false
//            });
//        }

////        public void SendMail(string email, string code)
////        {

////            var html = @"
////    <div width='100%' style='margin: 0; padding: 0 !important; background-color: #f1f1f1;'>
////        <h3>Xin chào bạn. Cảm ơn bạn đã tin tưởng và đặt hàng.</h3>
////        <center style='width: 100%; background-color: #f1f1f1;'>
////            <div style='margin: 0 auto;' class='m_431664471943874608email-container'>
////                <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'>
////                    <tbody>
////                        <tr>
////                            <td valign='middle' class='m_431664471943874608hero m_431664471943874608bg_white' style='padding: 2em 0 2em 0;'>
////                                <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
////                                    <tbody>
////                                        <tr>
////                                            <td style='padding: 0 2.5em; text-align: center;'>
////                                                <div class='m_431664471943874608text'>
////                                                    <h3>Đơn hàng của bạn</h3>
////                                                </div>
////                                            </td>
////                                        </tr>
////                                    </tbody>
////                                </table>
////                            </td>
////                        </tr>
////                        <tr>
////                            <td>
////                                <table class='m_431664471943874608bg_white' role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
////                                    <tbody>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05); background-color: grey;'>
////                                            <th width='20%' style='text-align: center; padding: 0 2.5em; color: #000; padding-bottom: 20px;'>Tên sản phẩm</th>
////                                            <th width='45%' style='text-align: center; padding: 0 2.5em; color: #000; padding-bottom: 20px;'>Số lượng</th>
////                                            <th width='10%' style='text-align: center; padding: 0 2.5em; color: #000; padding-bottom: 20px;'>Tổng</th>
////                                        </tr>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>Ipad Promax | màu null</p>
////                                            </td>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>x1</p>
////                                            </td>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>30000092</p>
////                                            </td>
////                                        </tr>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td colspan='2' style='text-align: right; padding: 0 2.5em;'>
////                                                Tạm tính
////                                            </td>
////                                            <td style='padding: 0 2.5em;'>30000092<i>đ</i></td>
////                                        </tr>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td colspan='2' style='text-align: right; padding: 0 2.5em;'>
////                                                Hình thức mua hàng
////                                            </td>
////                                            <td style='padding: 0 2.5em;'>Mua trực tiếp</td>
////                                        </tr>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td colspan='2' style='text-align: right; padding: 0 2.5em;'>
////                                                Tổng phải thanh toán
////                                            </td>
////                                            <td style='padding: 0 2.5em;'>30000092<i>đ</i></td>
////                                        </tr>
////                                    </tbody>
////                                </table>
////                            </td>
////                        </tr>
////                    </tbody>
////                </table>
////            </div>
////        </center>

////        <center style='width: 100%; background-color: #f1f1f1;'>
////            <div style='margin: 0 auto;' class='m_431664471943874608email-container'>
////                <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'>
////                    <tbody>
////                        <tr>
////                            <td valign='middle' class='m_431664471943874608hero m_431664471943874608bg_white' style='padding: 2em 0 2em 0;'>
////                                <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
////                                    <tbody>
////                                        <tr>
////                                            <td style='padding: 0 2.5em; text-align: center;'>
////                                                <div class='m_431664471943874608text'>
////                                                    <h3>Thông tin vận chuyển</h3>
////                                                </div>
////                                            </td>
////                                        </tr>
////                                    </tbody>
////                                </table>
////                            </td>
////                        </tr>
////                        <tr>
////                            <td>
////                                <table class='m_431664471943874608bg_white' role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
////                                    <tbody>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>Họ và tên:</p>
////                                            </td>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>nguyễn văn a</p>
////                                            </td>
////                                        </tr>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>Số điện thoại:</p>
////                                            </td>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>0345801983</p>
////                                            </td>
////                                        </tr>
////                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'>Ghi chú:</p>
////                                            </td>
////                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
////                                                <p style='color: #000; font-size: 15px;'></p>
////                                            </td>
////                                        </tr>
////                                    </tbody>
////                                </table>
////                            </td>
////                        </tr>
////                    </tbody>
////                </table>
////            </div>
////        </center>
////</div>
////";


////            MailMessage mail = new MailMessage();
////            mail.To.Add(email.Trim());
////            mail.From = new MailAddress("emcuahai@gmail.com");
////            mail.Subject = "Xác Thực Tài Khoản";
////            mail.Body = html;
////            mail.IsBodyHtml = true;
////            mail.Sender = new MailAddress("emcuahai@gmail.com");
////            SmtpClient smtp = new SmtpClient();
////            smtp.Port = 587;
////            smtp.EnableSsl = true;
////            smtp.UseDefaultCredentials = false;
////            smtp.Host = "smtp.gmail.com";
////            smtp.Credentials = new System.Net.NetworkCredential("emcuahai@gmail.com", "tnquotkcftugcbve");
////            smtp.Send(mail);
////        }

//        public IActionResult HistoryOrder()
//        {
//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            var model = new List<OrdersModelView>();
//            if (userString != null)
//            {
//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                if (user != null)
//                {
//                    var data = _ordersService.GetOrderForUserId(user.id);
//                    if (data != null && data.Count > 0)
//                    {
//                        foreach (var item in data)
//                        {
//                            if (item != null && item.user_id.HasValue)
//                            {
//                                if (item.payment == 1)
//                                {
//                                    item.paymentstr = "Ship Cod";
//                                }
//                                else if (item.payment == 2)
//                                {
//                                    item.paymentstr = "VnPay";
//                                }
//                                else if (item.payment == 3)
//                                {
//                                    item.paymentstr = "Momo";
//                                }
//                                else if (item.payment == 0)
//                                {
//                                    item.paymentstr = "Mua trực tiếp";
//                                }
//                            }
//                            if (item.status == 0)
//                            {
//                                item.statusstr = "Đang chờ xử lý";
//                            }
//                            else if (item.status == 1)
//                            {
//                                item.statusstr = "Đã hoàn thành";
//                            }
//                            else if (item.status == 2)
//                            {
//                                item.statusstr = "Đã hủy";
//                            }

//                        }
//                        model = data;
//                    }
//                    return View(model);
//                }
//            }
//            return Redirect("/home");
//        }

//        public IActionResult Index()
//        {
//            var carts = new List<CartsModelView>();
//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            if (userString != null)
//            {
//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                if (user != null)
//                {
//                    var model = _cartsService.GetAllCart(user.id);
//                    if (model != null && model.Count > 0)
//                    {
//                        foreach (var item in model)
//                        {
//                            if (item.product_id.HasValue && item.product_id.Value > 0)
//                            {
//                                var _product = _productsService.GetByid(item.product_id.Value);
//                                if (_product != null)
//                                {
//                                    decimal quantityProduct = 0;
//                                    decimal quantityProductCartAndSell = 0;
//                                    if (_product.quantity.HasValue && _product.quantity.Value > 0
//                                        && _product.quantitysell.HasValue && _product.quantitysell.Value > 0)
//                                    {
//                                        quantityProduct = _product.quantity.Value - _product.quantitysell.Value; // số lượng hiện còn trong hệ thống
//                                    }
//                                    else if (_product.quantity.HasValue && _product.quantity.Value > 0)
//                                    {
//                                        quantityProduct = _product.quantity.Value;
//                                    }
//                                    else if (_product.quantitysell.HasValue && _product.quantitysell.Value > 0)
//                                    {
//                                        quantityProductCartAndSell = _product.quantitysell.Value;
//                                    }

//                                    if (item.quantity.HasValue && item.quantity.Value > 0)
//                                    {
//                                        quantityProductCartAndSell += item.quantity.Value;
//                                    }

//                                    decimal quantityReal = quantityProduct - quantityProductCartAndSell;

//                                    _product.quantityReal = quantityReal;

//                                    item.productModelView = _product;
//                                }
//                            }
//                            if (item.sizeId.HasValue && item.sizeId.Value > 0)
//                            {
//                                var sizeData = _sizesService.GetByid(item.sizeId.Value);
//                                if (sizeData != null)
//                                {
//                                    item.sizeStr = sizeData.name;
//                                }
//                            }
//                        }
//                        carts = model;
//                    }
//                }
//                return View(carts);
//            }
//            return Redirect("/home");

//        }
//        public IActionResult ReviewOrderProduct(int orderId)
//        {
//            var data = new List<OrdersModelView>();
//            if (orderId > 0)
//            {
//                var order = _ordersService.GetByid(orderId);
//                if (order != null)
//                {
//                    var orderDetail = _ordersService.GetOrderDetails(orderId);
//                    if (orderDetail != null && orderDetail.Count > 0)
//                    {
//                        foreach (var item in orderDetail)
//                        {
//                            if (item.product_id.HasValue && item.product_id.Value > 0)
//                            {
//                                var product = _productsService.GetByid(item.product_id.Value);
//                                if (product != null)
//                                {
//                                    item.ProductModelView = product;
//                                }

//                            }
//                        }
//                        order.OrdersDetailModelView = orderDetail;
//                    }
//                    data.Add(order);
//                }
//            }
//            return PartialView("ReivewsOrderProduct", data);
//        }
//        public IActionResult OrderPay()
//        {
//            var ordersCartDetailModelView = new OrdersCartDetailModelView();
//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            if (userString != null)
//            {
//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                if (user != null)
//                {
//                    user.address = !string.IsNullOrEmpty(user.address) && user.address != "null" ? user.address : "";
//                    ordersCartDetailModelView.UserModelView = user;
//                    var model = _cartsService.GetAllCart(user.id);
//                    if (model != null && model.Count > 0)
//                    {
//                        foreach (var item in model)
//                        {
//                            if (item.product_id.HasValue && item.product_id.Value > 0)
//                            {
//                                var _product = _productsService.GetByid(item.product_id.Value);
//                                if (_product != null)
//                                {
//                                    //var productImage = _productsImagesService.GetImageProduct(_product.id);
//                                    //if (productImage != null && productImage.Count > 0)
//                                    //{
//                                    //    var image = _imagesService.GetImageName(productImage);
//                                    //    if (image != null && image.Count > 0)
//                                    //    {
//                                    //        _product.ImageModelView = image;
//                                    //    }
//                                    //}
//                                    item.productModelView = _product;
//                                }
//                            }
//                        }
//                        ordersCartDetailModelView.CartsModelView = model;
//                        return View(ordersCartDetailModelView);
//                    }
//                }
//            }
//            return Redirect("/home");

//        }

//        [HttpPost]
//        public JsonResult ReviewsPost(List<ReviewsModelView> reviewsPost)
//        {

//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            bool status = false;
//            if (userString != null)
//            {
//                if (reviewsPost != null && reviewsPost.Count > 0)
//                {
//                    foreach (var item in reviewsPost)
//                    {
//                        item.status = 0;
//                        //_reviewsService.Add(item);
//                        //_reviewsService.Save();
//                    }
//                    _ordersService.UpdateReview(reviewsPost[0].order_id.Value, 1);
//                    _ordersService.Save();
//                    status = true;
//                }
//                else
//                {
//                    status = false;
//                }
//            }
//            else
//            {
//                status = false;
//            }

//            return Json(new
//            {
//                success = status
//            });
//        }




//        [HttpPost]
//        public JsonResult Add(CartsModelView cartsModelView)
//        {

//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            bool overstock = false;
//            int tongsocon = 0;
//            var user = new UserModelView();
//            if (userString != null)
//            {

//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                if (user != null)
//                {
//                    cartsModelView.user_id = user.id;
//                    //var product = _productsService.GetByid(cartsModelView.product_id.Value);
//                    // thực hiện trừ đi trong database
//                    if (cartsModelView.product_id.HasValue && cartsModelView.product_id.Value > 0)
//                    {
//                        if (_cartsService.IsExist(user.id, cartsModelView.product_id.Value))
//                        {
//                            return Json(new
//                            {
//                                success = true
//                            });
//                        }
//                        var product = _productsService.GetByid(cartsModelView.product_id.Value);
//                        if (product != null)
//                        {
//                            decimal quantityProduct = 0;
//                            decimal quantityProductCartAndSell = 0;
//                            if (product.quantity.HasValue && product.quantity.Value > 0
//                                && product.quantitysell.HasValue && product.quantitysell.Value > 0)
//                            {
//                                quantityProduct = product.quantity.Value - product.quantitysell.Value; // số lượng hiện còn trong hệ thống
//                            }
//                            else if (product.quantity.HasValue && product.quantity.Value > 0)
//                            {
//                                quantityProduct = product.quantity.Value;
//                            }
//                            else if (product.quantitysell.HasValue && product.quantitysell.Value > 0)
//                            {
//                                quantityProductCartAndSell = product.quantitysell.Value;
//                            }

//                            if (cartsModelView.quantity.HasValue && cartsModelView.quantity.Value > 0)
//                            {
//                                quantityProductCartAndSell += cartsModelView.quantity.Value;
//                            }

//                            decimal quantityReal = quantityProduct - quantityProductCartAndSell;

//                            if (quantityReal < 0)
//                            {
//                                return Json(new
//                                {
//                                    success = false,
//                                    overstock = quantityProduct,
//                                    tongsoconlai = -1
//                                });
//                            }
//                            // check kiểm tra nếu như số lượng sản phẩm ít hơn số lượng cần mua thì hiển thị thông báo message
//                            // cùng với số lượng hiện đang còn trong hệ thống
//                            // cập nhật vào trong bảng giỏ hàng
//                            _cartsService.Add(cartsModelView);
//                            _cartsService.Save();
//                            return Json(new
//                            {
//                                success = true,
//                                //overstock = overstock,
//                                //tongsoconlai = tongsocon
//                            });
//                        }
//                    }
//                }
//            }

//            return Json(new
//            {
//                success = false,
//                overstock = overstock,
//                tongsoconlai = -1
//            });
//        }

//        [HttpPost]
//        public JsonResult Update(CartsModelView cartsModelView)
//        {

//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            if (userString != null)
//            {
//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                if (user != null)
//                {
//                    cartsModelView.user_id = user.id;
//                    if (cartsModelView.product_id.HasValue && cartsModelView.product_id.Value > 0)
//                    {
//                        var product = _productsService.GetByid(cartsModelView.product_id.Value);

//                        if (product != null)
//                        {
//                            decimal quantityProduct = 0;
//                            decimal quantityProductCartAndSell = 0;
//                            if (product.quantity.HasValue && product.quantity.Value > 0
//                                && product.quantitysell.HasValue && product.quantitysell.Value > 0)
//                            {
//                                quantityProduct = product.quantity.Value - product.quantitysell.Value; // số lượng hiện còn trong hệ thống
//                            }
//                            else if (product.quantity.HasValue && product.quantity.Value > 0)
//                            {
//                                quantityProduct = product.quantity.Value;
//                            }
//                            else if (product.quantitysell.HasValue && product.quantitysell.Value > 0)
//                            {
//                                quantityProductCartAndSell = product.quantitysell.Value;
//                            }

//                            if (cartsModelView.quantity.HasValue && cartsModelView.quantity.Value > 0)
//                            {
//                                quantityProductCartAndSell += cartsModelView.quantity.Value;
//                            }

//                            decimal quantityReal = quantityProduct - quantityProductCartAndSell;
//                            if (quantityReal < 0)
//                            {
//                                return Json(new
//                                {
//                                    success = false,
//                                    productname = product.name,
//                                    overstock = product.quantity
//                                });
//                            }

//                            cartsModelView.price = Convert.ToInt32(product.price_sell) * cartsModelView.quantity.Value;
//                            cartsModelView.pricestr = cartsModelView.price.HasValue && cartsModelView.price.Value > 0 ? cartsModelView.price.Value.ToString("#,###") : "";
//                        }

//                    }

//                    var result = _cartsService.Update(cartsModelView);
//                    _cartsService.Save();
//                    return Json(new
//                    {
//                        success = result,
//                        Data = cartsModelView
//                    });
//                }
//            }

//            return Json(new
//            {
//                success = false,
//                overstock = -1
//            });
//        }

//        [HttpPost]
//        public JsonResult Deleted(int id)
//        {

//            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
//            var user = new UserModelView();
//            if (userString != null)
//            {
//                user = JsonConvert.DeserializeObject<UserModelView>(userString);
//                if (user != null)
//                {
//                    var result = _cartsService.Deleted(id);
//                    _cartsService.Save();
//                    return Json(new
//                    {
//                        success = result
//                    });
//                }
//            }

//            return Json(new
//            {
//                success = false
//            });
//        }




//    }
//}