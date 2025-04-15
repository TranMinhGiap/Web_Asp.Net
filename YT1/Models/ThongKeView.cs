using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YT1.Models
{
    public class ThongKeView
    {
        public long HomNay { get; set; }
        public long HomQua { get; set; }
        public long TuanNay { get; set; }
        public long TuanTruoc { get; set; }
        public long ThangNay { get; set; }
        public long ThangTruoc { get; set; }
        public long TatCa { get; set; }
        public uint VisitorsOnline { get; set; }
    }
}