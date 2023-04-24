using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using System.Reflection;
using TaxCollectData.Library.Abstraction;
using System.Data.Entity;

namespace ApiTax.Controllers
{
    [Authorize]
    public class InvoiceApiController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
        public User CurrentUser;
        public Client _Client;
        func func;
        public string memory_id = "";
      
        // GET: InvoiceApi
        public ActionResult Index()
        {


            return View();
        }
        public ActionResult UploadInvoice(int? type, int? sub_type, string title)
        {
            ViewBag.type_1 = type;
            ViewBag.type_2 = sub_type;
            ViewBag.page_title = title;
            return View();
        }

        [HttpPost]
        public JsonResult UploadInvoice(FormCollection formCollection)
        {

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            CurrentUser = GlobalUser.CurrentUser;



            var json = formCollection["data"];

            var data = JsonConvert.DeserializeObject<ApiObject>(json, settings);

                db.tb_send.Add(data.tb_send);
                db.SaveChanges();
  
            var list_check = new List<tb_check_send>();
        
            for (int x = 0; x < data.tb_check_send.Count(); x++)
            {
                data.tb_check_send[x].SendId = data.tb_send.SendId;
                    db.tb_check_send.Add(data.tb_check_send[x]);
                    db.SaveChanges();


                list_check.Add(data.tb_check_send[x]);
            }
            try
            {
                var inquiryResultModels = JsonConvert.DeserializeObject<List<InquiryResultModel>>(data.check_result, settings);

                foreach (var it in inquiryResultModels)
                {
                    var items = list_check.Where(r => r.UID == it.Uid);
                    if (items != null && items.Count() > 0)
                    {
                        var item = items.FirstOrDefault();
                        var response = it.Data.ToString();
                        var status = it.Status;
                        item.state = 1;
                        item.ResponseStatus = status;
                        item.CheckResponse = response;
                        item.CheckDate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch { }


            // var output = JsonConvert.SerializeObject(new );
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult edit(long id = 0, int type = 0)
        {
            ViewBag.id = id;
            ViewBag.type = type;
            return View();
        }
        public void init()
        {
            var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/FA.CER.CER"));
            func = new func();
            //  _api= func.initApi("A1211P", fileContents);
      

        }
   
        public ActionResult SendInvoice()
        {

            return View();
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
    public class MyValidation
    {
        public InvoiceMyValidation InvoiceMyValidation { get; set; }
        public InvoiceField InvoiceField { get; set; }
    }

    public class MyExportData
    {
        public List<ApiTax.Models.InvoiceDto> list { get; set; }
        public List<InvoiceMyValidation> list_error { get; set; }
        public string response { get; set; }
        public bool state { get; set; }
        public string message { get; set; }

    }


    public class ResponcePack
    {
        public string Uid { get; set; }
        public string ReferenceNumber { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetail { get; set; }
    }
    public class ExtraJsonData
    {
        public string ClientID { get; set; }
    }

    public class Users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    [Serializable]
    public class ApiObject
    {
        public tb_send tb_send { get; set; }
        public List<tb_check_send> tb_check_send { get; set; }

        public string json_result { get; set; }
        public string check_result { get; set; }
    }
}