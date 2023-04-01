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
    public class tb_contactController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: tb_contact
        public ActionResult Index()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            var tb_contact = db.tb_contact.Include(t => t.ContactInPerson).Include(t => t.tb_degree).Include(t => t.tb_magor);
            return View(tb_contact.ToList());
        }

        // GET: tb_contact/Details/5
        public ActionResult Details(long? id)
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
            tb_contact tb_contact = db.tb_contact.Find(id);
            if (tb_contact == null)
            {
                return HttpNotFound();
            }
            return View(tb_contact);
        }

        // GET: tb_contact/Create
        public ActionResult Create()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            ViewBag.ContactID = new SelectList(db.ContactInPersons, "ContactInPersonID", "ContactInPersonID");
            ViewBag.degreeID = new SelectList(db.tb_degree, "degreeID", "title");
            ViewBag.magorID = new SelectList(db.tb_magor, "magorID", "title");
            return View();
        }

        // POST: tb_contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactID,NationalCode,Fullname,tel,mobile,address,degreeID,magorID")] tb_contact tb_contact)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.tb_contact.Add(tb_contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContactID = new SelectList(db.ContactInPersons, "ContactInPersonID", "ContactInPersonID", tb_contact.ContactID);
            ViewBag.degreeID = new SelectList(db.tb_degree, "degreeID", "title", tb_contact.degreeID);
            ViewBag.magorID = new SelectList(db.tb_magor, "magorID", "title", tb_contact.magorID);
            return View(tb_contact);
        }

        // GET: tb_contact/Edit/5
        public ActionResult Edit(long? id)
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
            tb_contact tb_contact = db.tb_contact.Find(id);
            if (tb_contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactID = new SelectList(db.ContactInPersons, "ContactInPersonID", "ContactInPersonID", tb_contact.ContactID);
            ViewBag.degreeID = new SelectList(db.tb_degree, "degreeID", "title", tb_contact.degreeID);
            ViewBag.magorID = new SelectList(db.tb_magor, "magorID", "title", tb_contact.magorID);
            return View(tb_contact);
        }

        // POST: tb_contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactID,NationalCode,Fullname,tel,mobile,address,degreeID,magorID")] tb_contact tb_contact)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(tb_contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContactID = new SelectList(db.ContactInPersons, "ContactInPersonID", "ContactInPersonID", tb_contact.ContactID);
            ViewBag.degreeID = new SelectList(db.tb_degree, "degreeID", "title", tb_contact.degreeID);
            ViewBag.magorID = new SelectList(db.tb_magor, "magorID", "title", tb_contact.magorID);
            return View(tb_contact);
        }

        // GET: tb_contact/Delete/5
        public ActionResult Delete(long? id)
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
            tb_contact tb_contact = db.tb_contact.Find(id);
            if (tb_contact == null)
            {
                return HttpNotFound();
            }
            return View(tb_contact);
        }

        // POST: tb_contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            tb_contact tb_contact = db.tb_contact.Find(id);
            db.tb_contact.Remove(tb_contact);
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
