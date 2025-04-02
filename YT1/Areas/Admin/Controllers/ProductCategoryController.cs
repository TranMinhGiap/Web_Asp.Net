using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using YT1.Models;
using YT1.Models.Common;
using YT1.Models.EF;

namespace YT1.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index(int? page, string txtSearch)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;

            var productList = _dbConect.ProductCategories.AsQueryable();

            if (!string.IsNullOrEmpty(txtSearch))
            {
                productList = productList.Where(x => x.Title.Contains(txtSearch));
                ViewBag.SearchText = txtSearch;
            }

            var items = productList.OrderByDescending(x => x.Id).ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;

            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory item, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                // Img
                if (Img != null && Img.ContentLength > 0)
                {
                    // Lấy danh sách đuôi file hợp lệ
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(Img.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Chỉ được phép tải lên file ảnh (.jpg, .jpeg, .png, .gif)");
                        return View(item);
                    }

                    // Lấy đường dẫn thư mục lưu ảnh
                    string rootFolder = Server.MapPath("~/assets/Admin/Img/");
                    if (!Directory.Exists(rootFolder))
                    {
                        Directory.CreateDirectory(rootFolder);
                    }

                    // Tạo tên file duy nhất để tránh trùng lặp
                    string uniqueFileName = Path.GetFileNameWithoutExtension(Img.FileName) + "_" + Guid.NewGuid().ToString("N") + fileExtension;
                    string filePath = Path.Combine(rootFolder, uniqueFileName);

                    // Lưu file vào server
                    Img.SaveAs(filePath);

                    // Lưu đường dẫn file vào đối tượng News
                    item.Icon = "/assets/Admin/Img/" + uniqueFileName;
                }
                else
                {
                    item.Icon = ""; // Không có ảnh thì để trống
                }
                // End Img

                item.Alias = ConvertStr.FilterChar(item.Title);
                _dbConect.ProductCategories.Add(item);
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
        }
        public ActionResult Update(int id)
        {
            ProductCategory item = _dbConect.ProductCategories.Find(id);
            if(item != null)
            {
                return View(item);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Update(ProductCategory res, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                ProductCategory item = _dbConect.ProductCategories.Find(res.Id);
                if (item != null)
                {
                    TryUpdateModel(item);
                    // Img
                    if (Img != null && Img.ContentLength > 0)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                        string fileExtension = Path.GetExtension(Img.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("", "Chỉ được phép tải lên file ảnh (.jpg, .jpeg, .png, .gif)");
                            return View(res);
                        }

                        string rootFolder = Server.MapPath("~/assets/Admin/Img/");
                        if (!Directory.Exists(rootFolder))
                        {
                            Directory.CreateDirectory(rootFolder);
                        }

                        string uniqueFileName = Path.GetFileNameWithoutExtension(Img.FileName) + "_" + Guid.NewGuid().ToString("N") + fileExtension;
                        string filePath = Path.Combine(rootFolder, uniqueFileName);

                        Img.SaveAs(filePath);

                        item.Icon = "/assets/Admin/Img/" + uniqueFileName;
                    }
                    // End Img
                    item.Alias = ConvertStr.FilterChar(item.Title);
                    _dbConect.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View(res);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ProductCategory item = _dbConect.ProductCategories.Find(id);
            if(item != null)
            {
                _dbConect.ProductCategories.Remove(item);
                _dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        ProductCategory obj = _dbConect.ProductCategories.Find(Convert.ToInt32(item));
                        _dbConect.ProductCategories.Remove(obj);
                        _dbConect.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
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