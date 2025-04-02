using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_ProductCategory")]
    public class ProductCategory : CommonAbs
    {
        public ProductCategory()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(150, ErrorMessage = "Tiêu đề không được vượt quá 150 ký tự")]
        public string Title { get; set; }
        public string Alias { get; set; }
        [StringLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
        public string Description { get; set; } 
        public string Icon { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoKeyWord { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}