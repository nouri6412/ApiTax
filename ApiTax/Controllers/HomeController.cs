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
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;
using System.Threading.Tasks;

namespace ApiTax.Controllers
{
    public class HomeController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
        public  ActionResult Index()
        {

            var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/FA.CER.CER"));
            var path = Server.MapPath(@"~/App_Data/FA.CER.CER");


            TaxApiService.Instance.Init("A11ZTP", new Pkcs8SignatoryConfig(path, null),new NormalProperties(ClientType.SELF_TSP, ""), "https://tp.tax.gov.ir/req/api/self-tsp");

            ServerInformationModel serverInformation=  TaxApiService.Instance.TaxApis.GetServerInformation();

         
            //  TaxApiService.Instance.TaxApis.RequestToken();
        
            var json = "{Header:{Bid:'salam'}}";
            var dd = JsonConvert.DeserializeObject<TaxCollectData.Library.Dto.Content.InvoiceDto>(json);
            TaxCollectData.Library.Dto.Content.InvoiceDto invoiceDto = new TaxCollectData.Library.Dto.Content.InvoiceDto();
            
            var invoices = new List<TaxCollectData.Library.Dto.Content.InvoiceDto>
                 {
                      invoiceDto
                 };


            ITaxApis _api = TaxApiService.Instance.TaxApis;

            TokenModel token =  _api.RequestToken();

            var headers = new Dictionary<string, string>();

           // var result=  TaxApiService.Instance.TaxApis.SendInvoices(invoices, headers);
            var responseModel =  TaxApiService.Instance.TaxApis.SendInvoices(invoices, null);
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
    }


}