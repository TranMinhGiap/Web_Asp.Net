using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using YT1.Models;
using YT1.Models.Common;
using YT1.Models.EF;

namespace YT1.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/Product
        public ActionResult Index(int? page, string txtSearch)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;

            var productList = _dbConect.products.AsQueryable();

            if (!string.IsNullOrEmpty(txtSearch))
            {
                productList = productList.Where(x => x.Title.Contains(txtSearch));
                ViewBag.SearchText = txtSearch;
            }

            var items = productList.OrderByDescending(x => x.Id).ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;

            return View(items);
        }
        // Create
        public ActionResult Create()
        {
            List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
            ViewBag.lstproductCategory = listProductCategory;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product item, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có file không
                if (Img != null && Img.ContentLength > 0)
                {
                    // Lấy danh sách đuôi file hợp lệ
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(Img.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
                        ViewBag.lstproductCategory = listProductCategory;
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
                    item.Images = "/assets/Admin/Img/" + uniqueFileName;
                }
                else
                {
                    item.Images = ""; // Không có ảnh thì để trống
                }
                item.Alias = ConvertStr.FilterChar(item.Title);

                _dbConect.products.Add(item);
                _dbConect.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
                ViewBag.lstproductCategory = listProductCategory;
                return View(item);
            }
        }
        // End Create
        
        // Update
        public ActionResult Update(int id)
        {
            Product item = _dbConect.products.Find(id);
            if(item != null)
            {
                List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
                ViewBag.lstproductCategory = listProductCategory;
                return View(item);
            }
            else
            {
                return HttpNotFound();
            } 
        }

        [HttpPost]
        public ActionResult Update(Product productItem, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                Product items = _dbConect.products.Find(productItem.Id);
                if (items != null)
                {
                    TryUpdateModel(items);
                    // Images
                    if (Img != null && Img.ContentLength > 0)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                        string fileExtension = Path.GetExtension(Img.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
                            ViewBag.lstproductCategory = listProductCategory;
                            ModelState.AddModelError("", "Chỉ được phép tải lên file ảnh (.jpg, .jpeg, .png, .gif)");
                            return View(productItem);
                        }

                        string rootFolder = Server.MapPath("~/assets/Admin/Img/");
                        if (!Directory.Exists(rootFolder))
                        {
                            Directory.CreateDirectory(rootFolder);
                        }

                        string uniqueFileName = Path.GetFileNameWithoutExtension(Img.FileName) + "_" + Guid.NewGuid().ToString("N") + fileExtension;
                        string filePath = Path.Combine(rootFolder, uniqueFileName);

                        Img.SaveAs(filePath);

                        items.Images = "/assets/Admin/Img/" + uniqueFileName;
                    }
                    // End Images

                    items.Alias = ConvertStr.FilterChar(productItem.Title);
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
                List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
                ViewBag.lstproductCategory = listProductCategory;
                return View(productItem);
            }
        }
        // End Update

        // Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product item = _dbConect.products.Find(id);
            if (item != null)
            {
                _dbConect.products.Remove(item);
                _dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        // End Delete

        // DeleteAll
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
                        var obj = _dbConect.products.Find(Convert.ToInt32(item));
                        _dbConect.products.Remove(obj);
                        _dbConect.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
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