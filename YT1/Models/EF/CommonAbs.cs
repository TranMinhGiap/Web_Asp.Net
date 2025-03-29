using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    public abstract class CommonAbs
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifierBy { get; set; }
        public DateTime ModifierDate { get; set; }
    }
}