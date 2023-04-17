using ApiTax.Models;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace ApiTax.Controllers
{
    public class HomeController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
        public ActionResult Index()
        {


          
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User userr)
        {
            //if (ModelState.IsValid)
            //{
            if (IsValid(userr.NationalCode, userr.PassWord))
            {               
                FormsAuthentication.SetAuthCookie(userr.NationalCode, true);
     
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            return View(userr);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool IsValid(string email, string password)
        {
         
            bool IsValid = false;

            var user = db.Users.FirstOrDefault(u => u.NationalCode == email);
            if (user != null)
            {
                if (user.PassWord == password)
                {
                    IsValid = true;
                }
            }

            return IsValid;
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