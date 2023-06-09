﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;
using PagedList;

namespace ApiTax.Controllers
{
    [Authorize]
    public class tb_sendController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();

        // GET: tb_send
        public ActionResult Index(int? page)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
           
            int pageSize = 15;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tb_send.Where(r => r.UserID == GlobalUser.CurrentUser.UserID).Include(t => t.User).Include(t => t.Client).OrderByDescending(r=>r.SendId).ToPagedList(pageIndex,pageSize);
            ViewBag.page_index = pageIndex;
            return View(list);
        }

        // GET: tb_send/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_send tb_send = db.tb_send.Find(id);
            if (tb_send == null)
            {
                return HttpNotFound();
            }

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            var ex = GlobalUser.UserBranches.Where(r=>r.Branch.Client.ID== tb_send.Client.ID);
            if(ex == null || ex.Count()==0)
            {
                return HttpNotFound();
            }

            return View(tb_send);
        }

        // GET: tb_send/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tb_send/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SendId,UID,InvoicePack,ResponcePack,CheckPack,SendDate,UserID,state")] tb_send tb_send)
        {
            //if (ModelState.IsValid)
            //{
            //    db.tb_send.Add(tb_send);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            return View(tb_send);
        }

        // GET: tb_send/Edit/5
        public ActionResult Edit(long? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //tb_send tb_send = db.tb_send.Find(id);
            //if (tb_send == null)
            //{
            //    return HttpNotFound();
            //}
            return View(new tb_send() { });
        }

        // POST: tb_send/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SendId,UID,InvoicePack,ResponcePack,CheckPack,SendDate,UserID,state")] tb_send tb_send)
        {
            if (ModelState.IsValid)
            {
           
                return RedirectToAction("Index");
            }
            return View(tb_send);
        }

        // GET: tb_send/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_send tb_send = db.tb_send.Find(id);
            if (tb_send == null)
            {
                return HttpNotFound();
            }
            return View(new tb_send() { });
        }

        // POST: tb_send/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_send tb_send = db.tb_send.Find(id);
            //db.tb_send.Remove(tb_send);
            //db.SaveChanges();
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
