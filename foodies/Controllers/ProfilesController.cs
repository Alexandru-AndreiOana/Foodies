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
            Profile profile = db.Profiles.Where(p => p.UserId == id).FirstOrDefault();

            ViewBag.Profile = profile;

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

            // Profile profile = db.Profiles.Local.Where(p => p.UserId == id);

            Profile profile = db.Profiles.Where(p => p.UserId == id).FirstOrDefault();

            var UserId = User.Identity.GetUserId();

            if (profile.UserId == User.Identity.GetUserId())
            {
                ViewBag.UserPosts = db.Posts.Where(p => p.UserId == UserId).ToList();
                ViewBag.User = db.Users.Find(id);
                ViewBag.Profile = profile;
                return View(profile);
            }
            else
            {
                return View();
            }
        }



        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Edit(int id)
        {
            Profile profile = db.Profiles.Find(id);
            //post.Tagger = GetAllTags();

            var UserId = User.Identity.GetUserId();

            if (profile.UserId == User.Identity.GetUserId())
            {
                return View(profile);
            }

            else
            {
               // TempData["message"] = "You do not have the permission to edit this post:(";
                return RedirectToAction("ShowSelf/" + UserId, "Profiles");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Edit(int id, Profile requestProfile)
        {
            var UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {

                    Profile profile = db.Profiles.Find(id);
                    if (profile.UserId == User.Identity.GetUserId())
                    {
                        if (TryUpdateModel(profile))
                        {
                            //post = requestPost;
                            profile.ProfileName = requestProfile.ProfileName;
                            profile.Description = requestProfile.Description;
                            profile.FavouriteFood = requestProfile.FavouriteFood;
                            db.SaveChanges();
                        }
                        return RedirectToAction("ShowSelf/" + UserId, "Profiles");
                    }
                    else
                    {
                        //TempData["message"] = "You do not have the permission to edit this post:(";
                        return RedirectToAction("ShowSelf/" + UserId, "Profiles");
                    }
                }
                else
                {
                    //requestProfile.Tagger = GetAllTags();
                    return View(requestProfile);
                }
            }
            catch (Exception e)
            {
                //requestProfile.Tagger = GetAllTags();
                return View(requestProfile);
            }
        }

        /*
        [Authorize(Roles = "User,Editor, Admin")]
        public ActionResult New()
        {
            Profile profile = new Profile();

            
            profile.UserId = User.Identity.GetUserId();
            return View(profile);
        }

        [HttpPost]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult New(Profile profile)
        {
            profile.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Profiles.Add(profile);
                    db.SaveChanges();
                    TempData["message"] = "Profile was created";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(profile);
                }
            }

            catch (Exception e)
            {
                return View(profile);
            }
        }
        
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