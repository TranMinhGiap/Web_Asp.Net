using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YT1.Models
{
    public class CustomerView
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int PaymentMethodsl { get; set; }
    }
}