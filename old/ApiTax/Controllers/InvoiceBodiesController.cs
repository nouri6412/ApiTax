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
    public class InvoiceBodiesController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: InvoiceBodies
        public ActionResult Index()
        {
            var invoiceBodies = db.InvoiceBodies.Include(i => i.InvoiceHeader);
            return View(invoiceBodies.ToList());
        }

        // GET: InvoiceBodies/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceBody invoiceBody = db.InvoiceBodies.Find(id);
            if (invoiceBody == null)
            {
                return HttpNotFound();
            }
            return View(invoiceBody);
        }

        // GET: InvoiceBodies/Create
        public ActionResult Create()
        {
            ViewBag.Taxid = new SelectList(db.InvoiceHeaders, "InvoiceID", "Taxid");
            return View();
        }

        // POST: InvoiceBodies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceBodyID,Taxid,Sstid,Am,Fee,Cfee,Cut,Exr,Prdis,Dis,Adis,Vra,Vam,Odt,Odr,Odam,Olt,Olr,Olam,Concfee,Spro,Bros,Tcpbs,Vop,Sstt,MuS,Tsstam,Bsrn,Cop")] InvoiceBody invoiceBody)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceBodies.Add(invoiceBody);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Taxid = new SelectList(db.InvoiceHeaders, "InvoiceID", "Taxid", invoiceBody.Taxid);
            return View(invoiceBody);
        }

        // GET: InvoiceBodies/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceBody invoiceBody = db.InvoiceBodies.Find(id);
            if (invoiceBody == null)
            {
                return HttpNotFound();
            }
            ViewBag.Taxid = new SelectList(db.InvoiceHeaders, "InvoiceID", "Taxid", invoiceBody.Taxid);
            return View(invoiceBody);
        }

        // POST: InvoiceBodies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceBodyID,Taxid,Sstid,Am,Fee,Cfee,Cut,Exr,Prdis,Dis,Adis,Vra,Vam,Odt,Odr,Odam,Olt,Olr,Olam,Concfee,Spro,Bros,Tcpbs,Vop,Sstt,MuS,Tsstam,Bsrn,Cop")] InvoiceBody invoiceBody)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceBody).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Taxid = new SelectList(db.InvoiceHeaders, "InvoiceID", "Taxid", invoiceBody.Taxid);
            return View(invoiceBody);
        }

        // GET: InvoiceBodies/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceBody invoiceBody = db.InvoiceBodies.Find(id);
            if (invoiceBody == null)
            {
                return HttpNotFound();
            }
            return View(invoiceBody);
        }

        // POST: InvoiceBodies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InvoiceBody invoiceBody = db.InvoiceBodies.Find(id);
            db.InvoiceBodies.Remove(invoiceBody);
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
