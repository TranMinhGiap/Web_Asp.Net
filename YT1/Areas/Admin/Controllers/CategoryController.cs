using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;
using YT1.Models.Common;

namespace YT1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var items = _dbConect.Categories.ToList();
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Alias = ConvertStr.FilterChar(category.Title);
                _dbConect.Categories.Add(category);
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }
        public ActionResult Update(int id)
        {
            Category item = _dbConect.Categories.Find(id);
            if (item != null)
            {
                return View(item);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Update(Category item)
        {
            if (ModelState.IsValid)
            {
                var tmp = _dbConect.Categories.Find(item.Id);
                if(tmp != null)
                {
                    tmp.Alias = ConvertStr.FilterChar(item.Title);
                    TryUpdateModel(tmp);
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
                return View(item);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.Categories.Find(id);
            if (item != null)
            {
                _dbConect.Categories.Remove(item);
                _dbConect.SaveChanges();
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

