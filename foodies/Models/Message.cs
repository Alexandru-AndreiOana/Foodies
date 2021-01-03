using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace foodies.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required(ErrorMessage = "Field must be non-empty")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
       

        public int GroupId { get; set; }
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Group group { get; set; }
       
    }
}