using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YT1.Models.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var url = filterContext.HttpContext.Request.Url?.AbsolutePath.ToLower();

                if (url != null && url.StartsWith("/admin"))
                {
                    filterContext.Result = new RedirectResult("~/Admin/Account/Login");
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }
            else
            {
                // Nếu đã login nhưng không có quyền, trả về 403
                filterContext.Result = new HttpStatusCodeResult(403);
            }
        }
    }
}