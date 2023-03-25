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
    public class tb_personController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: tb_person
        public ActionResult Index(int type=0)
        {
            ViewBag.type = type;
            var tb_person = db.tb_person.Include(t => t.CompanyType).Include(t => t.PersonType);
            return View(tb_person.ToList());
        }

        // GET: tb_person/Details/5
        public ActionResult Details(long? id, int type = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_person tb_person = db.tb_person.Find(id);
            if (tb_person == null)
            {
                return HttpNotFound();
            }

            ViewBag.type = type;
            return View(tb_person);
        }

        // GET: tb_person/Create
        public ActionResult Create( int type = 0)
        {
            ViewBag.CompanyTypeId = new SelectList(db.CompanyTypes, "CompanyTypeId", "CompanyTypeName");
            ViewBag.PersonTypeId = new SelectList(db.PersonTypes, "PersonTypeID", "Title");

            ViewBag.type = type;
            return View();
        }

        // POST: tb_person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,NatioinalCode,PersonTypeId,Fname,Lname,FatherName,BirthDate,Address,Phone,Mobile,ComapnyName,Zipcode,RegisterationDate,CompanyTypeId,RegisterNumber")] tb_person tb_person)
        {
            int type = 0;
            if (tb_person.PersonTypeId == 2)
            {
                type = 1;
            }

            if (ModelState.IsValid)
            {
                db.tb_person.Add(tb_person);
                db.SaveChanges();
                return RedirectToAction("Index",new {type= type });
            }

            ViewBag.CompanyTypeId = new SelectList(db.CompanyTypes, "CompanyTypeId", "CompanyTypeName", tb_person.CompanyTypeId);
            ViewBag.PersonTypeId = new SelectList(db.PersonTypes, "PersonTypeID", "Title", tb_person.PersonTypeId);

  

            ViewBag.type = type;
            return View(tb_person);
        }

        // GET: tb_person/Edit/5
        public ActionResult Edit(long? id, int type = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_person tb_person = db.tb_person.Find(id);
            if (tb_person == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyTypeId = new SelectList(db.CompanyTypes, "CompanyTypeId", "CompanyTypeName", tb_person.CompanyTypeId);
            ViewBag.PersonTypeId = new SelectList(db.PersonTypes, "PersonTypeID", "Title", tb_person.PersonTypeId);

            ViewBag.type = type;
            return View(tb_person);
        }

        // POST: tb_person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,NatioinalCode,PersonTypeId,Fname,Lname,FatherName,BirthDate,Address,Phone,Mobile,ComapnyName,Zipcode,RegisterationDate,CompanyTypeId,RegisterNumber")] tb_person tb_person)
        {
            int type = 0;
            if (tb_person.PersonTypeId == 2)
            {
                type = 1;
            }
            if (ModelState.IsValid)
            {
                db.Entry(tb_person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { type = type });
            }
            ViewBag.CompanyTypeId = new SelectList(db.CompanyTypes, "CompanyTypeId", "CompanyTypeName", tb_person.CompanyTypeId);
            ViewBag.PersonTypeId = new SelectList(db.PersonTypes, "PersonTypeID", "Title", tb_person.PersonTypeId);



            ViewBag.type = type;
            return View(tb_person);
        }

        // GET: tb_person/Delete/5
        public ActionResult Delete(long? id, int type = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_person tb_person = db.tb_person.Find(id);
            if (tb_person == null)
            {
                return HttpNotFound();
            }
            ViewBag.type = type;
            return View(tb_person);
        }

        // POST: tb_person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {


            tb_person tb_person = db.tb_person.Find(id);
            int type = 0;
            if (tb_person.PersonTypeId == 2)
            {
                type = 1;
            }
            db.tb_person.Remove(tb_person);
            db.SaveChanges();

            return RedirectToAction("Index", new { type = type });
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
