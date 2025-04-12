using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;

namespace YT1.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(int? id)
        {
            var items = _dbConect.products.ToList();
            
            if(id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
                ViewBag.Category = id;
                var category = _dbConect.ProductCategories.Find(id);
                ViewBag.CategoryName = category.Title;
            }
            return View(items);
        }
        public ActionResult Partial_ItemByCateId()
        {
            var items = _dbConect.products.Where(x => x.IsActive).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductHot()
        {
            var items = _dbConect.products.Where(x => x.IsHot).ToList();
            return PartialView(items);
        }

        public ActionResult Details(int id, string alias)
        {
            var item = _dbConect.products.Find(id);
            return View(item);
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