using foodies.Models;
using Microsoft.AspNet.Identity;
using RealProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace foodies.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: lista cu toate profilele
        /* public ActionResult Index(Profile profile)
         {
             //  var crtUser = UserManager.FindById(User.Identity.GetUserId());
             profile.UserId = User.Identity.GetUserId();
             var users = db.Users;
             ViewBag.Users = users;
             ViewBag.Profile = profile;
            /* foreach (ApplicationUser user in ViewBag.Users)
             {
                 Debug.WriteLine(user.Nickname);
             }

             return View();
         }
     */
        public ActionResult Index(string search)
        {
            var profile = new Profile();
            profile.UserId = User.Identity.GetUserId();
            ViewBag.Users = db.Users.Where(x => x.Nickname.StartsWith(search) || search == null).ToList();
            ViewBag.Profile = profile;

            return View();
        }

       


        // GET: vizualizarea unui profil anume
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Show(string id)
        {
           
          //  Profile profile = new Profile();
            ViewBag.User = db.Users.Find(id);
            ViewBag.UserPosts = db.Posts.Where(p => p.UserId == id).ToList();
            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.PrivateProfile = ViewBag.User.IsPrivate;

            /* if (ViewBag.User.IsPrivate == true)
             {
                TempData["message"] = "This profile is private:(";
                return RedirectToAction("Index");
             }
             */

            return View();
            //(User.IsInRole("Admin") == true) &&
        }




        public ActionResult ShowSelf(string id)
        {
            Profile profile = new Profile();
            ViewBag.User = db.Users.Find(id);
            ViewBag.UserPosts = db.Posts.Where(p => p.UserId == id).ToList();
            return View();
        }
        /*
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            SetAccesRights();

            return View(post);

        }


        */











    }
}