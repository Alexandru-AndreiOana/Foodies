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

    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(int id)
        {
            Message msg = db.Messages.Find(id);

            if (msg.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Messages.Remove(msg);
                db.SaveChanges();
                TempData["message"] = "Message was deleted.";
                return Redirect("/Groups/Show/" + msg.GroupId);
            }

            else
            {
                TempData["message"] = "You do not have the permission to delete this message:(";
                return RedirectToAction("Group", "Show");
            }
        }

        [HttpPost]
        public ActionResult New(Message msg)
        {
            msg.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Messages.Add(msg);
                    db.SaveChanges();
                    TempData["message"] = "The message was added.";
                    return Redirect("/Group/Show/" + msg.GroupId);
                }
                else
                {
                    return View(msg);
                }
            }

            catch (Exception e)
            {
                return Redirect("/Group/Show/" + msg.GroupId);
            }

        }

        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int id)
        {
            Message msg = db.Messages.Find(id);
            if (msg.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                ViewBag.Message = msg;
                TempData["message"] = "Message " + "was edited.";
                return View();
            }
            else
            {
                TempData["message"] = "You do not have the permission to edit this message(";
                return RedirectToAction("Groups", "Show");
            }
        }

        [HttpPut]
        public ActionResult Edit(int id, Message requestMessage)
        {
            try
            {
                Message msg = db.Messages.Find(id);
                if (TryUpdateModel(msg))
                {
                    msg.Content = requestMessage.Content;
                    db.SaveChanges();
                }
                return Redirect("/Groups/Show/" + msg.GroupId);
            }
            catch (Exception e)
            {
                return View();
            }

        }


    }

}
