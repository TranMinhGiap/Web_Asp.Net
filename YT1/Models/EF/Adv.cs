using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_Adv")]
    public class Adv : CommonAbs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; } 
        public string Images { get; set; } 
        public int Type { get; set; } 
        public string Link { get; set; }
    }
}