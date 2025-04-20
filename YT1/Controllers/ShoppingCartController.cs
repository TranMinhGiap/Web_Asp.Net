using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;
using YT1.Models.Common;

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