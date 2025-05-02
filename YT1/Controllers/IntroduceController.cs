using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;

namespace YT1.Controllers
{
    public class IntroduceController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Introduce
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageIndex = page ?? 1;
            var item = _dbConect.Posts.Where(x => x.Category.Title == "Giới Thiệu").AsQueryable();

            var items = item.OrderByDescending(x => x.CreatedDate).ToPagedList(pageIndex, pageSize);
            return View(items);
        }
        public ActionResult Details(int id)
        {
            var item = _dbConect.Posts.Find(id);
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