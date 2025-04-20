using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.EF;

namespace YT1.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Setting()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            if (!ModelState.IsValid)
            {
                return View("Partial_Setting", req);
            }

            SaveSetting("SettingTitle", req.SettingTitle);
            SaveSetting("SettingLogo", req.SettingLogo);
            SaveSetting("SettingEmail", req.SettingEmail);
            SaveSetting("SettingHotline", req.SettingHotline);
            SaveSetting("SettingTitleSeo", req.SettingTitleSeo);
            SaveSetting("SettingDesSeo", req.SettingDesSeo);
            SaveSetting("SettingKeySeo", req.SettingKeySeo);

            _dbConect.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SaveSetting(string key, string value)
        {
            var setting = _dbConect.Settings.FirstOrDefault(x => x.SettingKey == key);
            if (setting == null)
            {
                setting = new Setting { SettingKey = key, SettingValue = value };
                _dbConect.Settings.Add(setting);
            }
            else
            {
                setting.SettingValue = value;
                _dbConect.Entry(setting).State = EntityState.Modified;
            }
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