﻿using System;
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
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}