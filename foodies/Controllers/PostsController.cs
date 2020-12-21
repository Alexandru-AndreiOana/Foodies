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
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Article
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            var posts = db.Posts.Include("Tag").Include("User");
            ViewBag.Posts = posts;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
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
        [Authorize(Roles = "User,Editor, Admin")]
        public ActionResult New()
        {
            Post post = new Post();

            // preluam lista de categorii din metoda GetAllTags()
            post.Tagger = GetAllTags();
            post.UserId = User.Identity.GetUserId();
            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult New(Post post)
        {
            post.Date = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "The post was added.";
                    return RedirectToAction("Index");
                }       
                else
                {
                    post.Tagger = GetAllTags();
                    return View(post);
                }
            }

            catch (Exception e)
            {
                post.Tagger = GetAllTags();
                return View(post);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "The post titled " +
                post.Title + " was deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You do not have the permission to delete this post:(";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            post.Tagger = GetAllTags();

            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(post);
            }
            
            else 
            {
                TempData["message"] = "You do not have the permission to edit this post:(";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(post))
                        {
                            //post = requestPost;
                            post.Title = requestPost.Title;
                            post.Content = requestPost.Content;
                            post.Date = requestPost.Date;
                            post.TagId = requestPost.TagId;
                            db.SaveChanges();
                            TempData["message"] = "Post modified.";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "You do not have the permission to edit this post:(";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    requestPost.Tagger = GetAllTags();
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                requestPost.Tagger = GetAllTags();
                return View(requestPost);
            }
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllTags()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var tags = from tg in db.Tags
                             select tg;

            // iteram prin categorii
            foreach (var tag in tags)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = tag.TagId.ToString(),
                    Text = tag.TagName.ToString()
                });
            }
            /*
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.CategoryId.ToString();
                listItem.Text = category.CategoryName.ToString();

                selectList.Add(listItem);
            }*/

            // returnam lista de categorii
            return selectList;
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