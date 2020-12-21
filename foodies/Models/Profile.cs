﻿using foodies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealProject.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public bool isPublic;
    }
}