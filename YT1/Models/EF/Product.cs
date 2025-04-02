using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YT1.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbs 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(250, ErrorMessage = "Tên sản phẩm không được vượt quá 250 ký tự")]
        public string Title { get; set; }
        public string Alias { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được quá 50 ký tự")]
        public string ProductCode { get; set; }
        [StringLength(250, ErrorMessage = "Tên sản phẩm không được vượt quá 250 ký tự")]
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public string Images { get; set; }
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải lớn hơn hoặc bằng 0")]
        public decimal PriceSale { get; set; }
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }
        public int ProductCategoryId { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyWord { get; set; }
        public bool IsActive { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}