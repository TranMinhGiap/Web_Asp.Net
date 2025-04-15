using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;

namespace YT1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
    }
}