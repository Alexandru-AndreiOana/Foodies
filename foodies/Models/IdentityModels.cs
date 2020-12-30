using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RealProject.Models;

namespace foodies.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser clfile:///C:/Users/andre/Downloads/18.12.2020_DAW.rarass, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string Nickname { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new
            MigrateDatabaseToLatestVersion<ApplicationDbContext,
            foodies.Migrations.Configuration>("DefaultConnection"));
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Profile> Profiles { get; set;}
        public DbSet<Group> Groups { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}