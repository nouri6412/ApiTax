using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;

namespace ApiTax.Controllers
{
    public class UserBranchesController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: UserBranches
        public ActionResult Index(long UserID = 0)
        {
            var User = db.Users.Find(UserID);
            ViewBag.UserSelected = User;
            var userBranches = db.UserBranches.Where(r=>r.UserID== UserID).Include(u => u.User);
            return View(userBranches.ToList());
        }

        // GET: UserBranches/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBranch userBranch = db.UserBranches.Find(id);
            if (userBranch == null)
            {
                return HttpNotFound();
            }
            return View(userBranch);
        }

        // GET: UserBranches/Create
        public ActionResult Create(long UserID = 0)
        {
            ViewBag.UserID = UserID;
            var br = from br1 in db.Branches
                     join cl in db.Clients on br1.ClientID equals cl.ID
                     join pr in db.tb_person on cl.PersonId equals pr.PersonID
                     select new { BrancheID=br1.BranchID, BranchName=pr.Fname+" "+pr.Lname +" "+pr.ComapnyName +" - "+br1.BranchName };

            ViewBag.BrancheID = new SelectList(br, "BrancheID", "BranchName");
        

            return View();
        }

        // POST: UserBranches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,BrancheID,UserBranchId")] UserBranch userBranch)
        {
            if (ModelState.IsValid)
            {
                db.UserBranches.Add(userBranch);
                db.SaveChanges();
                return RedirectToAction("Index", new { UserID = userBranch.UserID });
            }
            var br = from br1 in db.Branches
                     join cl in db.Clients on br1.ClientID equals cl.ID
                     join pr in db.tb_person on cl.PersonId equals pr.PersonID
                     select new { BrancheID = br1.BranchID, BranchName = pr.Fname + " " + pr.Lname + " " + pr.ComapnyName + " - " + br1.BranchName };

            ViewBag.BrancheID = new SelectList(br, "BrancheID", "BranchName", userBranch.BrancheID);

            ViewBag.UserID = userBranch.UserID;
            return View(userBranch);
        }

        // GET: UserBranches/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBranch userBranch = db.UserBranches.Find(id);
            if (userBranch == null)
            {
                return HttpNotFound();
            }

            var br = from br1 in db.Branches
                     join cl in db.Clients on br1.ClientID equals cl.ID
                     join pr in db.tb_person on cl.PersonId equals pr.PersonID
                     select new { BrancheID = br1.BranchID, BranchName = pr.Fname + " " + pr.Lname + " " + pr.ComapnyName + " - " + br1.BranchName };

            ViewBag.BrancheID = new SelectList(br, "BrancheID", "BranchName", userBranch.BrancheID);
            return View(userBranch);
        }

        // POST: UserBranches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,BrancheID,UserBranchId")] UserBranch userBranch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userBranch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { UserID = userBranch.UserID });
            }
            var br = from br1 in db.Branches
                     join cl in db.Clients on br1.ClientID equals cl.ID
                     join pr in db.tb_person on cl.PersonId equals pr.PersonID
                     select new { BrancheID = br1.BranchID, BranchName = pr.Fname + " " + pr.Lname + " " + pr.ComapnyName + " - " + br1.BranchName };

            ViewBag.BrancheID = new SelectList(br, "BrancheID", "BranchName", userBranch.BrancheID);
            return View(userBranch);
        }

        // GET: UserBranches/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBranch userBranch = db.UserBranches.Find(id);
            if (userBranch == null)
            {
                return HttpNotFound();
            }
            return View(userBranch);
        }

        // POST: UserBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            UserBranch userBranch = db.UserBranches.Find(id);
            db.UserBranches.Remove(userBranch);
            db.SaveChanges();
            return RedirectToAction("Index", new { UserID = userBranch.UserID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
