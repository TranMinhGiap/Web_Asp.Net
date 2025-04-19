using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YT1.Models;

namespace YT1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            var items = _dbConect.Roles.ToList();
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbConect));
                roleManager.Create(role);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo quyền. Vui lòng thử lại.");
            return View(role);
        }
        public ActionResult Update(string id) 
        {
            var item = _dbConect.Roles.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbConect));
                var existingRole = roleManager.FindById(role.Id);

                if (existingRole != null)
                {
                    existingRole.Name = role.Name;
                    var result = roleManager.Update(existingRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", string.Join(", ", result.Errors));
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy quyền để cập nhật.");
                }
            }

            return View(role);
        }

        // Delete
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var item = _dbConect.Roles.Find(id);
            if (item != null)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbConect));
                var result = roleManager.Delete(item);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = string.Join(", ", result.Errors) });
                }
            }
            return Json(new { success = false, message = "Không tìm thấy quyền cần xóa." });
        }

        // End Delete
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