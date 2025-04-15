using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace YT1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["HomNay"] = (long)0;
            Application["HomQua"] = (long)0;
            Application["TuanNay"] = (long)0;
            Application["TuanTruoc"] = (long)0;
            Application["ThangNay"] = (long)0;
            Application["ThangTruoc"] = (long)0;
            Application["TatCa"] = (long)0;
            Application["visitors_online"] = (uint)0;
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 40;
            Application.Lock();
            Application["visitors_online"] = Convert.ToUInt32(Application["visitors_online"]) + 1;
            try
            {
                var item = YT1.Models.Common.ThongKeTruyCap.ThongKe();
                if (item != null)
                {
                    Application["HomNay"] = item.HomNay;
                    Application["HomQua"] = item.HomQua;
                    Application["TuanNay"] = item.TuanNay;
                    Application["TuanTruoc"] = item.TuanTruoc;
                    Application["ThangNay"] = item.ThangNay;
                    Application["ThangTruoc"] = item.ThangTruoc;
                    Application["TatCa"] = item.TatCa;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Session_Start: {ex.Message}");
            }
            Application.UnLock();
        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            uint visitors = Convert.ToUInt32(Application["visitors_online"]);
            Application["visitors_online"] = visitors > 0 ? visitors - 1 : 0;
            Application.UnLock();
        }
    }
}
