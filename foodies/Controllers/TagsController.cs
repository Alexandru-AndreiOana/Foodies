using foodies.Models;
using RealProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealProject.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    public class TagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            var tags = from tag in db.Tags
                       orderby tag.TagName
                       select tag;
            ViewBag.Tags = tags;
            return View();
        }

        public ActionResult Show(int id)
        {
            Tag tag = db.Tags.Find(id);
            ViewBag.Tag = tag;
            return View();
        }

        [HttpPost]
        public ActionResult Show(Comment comm)
        {
            comm.Date = DateTime.Now;
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
                    return View(a);
                }

            }

            catch (Exception e)
            {
                Post a = db.Posts.Find(comm.PostId);
                return View(a);
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Tag tag)
        {
            try
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        
        public ActionResult Edit(int id)
        {
            Tag tag = db.Tags.Find(id);
            ViewBag.Tag = tag;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Tag requestTag)
        {
            try
            {
                Tag tag = db.Tags.Find(id);
                if (TryUpdateModel(tag))
                {
                    tag.TagName = requestTag.TagName;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
     
    }
}