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
    public class InvoiceHeadersController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: InvoiceHeaders
        public ActionResult Index()
        {
            var invoiceHeaders = db.InvoiceHeaders.Include(i => i.ftType).Include(i => i.InPatern).Include(i => i.InsType);
            return View(invoiceHeaders.ToList());
        }

        // GET: InvoiceHeaders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            if (invoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Create
        public ActionResult Create()
        {
            ViewBag.ft = new SelectList(db.ftTypes, "ftID", "Title");
            ViewBag.Inp = new SelectList(db.InPaterns, "InpID", "Title");
            ViewBag.Ins = new SelectList(db.InsTypes, "Ins", "Title");
            return View();
        }

        // POST: InvoiceHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceID,Taxid,Inno,Irtaxid,Indatim,Indati2m,Inp,Tins,Ins,Sbc,Crn,Scln,Scc,spc,Tinb,Bid,Bbc,Billid,Bpc,ft,Bpn,Setm,Cap,Insp,tax17,Tvam,Tprdis,Tdis,Tadis,Todam,Tbill,tcpbs,tonw,torv,tocv,Tob,uuid,Inty,Tvop,Dpvb,InpID")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceHeaders.Add(invoiceHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ft = new SelectList(db.ftTypes, "ftID", "Title", invoiceHeader.ft);
            ViewBag.Inp = new SelectList(db.InPaterns, "InpID", "Title", invoiceHeader.Inp);
            ViewBag.Ins = new SelectList(db.InsTypes, "Ins", "Title", invoiceHeader.Ins);
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            if (invoiceHeader == null)
            {
                return HttpNotFound();
            }
            ViewBag.ft = new SelectList(db.ftTypes, "ftID", "Title", invoiceHeader.ft);
            ViewBag.Inp = new SelectList(db.InPaterns, "InpID", "Title", invoiceHeader.Inp);
            ViewBag.Ins = new SelectList(db.InsTypes, "Ins", "Title", invoiceHeader.Ins);
            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceID,Taxid,Inno,Irtaxid,Indatim,Indati2m,Inp,Tins,Ins,Sbc,Crn,Scln,Scc,spc,Tinb,Bid,Bbc,Billid,Bpc,ft,Bpn,Setm,Cap,Insp,tax17,Tvam,Tprdis,Tdis,Tadis,Todam,Tbill,tcpbs,tonw,torv,tocv,Tob,uuid,Inty,Tvop,Dpvb,InpID")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ft = new SelectList(db.ftTypes, "ftID", "Title", invoiceHeader.ft);
            ViewBag.Inp = new SelectList(db.InPaterns, "InpID", "Title", invoiceHeader.Inp);
            ViewBag.Ins = new SelectList(db.InsTypes, "Ins", "Title", invoiceHeader.Ins);
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            if (invoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            db.InvoiceHeaders.Remove(invoiceHeader);
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
