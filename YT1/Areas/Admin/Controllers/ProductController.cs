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
            // Cách thông thường + foreach
            //List<ProductCategory> listProductCategory = _dbConect.ProductCategories.ToList();
            //ViewBag.lstproductCategory = listProductCategory;
            ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product item, List<string> Img, List<int> radioButton)
        {
            if (ModelState.IsValid)
            {
                if (Img != null && Img.Count > 0)
                {
                    for (int i = 0; i < Img.Count; i++)
                    {
                        if(i + 1 == radioButton[0])
                        {
                            item.Images = Img[i];
                            item.ProductImages.Add(new ProductImages
                            {
                                ProductId = item.Id,
                                Image = Img[i],
                                isDefault = true
                            });
                        }
                        else
                        {
                            item.ProductImages.Add(new ProductImages
                            {
                                ProductId = item.Id,
                                Image = Img[i],
                                isDefault = false
                            });
                        }
                    }
                }
                item.Alias = ConvertStr.FilterChar(item.Title);
                if (string.IsNullOrEmpty(item.SeoTitle))
                {
                    item.SeoTitle = item.Title;
                }
                _dbConect.products.Add(item);
                _dbConect.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
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
                ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
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
                            ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
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
                ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
                return View(productItem);
            }
        }
        // End Update

        // Details
        public ActionResult Details(int id)
        {
            Product item = _dbConect.products.Find(id);
            if(item != null)
            {
                return View(item);
            }
            else
            {
                return HttpNotFound();
            }
        }
        // End Details

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