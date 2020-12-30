using foodies.Models;
using Microsoft.AspNet.Identity;
using RealProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace foodies.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Groups
        // returneaza lista de grupuri din care face parte user ul curent
        public ActionResult IndexSelf(string search)
        {
            return View();
        }

        //returneaza toate grupurile
        public ActionResult Index(string search)
        {
           /* Membership membership = new Membership();
            var userId = User.Identity.GetUserId();
            var correctMemberships = from grup in db.Memberships
                                     where grup.MemberId != userId
                                     select grup.GroupId;
            System.Diagnostics.Debug.WriteLine(correctMemberships);


            var correctGroups = from grup in db.Groups
                                where correctMemberships.Contains(grup.GroupId)
                                select grup;

            ViewBag.Groups = correctGroups.Where(x => x.Name.StartsWith(search) || search == null).ToList();
            
                */
            ViewBag.Groups = db.Groups.Where(x => x.Name.StartsWith(search) || search == null).ToList();
            
            return View();
        }
        
        [Authorize(Roles = "User,Editor, Admin")]
        public ActionResult New()
        {
            Group group = new Group();

            // preluam lista de categorii din metoda GetAllTags()
            // group.Tagger = GetAllTags();
            group.OwnerId = User.Identity.GetUserId();
            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult New(Group group)
        {
            group.OwnerId = User.Identity.GetUserId();
            //db.Groups.
            try
            {
                if (ModelState.IsValid)
                {
                    Membership membership = new Membership();
                    membership.GroupId = group.GroupId;
                    membership.MemberId = User.Identity.GetUserId();
                    db.Groups.Add(group);
                    db.Memberships.Add(membership);
                    db.SaveChanges();
                    //TempData["message"] = "The group was added."; merge dubios, se afiseaza la urmatorul redirect 
                    return RedirectToAction("Index");
                }
                else
                {
                    //group.Tagger = GetAllTags();
                    return View(group);
                }
            }

            catch (Exception e)
            {
                //group.Tagger = GetAllTags();
                return View(group);
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            SetAccesRights();

            return View(post);

        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(Comment comm)
        {
            comm.Date = DateTime.Now;
            comm.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    return Redirect("/Posts/Show/" + comm.PostId);
                }

                else
                {
                    Post a = db.Posts.Find(comm.PostId);
                    SetAccesRights();
                    return View(a);
                }

            }

            catch (Exception e)
            {
                Post a = db.Posts.Find(comm.PostId);
                SetAccesRights();
                return View(a);
            }

        }

        private void SetAccesRights()
        {
            ViewBag.showButtons = false;

            if (User.IsInRole("Admin") || User.IsInRole("User"))
            {
                ViewBag.showButtons = true;
            }

            ViewBag.isAdmin = User.IsInRole("Admin");
            ViewBag.crtUser = User.Identity.GetUserId();

        }
    }
}