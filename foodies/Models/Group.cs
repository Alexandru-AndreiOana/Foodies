using RealProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace foodies.Models
{
    public class Group
    {

        [Key]
        public int GroupId { get; set; }

        public string OwnerId { get; set; }

        [Required(ErrorMessage = "The Group Name is required.")]
        [StringLength(100, ErrorMessage = "The Group Name can't have more than 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A group description is required")]
        [DataType(DataType.MultilineText)]
        public string About { get; set; }
        
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}