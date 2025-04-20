using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;

namespace YT1.Controllers
{
    public class NewsController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_News_Home()
        {
            var item = _dbConect.News
                .Where(x=>x.IsActive)
                .OrderByDescending(x=>x.CreatedDate)
                .Take(3).ToList();
            return PartialView(item);
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