using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealProject.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        [Required]
        public string TagName { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}