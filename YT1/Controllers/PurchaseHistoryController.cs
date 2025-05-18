using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YT1.Models;
using YT1.Models.Common;
using System.Data.Entity;

namespace YT1.Controllers
{
    [CustomAuthorize(Roles = "Admin, Customer")]
    public class PurchaseHistoryController : Controller
    {
        // GET: PurchaseHistory
        ApplicationDbContext _dbConect = new ApplicationDbContext();

        // Lấy thông tin user
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public PurchaseHistoryController()
        {
        }

        public PurchaseHistoryController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // end user
        public ActionResult Index()
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name).ConfigureAwait(false).GetAwaiter().GetResult();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var customerId = User.Identity.GetUserId();

            var listOrder = _dbConect.Orders
                .Where(o => o.CustomerId == customerId)
                .ToList();
            return PartialView(listOrder);
        }
        public ActionResult PurchaseDetails(string purchaseId)
        {
            ViewBag.PurchaseId = purchaseId;
            var order = _dbConect.Orders.FirstOrDefault(o => o.Code == purchaseId);
            if (order == null)
            {
                return HttpNotFound();
            }
            var orderDetails = _dbConect.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == order.Id)
                .ToList();

            return View(orderDetails);
        }
    }
}