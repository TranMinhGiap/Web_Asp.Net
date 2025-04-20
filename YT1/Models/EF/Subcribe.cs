using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_Subcribe")]
    public class Subcribe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(150, ErrorMessage = "Email không được vượt quá 150 ký tự")]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }
        [Display(Name = "Ngày đăng ký")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}