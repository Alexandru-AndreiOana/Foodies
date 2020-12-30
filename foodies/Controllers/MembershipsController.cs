using foodies.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace foodies.Controllers
{
    public class MembershipsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Memberships
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(int id)
        {
            
            Membership membership = new Membership();
            membership.GroupId = id;
            membership.MemberId = User.Identity.GetUserId();
            db.Memberships.Add(membership);
            db.SaveChanges();
            TempData["message"] = "U joined the group.";
            return View();
        }
    }
}