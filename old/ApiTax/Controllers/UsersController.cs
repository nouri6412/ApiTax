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
 
    public class UsersController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: Users
        public ActionResult Index()
        {

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.BankID = new SelectList(db.tb_bank, "BankID", "title");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,NationalCode,Fname,LastName,Mobile,FatherName,Email,PassWord,IsActive,BankID,Shaba")] User user)
        {

            if (ModelState.IsValid)
            {
                user.StartActive = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                user.user_type = 2;
                db.Users.Add(user);
                db.SaveChanges();

                InitRequest InitRequest = new InitRequest();
                InitRequest.init(User);
                if (GlobalUser.isAdmin == false)
                {
                    return RedirectToAction("Index","Home",new { });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            ViewBag.BankID = new SelectList(db.tb_bank, "BankID", "title",user.BankID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankID = new SelectList(db.tb_bank, "BankID", "title", user.BankID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,NationalCode,Fname,LastName,Mobile,FatherName,Email,PassWord,IsActive,BankID,Shaba,StartActive,EndActive,user_type")] User user)
        {

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankID = new SelectList(db.tb_bank, "BankID", "title", user.BankID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
