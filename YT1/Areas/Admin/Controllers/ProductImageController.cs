using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;

namespace YT1.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _dbConect.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }
        [HttpPost]
        public ActionResult Add(int productId, string url)
        {
            _dbConect.ProductImages.Add(new ProductImages
            {
                ProductId = productId,
                Image = url,
                isDefault = false
            });
            _dbConect.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ProductImages item = _dbConect.ProductImages.Find(id);
            if(item != null)
            {
                _dbConect.ProductImages.Remove(item);
                _dbConect.SaveChanges();
                return Json(new {success = true});
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult SetDefault(int id)
        {
            var selectedImage = _dbConect.ProductImages.Find(id);
            
            if (selectedImage != null)
            {
                //
                var productitem = _dbConect.products.Find(selectedImage.ProductId);
                productitem.Images = selectedImage.Image;
                //

                var productImages = _dbConect.ProductImages.Where(x => x.ProductId == selectedImage.ProductId).ToList();

                foreach (var image in productImages)
                {
                    image.isDefault = false;
                }

                selectedImage.isDefault = true;

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