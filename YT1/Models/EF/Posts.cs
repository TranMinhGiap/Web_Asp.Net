using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YT1.Models.EF
{
    [Table("tb_Posts")]
    public class Posts : CommonAbs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(250, ErrorMessage = "Tiêu đề không được quá 250 ký tự")]
        public string Title { get; set; }
        public string Alias { get; set; }
        public int CategoryId { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public string Images { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyWord { get; set; }
        public bool IsActive { get; set; }

        public virtual Category Category { get; set; }
    }
}