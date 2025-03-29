using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_Setting")]
    public class Setting
    {
        [Key]
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string SettingDescription { get; set; }
    }
}