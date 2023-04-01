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
    public class ContactInPersonsController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: ContactInPersons
        public ActionResult Index()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            var contactInPersons = db.ContactInPersons.Include(c => c.tb_contact).Include(c => c.tb_person);
            return View(contactInPersons.ToList());
        }

        // GET: ContactInPersons/Details/5
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
            ContactInPerson contactInPerson = db.ContactInPersons.Find(id);
            if (contactInPerson == null)
            {
                return HttpNotFound();
            }
            return View(contactInPerson);
        }

        // GET: ContactInPersons/Create
        public ActionResult Create()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            ViewBag.ContactInPersonID = new SelectList(db.tb_contact, "ContactID", "NationalCode");
            ViewBag.ContactInPersonID = new SelectList(db.tb_person, "PersonID", "NatioinalCode");
            return View();
        }

        // POST: ContactInPersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactInPersonID,personID,ContactID")] ContactInPerson contactInPerson)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.ContactInPersons.Add(contactInPerson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContactInPersonID = new SelectList(db.tb_contact, "ContactID", "NationalCode", contactInPerson.ContactInPersonID);
            ViewBag.ContactInPersonID = new SelectList(db.tb_person, "PersonID", "NatioinalCode", contactInPerson.ContactInPersonID);
            return View(contactInPerson);
        }

        // GET: ContactInPersons/Edit/5
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
            ContactInPerson contactInPerson = db.ContactInPersons.Find(id);
            if (contactInPerson == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactInPersonID = new SelectList(db.tb_contact, "ContactID", "NationalCode", contactInPerson.ContactInPersonID);
            ViewBag.ContactInPersonID = new SelectList(db.tb_person, "PersonID", "NatioinalCode", contactInPerson.ContactInPersonID);
            return View(contactInPerson);
        }

        // POST: ContactInPersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactInPersonID,personID,ContactID")] ContactInPerson contactInPerson)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(contactInPerson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContactInPersonID = new SelectList(db.tb_contact, "ContactID", "NationalCode", contactInPerson.ContactInPersonID);
            ViewBag.ContactInPersonID = new SelectList(db.tb_person, "PersonID", "NatioinalCode", contactInPerson.ContactInPersonID);
            return View(contactInPerson);
        }

        // GET: ContactInPersons/Delete/5
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
            ContactInPerson contactInPerson = db.ContactInPersons.Find(id);
            if (contactInPerson == null)
            {
                return HttpNotFound();
            }
            return View(contactInPerson);
        }

        // POST: ContactInPersons/Delete/5
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
            ContactInPerson contactInPerson = db.ContactInPersons.Find(id);
            db.ContactInPersons.Remove(contactInPerson);
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
