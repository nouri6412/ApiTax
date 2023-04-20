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
    public class tb_marketingController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: tb_marketing
        public ActionResult Index()
        {
            var tb_marketing = db.tb_marketing.Include(t => t.User);
            return View(tb_marketing.ToList());
        }

        // GET: tb_marketing/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_marketing tb_marketing = db.tb_marketing.Find(id);
            if (tb_marketing == null)
            {
                return HttpNotFound();
            }
            return View(tb_marketing);
        }

        // GET: tb_marketing/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "NationalCode");
            return View();
        }

        // POST: tb_marketing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MarketingID,UserID,NationalCode,title,note,RegDate")] tb_marketing tb_marketing)
        {
            if (ModelState.IsValid)
            {
                db.tb_marketing.Add(tb_marketing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            tb_marketing.RegDate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();

            ViewBag.UserID = new SelectList(db.Users, "UserID", "NationalCode", tb_marketing.UserID);
            return View(tb_marketing);
        }

        // GET: tb_marketing/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_marketing tb_marketing = db.tb_marketing.Find(id);
            if (tb_marketing == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "NationalCode", tb_marketing.UserID);
            return View(tb_marketing);
        }

        // POST: tb_marketing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MarketingID,UserID,NationalCode,title,note,RegDate")] tb_marketing tb_marketing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_marketing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "NationalCode", tb_marketing.UserID);
            return View(tb_marketing);
        }

        // GET: tb_marketing/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_marketing tb_marketing = db.tb_marketing.Find(id);
            if (tb_marketing == null)
            {
                return HttpNotFound();
            }
            return View(tb_marketing);
        }

        // POST: tb_marketing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_marketing tb_marketing = db.tb_marketing.Find(id);
            db.tb_marketing.Remove(tb_marketing);
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
