using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_Category")]
    public class Category : CommonAbs
    {
        public Category() {
            this.News = new HashSet<News>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(250, ErrorMessage = "Title không được quá 250 ký tự")]
        public string Title { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        public string Alias { get; set; }
        public string Desc { get; set; }
        [Required(ErrorMessage = "Vị trí không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Vị trí phải là số dương")]
        public int Position { get; set; }
        [StringLength(500, ErrorMessage = "Seo Description không được quá 200 ký tự")]
        public string SeoTitle { get; set; }
        [StringLength(500, ErrorMessage = "Seo Description không được quá 200 ký tự")]
        public string SeoDesc { get; set; }
        [StringLength(500, ErrorMessage = "Seo Description không được quá 200 ký tự")]
        public string SeoKeyWord { get; set; }
        public bool IsActive { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Posts> Posts { get; set; }
    }
}