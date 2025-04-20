using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;

namespace YT1.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Subcribe()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Subcribe(string email)
        {
            if(ModelState.IsValid)
            {
                if (_dbConect.Subcribe.Any(x => x.Email == email))
                {
                    return Json(new { success = false, message = "Email đã tồn tại trong hệ thống!" });
                }
                else
                {
                    Subcribe subcribe = new Subcribe();
                    subcribe.Email = email;
                    _dbConect.Subcribe.Add(subcribe);
                    _dbConect.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }
        public ActionResult Refresh()
        {
            var item = new ThongKeView();
            var visitors_online = HttpContext.Application["visitors_online"];
            item.VisitorsOnline = HttpContext.Application["visitors_online"] != null ? (uint)HttpContext.Application["visitors_online"] : 0;
            item.HomNay = HttpContext.Application["HomNay"] != null ? (long)HttpContext.Application["HomNay"] : 0;
            item.HomQua = HttpContext.Application["HomQua"] != null ? (long)HttpContext.Application["HomQua"] : 0;
            item.TuanNay = HttpContext.Application["TuanNay"] != null ? (long)HttpContext.Application["TuanNay"] : 0;
            item.TuanTruoc = HttpContext.Application["TuanTruoc"] != null ? (long)HttpContext.Application["TuanTruoc"] : 0;
            item.ThangNay = HttpContext.Application["ThangNay"] != null ? (long)HttpContext.Application["ThangNay"] : 0;
            item.ThangTruoc = HttpContext.Application["ThangTruoc"] != null ? (long)HttpContext.Application["ThangTruoc"] : 0;
            item.TatCa = HttpContext.Application["TatCa"] != null ? (long)HttpContext.Application["TatCa"] : 0;
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