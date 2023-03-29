using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;
using Newtonsoft.Json;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;

namespace ApiTax.Controllers
{
    public class tb_check_sendController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
        ITaxApis _api;

        // GET: tb_check_send
        public ActionResult Index(long? send_id)
        {
            var tb_check_send = db.tb_check_send.Where(r=>r.SendId==send_id).Include(t => t.tb_send);
            return View(tb_check_send.ToList());
        }

        public ActionResult CheckSend(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_check_send tb_check_send = db.tb_check_send.Find(id);
            if (tb_check_send == null)
            {
                return HttpNotFound();
            }

            var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/FA.CER.CER"));
            func func = new func();
            _api= func.initApi("A1211P", fileContents);

            func.check_send(new List<tb_check_send>() { tb_check_send },db);
            return RedirectToAction("Index",new { send_id = tb_check_send.SendId});
        }

        // GET: tb_check_send/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_check_send tb_check_send = db.tb_check_send.Find(id);
            if (tb_check_send == null)
            {
                return HttpNotFound();
            }
            return View(tb_check_send);
        }

        // GET: tb_check_send/Create
        public ActionResult Create()
        {
            ViewBag.SendId = new SelectList(db.tb_send, "SendId", "UID");
            return View();
        }

        // POST: tb_check_send/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SendCheckId,Response,CheckResponse,CheckDate,SendId,state,UID,ReferenceNumber")] tb_check_send tb_check_send)
        {
            if (ModelState.IsValid)
            {
                db.tb_check_send.Add(tb_check_send);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SendId = new SelectList(db.tb_send, "SendId", "UID", tb_check_send.SendId);
            return View(tb_check_send);
        }

        // GET: tb_check_send/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_check_send tb_check_send = db.tb_check_send.Find(id);
            if (tb_check_send == null)
            {
                return HttpNotFound();
            }
            ViewBag.SendId = new SelectList(db.tb_send, "SendId", "UID", tb_check_send.SendId);
            return View(tb_check_send);
        }

        // POST: tb_check_send/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SendCheckId,Response,CheckResponse,CheckDate,SendId,state,UID,ReferenceNumber")] tb_check_send tb_check_send)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_check_send).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SendId = new SelectList(db.tb_send, "SendId", "UID", tb_check_send.SendId);
            return View(tb_check_send);
        }

        // GET: tb_check_send/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_check_send tb_check_send = db.tb_check_send.Find(id);
            if (tb_check_send == null)
            {
                return HttpNotFound();
            }
            return View(tb_check_send);
        }

        // POST: tb_check_send/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_check_send tb_check_send = db.tb_check_send.Find(id);
            db.tb_check_send.Remove(tb_check_send);
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
