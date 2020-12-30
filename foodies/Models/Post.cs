using foodies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealProject.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The Title is required.")]
        [StringLength(100, ErrorMessage = "The title can't have more than 20 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Some content is required")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "A tag is required.")]
        public int TagId { get; set; }
        //public int ProfileId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Tag Tag { get; set; }
        //public virtual Profile Profile { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public IEnumerable<SelectListItem> Tagger { get; set; }

        public virtual Group group { get; set; }
    }
   /* public class PostDBContext : DbContext
    {
        public PostDBContext() : base("DBConnectionString") { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    } */
}