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
        public string Title { get; set; } 
        public string Desc { get; set; } 
        public int Position { get; set; } 
        public string SeoTitle { get; set; } 
        public string SeoDesc { get; set; } 
        public string SeoKeyWord { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Posts> Posts { get; set; }
    }
}