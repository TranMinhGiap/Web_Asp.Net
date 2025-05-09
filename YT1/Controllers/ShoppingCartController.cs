using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;
using YT1.Models.Common;
using YT1.Models.Payments;

namespace YT1.Controllers
{
    [CustomAuthorize(Roles = "Admin, Customer")]
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                ViewBag.checkCart = cart;
            }
            return View();
        }
        public ActionResult PartialItemPayment()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                return PartialView(cart.CartList);
            }
            return PartialView();
        }
        public ActionResult PartialItemCart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                return PartialView(cart.CartList);
            }
            return PartialView();
        }
        public ActionResult vnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = _dbConect.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 2;//đã thanh toán
                            _dbConect.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //string check = UrlPayment(0, "DH5132306185");
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                ViewBag.checkCart = cart;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(Order order)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (ModelState.IsValid)
            {
                if (cart != null && cart.CartList.Any())
                {
                    try
                    {
                        order.Code = "DH" + DateTime.Now.Ticks.ToString().Substring(8);
                        order.TotalAmount = cart.CartList.Sum(x => x.Price * x.Quantity);
                        order.Quantity = cart.CartList.Sum(x => x.Quantity);
                        order.CreatedDate = DateTime.Now;
                        order.CreatedBy = order.CustomerName;
                        order.ModifierDate = DateTime.Now;
                        order.ModifierBy = order.CustomerName;

                        foreach (var item in cart.CartList)
                        {
                            if (item.Quantity > 0 && item.Price >= 0)
                            {
                                order.OrderDetails.Add(new OrderDetail
                                {
                                    ProductId = item.ProductId,
                                    Quantity = item.Quantity,
                                    Price = item.Price
                                });
                            }
                        }

                        _dbConect.Orders.Add(order);
                        _dbConect.SaveChanges();
                        // Send Mail Customer
                        var paymentMethod = "";
                        var strProduct = "";
                        var ThanhTien = decimal.Zero;
                        if(order.PaymentMethod == 1)
                        {
                            paymentMethod = "COD";
                        }
                        else
                        {
                            paymentMethod = "Thanh Toán Qua Ngân Hàng";
                        }
                        foreach (var pr in cart.CartList)
                        {
                            strProduct += "<tr>";
                            strProduct += "<td>"+pr.ProductName+"</td>";
                            strProduct += "<td style=\"padding:12px\">" + pr.Quantity+"</td>";
                            strProduct += "<td style=\"padding:12px\">" + pr.TotalPrice.ToString("N0")+"</td>";
                            strProduct += "</tr>";
                            ThanhTien += pr.Price * pr.Quantity;
                        }
                        string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/assets/SendMail/send2.html"));
                        contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                        contentCustomer = contentCustomer.Replace("{{SanPham}}", strProduct);
                        contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ThanhTien.ToString("N0"));
                        contentCustomer = contentCustomer.Replace("{{TongTien}}", ThanhTien.ToString("N0"));
                        contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                        contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                        contentCustomer = contentCustomer.Replace("{{Email}}", order.Email);
                        contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                        contentCustomer = contentCustomer.Replace("{{PaymentMethod}}", paymentMethod);
                        contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate.ToString("dd/MM/yyyy"));

                        YT1.Models.Common.SendMail.SendMailClient("ShopClother", "Đơn Hàng: " + order.Code, contentCustomer, order.Email);
                        // End Send Mail Customer
                        cart.ClearCart();
                        // Send Mail Admin
                        string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/assets/SendMail/send1.html"));
                        contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                        contentAdmin = contentAdmin.Replace("{{SanPham}}", strProduct);
                        contentAdmin = contentAdmin.Replace("{{ThanhTien}}", ThanhTien.ToString("N0"));
                        contentAdmin = contentAdmin.Replace("{{TongTien}}", ThanhTien.ToString("N0"));
                        contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                        contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                        contentAdmin = contentAdmin.Replace("{{Email}}", order.Email);
                        contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                        contentAdmin = contentAdmin.Replace("{{PaymentMethod}}", paymentMethod);
                        contentAdmin = contentAdmin.Replace("{{NgayDat}}", order.CreatedDate.ToString("dd/MM/yyyy"));

                        YT1.Models.Common.SendMail.SendMailClient("ShopClother", "Đơn Hàng Mới: " + order.Code, contentAdmin, ConfigurationManager.AppSettings["EmailAdmin"]);
                        // End Send Mail Admin
                        // VnPay
                        if(order.PaymentMethod == 2)
                        {
                            var url = UrlPayment(order.PaymentMethodVn, order.Code);
                            return Redirect(url);
                        }
                        return RedirectToAction("CheckOutSuccess");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi xử lý đơn hàng. Vui lòng thử lại. Error: " + ex.Message);
                    }
                }
            }
            ViewBag.checkCart = cart;
            return View(order);
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null && cart.CartList.Any())
            {
                return Json(new { Count = cart.CartList.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var checkProduct = _dbConect.products.Find(id);
            if(checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if(cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImages.FirstOrDefault(x => x.isDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImages.FirstOrDefault(x => x.isDefault).Image;
                }
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = checkProduct.PriceSale;
                }
                else
                {
                    item.Price = checkProduct.Price;
                }
                item.TotalPrice = item.Price * item.Quantity;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm thành công", code = 1, Count = cart.CartList.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                var checkProduct =  cart.CartList.FirstOrDefault(x=>x.ProductId == id);
                if(checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "Đã xóa sản phẩm", code = 1, Count = cart.CartList.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.CartList.Any())
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = _dbConect.Orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbConect.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}