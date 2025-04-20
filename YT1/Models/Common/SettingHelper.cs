using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YT1.Models.Common
{
    public class SettingHelper
    {
        private static ApplicationDbContext _dbConect = new ApplicationDbContext();
        public static string GetValue(string key)
        {
            var item = _dbConect.Settings.SingleOrDefault(x => x.SettingKey == key);
            if (item != null)
            {
                return item.SettingValue;
            }
            return "";
        }
    }
}