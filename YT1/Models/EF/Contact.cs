﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YT1.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : CommonAbs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }                 
        public string Name { get; set; }                
        public string WebSite { get; set; }         
        public string Email { get; set; }              
        public string Message { get; set; }           
        public bool IsRead { get; set; }        
    }
}