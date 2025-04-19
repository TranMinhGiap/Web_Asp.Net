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
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index(int? page, string txtSearch)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;

            var news = _dbConect.News.AsQueryable();

            if (!string.IsNullOrEmpty(txtSearch))
            {
                news = news.Where(x => x.Title.Contains(txtSearch));
                ViewBag.SearchText = txtSearch;
            }

            var items = news.OrderByDescending(x => x.Id).ToPagedList(pageIndex, pageSize);
            ViewBag.indexItem = (pageIndex - 1) * pageSize;

            return View(items);
        }
        public ActionResult Create()
        {
            List<Category> tmp = _dbConect.Categories.ToList();
            ViewBag.listCategory = tmp;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News document, HttpPostedFileBase Img)
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
                        ModelState.AddModelError("", "Chỉ được phép tải lên file ảnh (.jpg, .jpeg, .png, .gif)");
                        return View(document);
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
                    document.Images = "/assets/Admin/Img/" + uniqueFileName;
                }
                else
                {
                    document.Images = ""; // Không có ảnh thì để trống
                }

                document.Alias = ConvertStr.FilterChar(document.Title);

                _dbConect.News.Add(document);
                _dbConect.SaveChanges(); 

                return RedirectToAction("Index");
            }
            else
            {
                List<Category> tmp = _dbConect.Categories.ToList();
                ViewBag.listCategory = tmp;
                return View(document);
            }
        }
        // Update
        public ActionResult Update(int id)
        {
            var items = _dbConect.News.Find(id);
            if(items != null)
            {
                List<Category> tmp = _dbConect.Categories.ToList();
                ViewBag.listCategory = tmp;
                return View(items);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Update(News news, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                News items = _dbConect.News.Find(news.Id);
                if(items != null)
                {
                    TryUpdateModel(items);
                    // Images
                    if (Img != null && Img.ContentLength > 0)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                        string fileExtension = Path.GetExtension(Img.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("", "Chỉ được phép tải lên file ảnh (.jpg, .jpeg, .png, .gif)");
                            return View(news);
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

                    items.Alias = ConvertStr.FilterChar(news.Title);
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
                List<Category> tmp = _dbConect.Categories.ToList();
                ViewBag.listCategory = tmp;
                return View(news);
            }
        }
        // End Update

        // Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.News.Find(id);
            if (item != null)
            {
                _dbConect.News.Remove(item);
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
                if(items != null && items.Any())
                {
                    foreach(var item in items)
                    {
                        var obj = _dbConect.News.Find(Convert.ToInt32(item));
                        _dbConect.News.Remove(obj);
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