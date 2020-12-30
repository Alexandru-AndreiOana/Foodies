using RealProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace foodies.Models
{
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string MemberId { get; set; }

        public IEnumerable<SelectListItem> usr { get; set; }
        
        public virtual Group group { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}