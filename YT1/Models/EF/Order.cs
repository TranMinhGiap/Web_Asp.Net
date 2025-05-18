using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_Order")]
    public class Order : CommonAbs
    {
        public Order() { 
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Code { get; set; }
        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(50, ErrorMessage = "Tên khách hàng không được vượt quá 50 ký tự")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; } 
        public decimal TotalAmount { get; set; } 
        public int Quantity { get; set; } 
        public string CustomerId { get; set; }
        public int PaymentMethod { get; set; } 
        public int PaymentMethodVn { get; set;}
        public int Status { get; set; } 

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}