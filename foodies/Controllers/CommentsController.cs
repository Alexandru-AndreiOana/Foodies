using foodies.Models;
using Microsoft.AspNet.Identity;
using RealProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealProject.Controllers
{

    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);

            if(comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message"] = "Comment " + "was deleted.";
                return Redirect("/Posts/Show/" + comm.PostId);
            }

            else
            {
                TempData["message"] = "You do not have the permission to delete this post:(";
                return RedirectToAction("Index", "Posts");
            }
        }
        
        [HttpPost]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    TempData["message"] = "The comment was added.";
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                else
                {
                    return View(comm);
                }
            }

            catch (Exception e)
            {
                return Redirect("/Post/Show/" + comm.PostId);
            }

        }

        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                ViewBag.Comment = comm;
                TempData["message"] = "Comment " + "was edited.";
                return View();
            }
            else
            {
                TempData["message"] = "You do not have the permission to edit this post:(";
                return RedirectToAction("Index", "Posts");
            }
        }

            [HttpPut]
            public ActionResult Edit(int id, Comment requestComment)
            {
                try
                {
                    Comment comm = db.Comments.Find(id);
                    if (TryUpdateModel(comm))
                    {
                        comm.Content = requestComment.Content;
                        db.SaveChanges();
                    }
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                catch (Exception e)
                {
                    return View();
                }

            }


        }
        
    }
