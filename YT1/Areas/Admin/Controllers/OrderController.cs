using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;

namespace YT1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;
            
            var orderList = _dbConect.Orders.OrderByDescending(x=>x.CreatedDate).ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;
            return View(orderList);
        }
        // Xác nhận đơn hàng (status == 1)
        public ActionResult Confirm(int? page)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;
            var orderList = _dbConect.Orders
                .Where(x => x.Status == 1) 
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;
            return View(orderList);
        }
        // =================

        // Chuẩn bị đơn hàng (status == 3)
        public ActionResult PrepareGood(int? page)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;
            var orderList = _dbConect.Orders
                .Where(x => x.Status == 3)
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;
            return View(orderList);
        }
        // =============================

        // Đơn hàng được đang được giao (status == 4)
        public ActionResult Delivery(int? page)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;
            var orderList = _dbConect.Orders
                .Where(x => x.Status == 4)
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;
            return View(orderList);
        }
        // =============================

        // Hoàn tất thanh toán (các đơn hàng đã được thanh toán)
        public ActionResult Complete(int? page)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;
            var orderList = _dbConect.Orders
                .Where(x => x.Status == 2)
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;
            return View(orderList);
        }
        // =============================

        // ================ TEST  Complete===============
        public ActionResult InvoicePrint(int id)
        {
            var order_item = _dbConect.Orders.Find(id);
            if (order_item != null)
            {
                var orderDetails = _dbConect.OrderDetails
                                    .Where(od => od.OrderId == id)
                                    .ToList();
                ViewBag.orderDetails = orderDetails;
                return View(order_item);
            }
            return HttpNotFound();
        }
        // ===============================
        public ActionResult Details(int id)
        {
            var order_item = _dbConect.Orders.Find(id);
            if(order_item != null)
            {
                var orderDetails = _dbConect.OrderDetails
                                    .Where(od => od.OrderId == id).ToList();
                ViewBag.orderDetails = orderDetails;
                return View(order_item);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Update(int id, int status)
        {
            var item = _dbConect.Orders.Find(id);
            if (item != null)
            {
                item.Status = status;
                _dbConect.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Failed", Success = false });
        }
        // delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Order item = _dbConect.Orders.Find(id);
            if (item != null)
            {
                try
                {
                    var orderDetails = _dbConect.OrderDetails
                        .Where(od => od.OrderId == id)
                        .ToList();

                    if (orderDetails.Any())
                    {
                        _dbConect.OrderDetails.RemoveRange(orderDetails);
                    }

                    _dbConect.Orders.Remove(item);

                    _dbConect.SaveChanges();

                    return Json(new { success = true, message = "Order deleted successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Failed to delete: {ex.Message}" });
                }
            }
            return Json(new { success = false });
        }
        // End Delete

        // DeleteAll
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { success = false, message = "No IDs provided" });
            }

            var idList = new List<int>();
            try
            {
                idList = ids.Split(',')
                    .Select(id => Convert.ToInt32(id))
                    .Where(id => id > 0) 
                    .ToList();
            }
            catch (FormatException)
            {
                return Json(new { success = false, message = "Invalid ID format" });
            }

            if (!idList.Any())
            {
                return Json(new { success = false, message = "No valid IDs provided" });
            }

            try
            {
                var orderDetailsToDelete = _dbConect.OrderDetails
                    .Where(od => idList.Contains(od.OrderId))
                    .ToList();

                if (orderDetailsToDelete.Any())
                {
                    _dbConect.OrderDetails.RemoveRange(orderDetailsToDelete);
                }

                var ordersToDelete = _dbConect.Orders
                    .Where(o => idList.Contains(o.Id))
                    .ToList();

                if (ordersToDelete.Any())
                {
                    _dbConect.Orders.RemoveRange(ordersToDelete);
                }
                else
                {
                    return Json(new { success = false, message = "No orders found to delete" });
                }

                _dbConect.SaveChanges();

                return Json(new { success = true, message = "Orders deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Failed to delete: {ex.Message}" });
            }
        }
        // End DeleteAll
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