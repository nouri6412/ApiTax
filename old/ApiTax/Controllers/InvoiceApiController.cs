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

namespace ApiTax.Controllers
{

    public class InvoiceApiController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
        public User CurrentUser;
        public Client _Client;
        func func;
        public string memory_id = "";
        ITaxApis _api;
        // GET: InvoiceApi
        public ActionResult Index()
        {


            return View();
        }

        [Authorize]
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
            try
            {

                var userJson = formCollection["user"];

                InitRequest InitRequest = new InitRequest();
                InitRequest.init(User, userJson);

                CurrentUser = GlobalUser.CurrentUser;

              

                var json = formCollection["excel_data"];
                List<ApiTax.Models.InvoiceBodyDto> _body = JsonConvert.DeserializeObject<List<ApiTax.Models.InvoiceBodyDto>>(json);
                List<ApiTax.Models.InvoiceHeaderDto> _Header = JsonConvert.DeserializeObject<List<ApiTax.Models.InvoiceHeaderDto>>(json);
                List<ApiTax.Models.PaymentDto> _Payments = JsonConvert.DeserializeObject<List<ApiTax.Models.PaymentDto>>(json);
                List<ExtraJsonData> _ExtraJsonData = JsonConvert.DeserializeObject<List<ExtraJsonData>>(json);
                memory_id = _ExtraJsonData[0].ClientID;


                var ex_client = db.Clients.Where(r => r.ClientID == memory_id);

                if (ex_client == null || ex_client.Count() == 0)
                {
                    var error = JsonConvert.SerializeObject(new MyExportData() { state = false, message = "حافظه مالیاتی یافت نشد" });
                    return Json(error, JsonRequestBehavior.AllowGet);
                }

                _Client = ex_client.FirstOrDefault();

                if (_ExtraJsonData[0].PKEY != null && _ExtraJsonData[0].PKEY.Trim().Length > 0)
                {
                    _Client.PrivateKey = _ExtraJsonData[0].PKEY.Trim();
                }



                init();

                var random = new Random();

                int type_1 = Convert.ToInt32(formCollection["type_1"]);
                int type_2 = Convert.ToInt32(formCollection["type_2"]);


                for (int x = 0; x < _Header.Count(); x++)
                {
                    #region ضربدر ها

                    try
                    {
                        _body[x].Prdis = Math.Floor(Convert.ToDecimal(Convert.ToDecimal(_body[x].Am) * _body[x].Fee));
                    }
                    catch { }

                    ////

                    try
                    {
                        _body[x].Adis = Math.Floor(Convert.ToDecimal(_body[x].Prdis - _body[x].Dis));
                    }
                    catch { }




                    ////
                    ///

                    try
                    {
                        if (type_1 == 1 && type_2 == 3)
                        {
                            ///الگوی طلا و جواهر
                            _body[x].Odam = Math.Floor(Convert.ToDecimal((_body[x].Consfee + _body[x].Spro + _body[x].Bros)));
                        }


                    }
                    catch { }

                    try
                    {
                        if (type_1 == 1 && type_2 == 7)
                        {
                            ///صادراتی از اکسل بر می دارد
                        }
                        else if (type_1 == 1 && type_2 == 3)
                        {
                            ///الگوی طلا و جواهر
                            _body[x].Odam = Math.Floor(Convert.ToDecimal((_body[x].Tcpbs * _body[x].Odr) / 100));
                        }
                        else
                        {
                            _body[x].Odam = Math.Floor(Convert.ToDecimal((_body[x].Adis * _body[x].Odr) / 100));
                        }


                    }
                    catch { }

                    try
                    {
                        if (type_1 == 1 && type_2 == 7)
                        {
                            ///صادراتی از اکسل بر می دارد
                        }
                        else if (type_1 == 1 && type_2 == 3)
                        {
                            ///الگوی طلا و جواهر
                            _body[x].Odam = Math.Floor(Convert.ToDecimal((_body[x].Tcpbs * _body[x].Olr) / 100));
                        }
                        else
                        {
                            _body[x].Olam = Math.Floor(Convert.ToDecimal((_body[x].Adis * _body[x].Olr) / 100));
                        }
                    }
                    catch { }

                    ////

                    if (type_1 == 1 && type_2 == 3)
                    {
                        try
                        {
                            _body[x].Adis = Math.Floor(Convert.ToDecimal(_body[x].Prdis + _body[x].Consfee + _body[x].Spro + _body[x].Bros - _body[x].Dis));
                        }
                        catch { }
                    }

                    ////

                    try
                    {
                        decimal Vam = Math.Floor(Convert.ToDecimal((_body[x].Adis * _body[x].Vra) / 100));
                        _body[x].Vam = Vam;
                    }
                    catch { }

                    try
                    {
                        if (type_1 == 1 && type_2 == 7)
                        {

                        }
                        else if (type_1 == 1 && type_2 == 6)
                        {
                            _body[x].Tsstam = Math.Floor(Convert.ToDecimal((_body[x].Fee + _body[x].Vam)));

                        }
                        else
                        {
                            _body[x].Tsstam = Math.Floor(Convert.ToDecimal((_body[x].Vam + _body[x].Adis + _body[x].Odam + _body[x].Olam)));
                        }
                    }
                    catch { }

                    try
                    {
                        _body[x].Cop = Math.Floor(Convert.ToDecimal((_body[x].Tsstam * _Header[x].Cap) / _Header[x].Tadis));
                    }
                    catch { }


                    try
                    {
                        _body[x].Vop = Math.Floor(Convert.ToDecimal((_body[x].Vam * _Header[x].Cap) / _Header[x].Tadis));
                    }
                    catch { }



                    #endregion
                } ///body

                //for (int x = 0; x < _Header.Count(); x++)
                //{

                //}///payment

                for (int x = 0; x < _Header.Count(); x++)
                {

                    long randomSerialDecimal = random.Next(999999999);
                    var now = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                    string date1 = "";
                    try
                    {
                        date1 = _Header[x].Indatim.ToString().Substring(0, 4) + "/" + _Header[x].Indatim.ToString().Substring(4, 2) + "/" + _Header[x].Indatim.ToString().Substring(6, 2);
                    }
                    catch
                    {
                        MyExportData MyExportData1 = new MyExportData() { };

                        MyExportData1.state = false;
                        MyExportData1.message = "{\"error\":\"invalid data in row " + (x + 1) + "\"}";

                        var output1 = JsonConvert.SerializeObject(MyExportData1);
                        return Json(output1, JsonRequestBehavior.AllowGet);
                    }
                    int Hours = DateTime.Now.Hour;
                    int Minutes = DateTime.Now.Minute;

                    int Seconds = DateTime.Now.Second;
                    DateTime Indatim = utility.ToMiladi(date1).AddHours(Hours).AddMinutes(Minutes).AddSeconds(Seconds);

                    var now2 = new DateTimeOffset(Indatim).ToUnixTimeMilliseconds();
                    var taxId = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(memory_id, randomSerialDecimal, Indatim);
                                
                    _Header[x].Taxid = taxId;
                    _Header[x].Indati2m = now;
                    _Header[x].Indatim = now2;
                    _Header[x].Tdis = 0;

                    if (type_1 == 1 && type_2 == 7)
                    {
                        string Cdcd = "";
                        Cdcd = _Header[x].Cdcd.ToString().Substring(0, 4) + "/" + _Header[x].Cdcd.ToString().Substring(4, 2) + "/" + _Header[x].Cdcd.ToString().Substring(6, 2);
                        DateTime Cdcd1 = utility.ToMiladi(Cdcd);
                        var now3 = new DateTimeOffset(Cdcd1).ToUnixTimeSeconds();
                        int xx = (int)((Int64)now3 / 86400);
                        _Header[x].Cdcd = xx;
                    }

                }

                string inno = "0";

                List<InvoiceMyValidation> _InvoiceMyValidation = new List<InvoiceMyValidation>();
                List<ApiTax.Models.InvoiceDto> list = new List<ApiTax.Models.InvoiceDto>();
                var list_detail = new List<ApiTax.Models.InvoiceBodyDto>();
                var list_pay = new List<ApiTax.Models.PaymentDto>();
                for (int x = 0; x < _Header.Count(); x++)
                {
                    Boolean is_added = false;
                    Boolean is_added_detail = false;

                    if ((inno != _Header[x].Inno && inno != "0") || x == _Header.Count() - 1)
                    {
                        if (x == _Header.Count() - 1 && inno == _Header[x].Inno)
                        {
                            list_detail.Add(_body[x]);
                            list_pay.Add(_Payments[x]);
                            is_added_detail = true;
                        }

                        ApiTax.Models.InvoiceDto _InvoiceDto = new ApiTax.Models.InvoiceDto() { };

                        if (x > 0)
                        {
                            _InvoiceDto.Header = _Header[x - 1];
                            _InvoiceDto.Payments = new List<Models.PaymentDto>() { _Payments[x - 1] };// روش header
                        }
                        else
                        {
                            _InvoiceDto.Header = _Header[x];
                            _InvoiceDto.Payments = new List<Models.PaymentDto>() { _Payments[x] };// روش header
                        }

                        _InvoiceDto.Body = list_detail;
                        // _InvoiceDto.extension = new Extension();
                        //  _InvoiceDto.Payments = list_pay;// روش body

                        if (x != _Header.Count() - 1)
                        {
                            list_detail = new List<ApiTax.Models.InvoiceBodyDto>();
                            list_pay = new List<ApiTax.Models.PaymentDto>();
                        }
                        is_added = true;
                        list.Add(_InvoiceDto);
                    }
                    if (is_added_detail == false)
                    {
                        list_detail.Add(_body[x]);
                        list_pay.Add(_Payments[x]);
                    }

                    if (x == _Header.Count() - 1 && inno != _Header[x].Inno && is_added == false)
                    {
                        ApiTax.Models.InvoiceDto _InvoiceDto = new ApiTax.Models.InvoiceDto() { };

                        list_detail = new List<ApiTax.Models.InvoiceBodyDto>();

                        list_detail.Add(_body[x]);

                        _InvoiceDto.Body = list_detail;

                        _InvoiceDto.Header = _Header[x];
                        _InvoiceDto.Payments = new List<Models.PaymentDto>() { _Payments[x] };// روش header
                        list.Add(_InvoiceDto);
                    }

                    inno = _Header[x].Inno;

                    //int? type_1 = _Header[x].Inty;
                    //int? type_2 = _Header[x].Inp;


                    _Header[x].Inty = type_1;

                    _Header[x].Inp = type_2;
                    string _sbc = _Header[x].Sbc;


                    string ClientID = _ExtraJsonData[x].ClientID;

                    foreach (PropertyInfo propertyInfo in _Header[x].GetType().GetProperties())
                    {
                        MyValidation MyValidation = getValidation(propertyInfo, x, type_1, type_2, _Header[x], ClientID, _sbc);
                        if (MyValidation.InvoiceMyValidation.row > -1)
                        {
                            _InvoiceMyValidation.Add(MyValidation.InvoiceMyValidation);
                        }
                    }

                    foreach (PropertyInfo propertyInfo in _body[x].GetType().GetProperties())
                    {
                        MyValidation MyValidation = getValidation(propertyInfo, x, type_1, type_2, _body[x], ClientID, _sbc);
                        if (MyValidation.InvoiceMyValidation.row > -1)
                        {
                            _InvoiceMyValidation.Add(MyValidation.InvoiceMyValidation);
                        }
                    }

                    foreach (PropertyInfo propertyInfo in _Payments[x].GetType().GetProperties())
                    {
                        MyValidation MyValidation = getValidation(propertyInfo, x, type_1, type_2, _Payments[x], ClientID, _sbc);
                        if (MyValidation.InvoiceMyValidation.row > -1)
                        {
                            _InvoiceMyValidation.Add(MyValidation.InvoiceMyValidation);
                        }
                    }


                }

                if (list.Count() > 10)
                {
                    MyExportData MyExportData1 = new MyExportData() { };

                    MyExportData1.state = false;
                    MyExportData1.message = "{\"error\":\"تعداد فاکتور های مجاز برای ارسال 10 می باشد \"}";

                    var output1 = JsonConvert.SerializeObject(MyExportData1);
                    return Json(output1, JsonRequestBehavior.AllowGet);
                }

                foreach (var item in list)
                {
                    #region جمع ها  
                    try
                    {
                        if (type_1 == 1 && type_2 == 7)
                        {
                            /// صادرات از اکسل می گیرد
                        }
                        else
                        {
                            item.Header.Tbill = 0;
                        }

                        foreach (var it in item.Body)
                        {
                            try
                            {
                                if (type_1 == 1 && type_2 == 7)
                                {
                                    ///صادراتی از اکسل بر می دارد
                                }
                                else
                                {
                                    item.Header.Tbill += Convert.ToDecimal(it.Tsstam);
                                }
                            }
                            catch { }
                        }
                        item.Header.Tbill = Math.Floor(Convert.ToDecimal(item.Header.Tbill));
                    }
                    catch { }

                    try
                    {
                        item.Header.Tvop = 0;
                        foreach (var it in item.Body)
                        {
                            try
                            {
                                item.Header.Tvop += Convert.ToDecimal(it.Vop);
                            }
                            catch { }
                        }
                        item.Header.Tvop = Math.Floor(Convert.ToDecimal(item.Header.Tvop));

                    }
                    catch { }

                    try
                    {
                        item.Header.Tdis = 0;
                        foreach (var it in item.Body)
                        {
                            try
                            {
                                item.Header.Tdis += Convert.ToDecimal(it.Dis);
                            }
                            catch { }
                        }
                        item.Header.Tdis = Math.Floor(Convert.ToDecimal(item.Header.Tdis));
                    }
                    catch { }

                    try
                    {
                        item.Header.Tprdis = 0;
                        foreach (var it in item.Body)
                        {
                            try
                            {
                                item.Header.Tprdis += Convert.ToDecimal(it.Prdis);
                            }
                            catch { }
                        }
                        item.Header.Tprdis = Math.Floor(Convert.ToDecimal(item.Header.Tprdis));
                    }
                    catch { }

                    try
                    {
                        item.Header.Tvam = 0;
                        foreach (var it in item.Body)
                        {
                            try
                            {
                                item.Header.Tvam += Convert.ToDecimal(it.Vam);
                            }
                            catch { }
                        }
                        item.Header.Tvam = Math.Floor(Convert.ToDecimal(item.Header.Tvam));
                    }
                    catch { }

                    try
                    {
                        item.Header.Tadis = 0;
                        foreach (var it in item.Body)
                        {
                            try
                            {
                                item.Header.Tadis += Convert.ToDecimal(it.Adis);
                            }
                            catch { }
                        }
                        item.Header.Tadis = Math.Floor(Convert.ToDecimal(item.Header.Tadis));
                    }
                    catch { }

                    try
                    {
                        item.Header.Todam = 0;
                        foreach (var it in item.Body)
                        {
                            try
                            {
                                item.Header.Todam += Convert.ToDecimal(it.Odam);
                            }
                            catch { }
                        }
                        item.Header.Todam = Math.Floor(Convert.ToDecimal(item.Header.Todam));

                    }
                    catch { }

                    try
                    {
                        item.Header.Cap = item.Header.Tbill - item.Header.Insp - item.Header.Tvam - item.Header.Todam;
                        item.Header.Cap = Math.Floor(Convert.ToDecimal(item.Header.Cap));
                    }
                    catch { }


                    #endregion
                }

                MyExportData MyExportData = new MyExportData() { list = list, list_error = _InvoiceMyValidation };

                MyExportData.response = "{\"error\":\"invalid data\"}";

                if (MyExportData.list_error.Count() == 0)
                {
                    MyExportData.response = send_invoice(list);
                }
                else
                {
                    MyExportData.state = false;
                    MyExportData.message = "{\"error\":\"invalid data\"}";
                }

                var output = JsonConvert.SerializeObject(MyExportData);
                return Json(output, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                MyExportData MyExportData1 = new MyExportData() { };

                MyExportData1.state = false;
                MyExportData1.message = "{\"error\":\"" + ex.Message + "\"}";

                var output1 = JsonConvert.SerializeObject(MyExportData1);
                return Json(output1, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult edit(long id = 0, int type = 0)
        {
            ViewBag.id = id;
            ViewBag.type = type;
            return View();
        }
        public ActionResult Confirmedit(string _user = null, long id = 0, int type = 0)
        {
            tb_check_send tb_check_send = db.tb_check_send.Where(r => r.SendCheckId == id).FirstOrDefault();

            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User, _user);

            CurrentUser = GlobalUser.CurrentUser;

            memory_id = tb_check_send.tb_send.Client.ClientID;
            var ex_client = db.Clients.Where(r => r.ClientID == memory_id);

            if (ex_client == null || ex_client.Count() == 0)
            {
                var error = JsonConvert.SerializeObject(new MyExportData() { state = false, message = "حافظه مالیاتی یافت نشد" });
                return Json(error, JsonRequestBehavior.AllowGet);
            }

            _Client = ex_client.FirstOrDefault();
            init();



            ApiTax.Models.InvoiceDto _InvoiceDto = JsonConvert.DeserializeObject<ApiTax.Models.InvoiceDto>(tb_check_send.invoiceData);
            var list = new List<ApiTax.Models.InvoiceDto>();
            var random = new Random();
            long randomSerialDecimal = random.Next(999999999);
            var now = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

            var taxId = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(memory_id,
randomSerialDecimal, DateTime.Now);

            _InvoiceDto.Header.Indatim = now;
            _InvoiceDto.Header.Ins = type;
            _InvoiceDto.Header.Irtaxid = _InvoiceDto.Header.Taxid;
            _InvoiceDto.Header.Taxid = taxId;
            list.Add(_InvoiceDto);
            MyExportData MyExportData = new MyExportData() { list = list };

            MyExportData.response = send_invoice(list);
            return RedirectToAction("index", "tb_send", new { });
        }
        public void init()
        {
            //var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/FA.CER.CER"));
            func = new func();
            //  _api= func.initApi("A1211P", fileContents);
            _api = func.initApi(_Client.ClientID, _Client.PrivateKey);

        }
        public string send_invoice(List<ApiTax.Models.InvoiceDto> list_send)
        {
            try
            {
                ApiObject _ApiObject = new ApiObject() { tb_check_send = new List<tb_check_send>() };
                var json = JsonConvert.SerializeObject(list_send);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<TaxCollectData.Library.Dto.Content.InvoiceDto> invoices = JsonConvert.DeserializeObject<List<TaxCollectData.Library.Dto.Content.InvoiceDto>>(json, settings);
                var responseModel = _api.SendInvoices(invoices, null);

                if (responseModel != null && responseModel.Status == 200)
                {
                    var result = responseModel.Body.Result;
                    var json_result = JsonConvert.SerializeObject(result);

                    tb_send _tb_send = new tb_send()
                    {
                        CheckPack = "",
                        InvoicePack = json,
                        ResponcePack = json_result,
                        SendDate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString(),
                        state = 0,
                        UserID = CurrentUser.UserID,
                        ClientID = _Client.ID
                    };

                    if (GlobalUser._ObjectUser.is_api)
                    {
                        _ApiObject.tb_send = _tb_send;
                    }
                    else
                    {
                        db.tb_send.Add(_tb_send);
                        db.SaveChanges();
                    }

                    List<ResponcePack> responseData = JsonConvert.DeserializeObject<List<ResponcePack>>(json_result, settings);

                    var list_check = new List<tb_check_send>();
                    for (int x = 0; x < invoices.Count(); x++)
                    {

                        tb_check_send _tb_check_send = new tb_check_send()
                        {
                            CheckDate = "",
                            CheckResponse = "",
                            invoiceData = JsonConvert.SerializeObject(invoices[x]),
                            ReferenceNumber = responseData[x].ReferenceNumber,
                            UID = responseData[x].Uid,
                            Response = JsonConvert.SerializeObject(responseData[x]),
                            SendId = _tb_send.SendId,
                            state = 0
                        };
                        if (GlobalUser._ObjectUser.is_api)
                        {
                            _ApiObject.tb_check_send.Add(_tb_check_send);
                        }
                        else
                        {
                            db.tb_check_send.Add(_tb_check_send);
                            db.SaveChanges();
                        }

                        list_check.Add(_tb_check_send);
                    }


                    _ApiObject.check_result = func.check_send(list_check, db);
                    if (GlobalUser._ObjectUser.is_api)
                    {
                        _ApiObject.json_result = json_result;

                        json_result = JsonConvert.SerializeObject(_ApiObject);
                    }


                    return json_result;
                }
            }
            catch { }
            return "{\"error\":\"not sended\"}";
        }
        #region old upload
        //        [HttpPost]
        //        public JsonResult UploadInvoice_Old(FormCollection formCollection)
        //        {

        //            var json = formCollection["excel_data"];
        //            List<body> _body = JsonConvert.DeserializeObject<List<body>>(json);
        //            List<Header> _Header = JsonConvert.DeserializeObject<List<Header>>(json);

        //            var inno = "";

        //            List<InvoiceSigningObject> list = new List<InvoiceSigningObject>();
        //            var list_detail = new List<body>();
        //            for (int x = 0; x < _Header.Count(); x++)
        //            {

        //                if ((inno != _Header[x].inno && inno != "") || x == _Header.Count() - 1)
        //                {
        //                    if (x == _Header.Count() - 1)
        //                    {
        //                        list_detail.Add(_body[x]);
        //                    }

        //                    InvoiceSigningObject _InvoiceSigningObject = new InvoiceSigningObject();
        //                    _InvoiceSigningObject.Header = _Header[x - 1];
        //                    _InvoiceSigningObject.body = list_detail;
        //                    _InvoiceSigningObject.extension = new Extension();
        //                    //_InvoiceSigningObject.payment = new List<Payment>();
        //                    list_detail = new List<body>();
        //                    list.Add(_InvoiceSigningObject);
        //                }
        //                list_detail.Add(_body[x]);

        //                inno = _Header[x].inno;
        //            }



        //            var output = JsonConvert.SerializeObject(list);

        //            foreach (var item in list)
        //            {
        //                var ser = JsonConvert.SerializeObject(item);
        //                var normal = CryptoUtils.NormalJson(ser, null);
        //                //                var publikkey = @"-----BEGIN PUBLIC KEY-----
        //                //MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCvePUPeUekAesZcRJkJOKZqlGm
        //                //Ww/dttA3Qx8yv5lLTs+0MA87Ay21hoxpda2cpbwmXiJHgocLRJMCoR39rbr8R5dP
        //                //s81DLxPiBLld7BXc1O1nWl0dLmtO7Tx6cTs0zs8svMopLv40ic+ojOLOhvYC0wtO
        //                //UZvukpXgVrVTfVkwIQIDAQAB
        //                //-----END PUBLIC KEY-----";

        //                var publikkey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCvePUPeUekAesZcRJkJOKZqlGm
        //Ww/dttA3Qx8yv5lLTs+0MA87Ay21hoxpda2cpbwmXiJHgocLRJMCoR39rbr8R5dP
        //s81DLxPiBLld7BXc1O1nWl0dLmtO7Tx6cTs0zs8svMopLv40ic+ojOLOhvYC0wtO
        //UZvukpXgVrVTfVkwIQIDAQAB";


        //                var privatekey = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCuahNfDw3F4Oqd
        //                tcXSsNj3EcbasXhuv2gY0KfZ65bfKwV/z8VIAqegvhBER9A87CTq10CtSrTI9Jov
        //                /5/6fByvWb9t/6pA/bw/oEZDLTNwP/3Y2COdY6PU646QS7+LTMxpisfjNsecBc3n
        //                5JXRFu9o/UBeGtrW0rlcXSwSLoNLxkchBwbwRf63WBcHBl6y8EVGBYu20w8FIVv7
        //                tXen5QCvUiRnOQLXC8463oyHn8aJJxxdFyhXMiq/CLRKeZd2z/xIb5xmbk3kbD9p
        //                sJgRgSSxMFwOFc7jivko3euvqWAA3L0nR5N73HcnS7lB5Z3efA4elIlxQq26Dou9
        //                34jTNRYVAgMBAAECggEBAIw2vqf25C2mKTbsQMKmZWYKpoB9l8IAomEArU/ls35p
        //                iZw8ne7MI5J3+X/K879mYC9jKJ2npzq+WY4oxKWMTUsyrVBy7p4c3c+Qu6uZlPay
        //                mxJOgCMxTS4IyK18F9qWvOZEKXmiOkpV6Dh6bW6QL5uJrMt6b1+wPE/in2FmfyrU
        //                QawAvyuNtJnwVgGQCkGSw25KDDIdw+DB6uvQfQSyqB2blm0vabHyRHZb1uSiPu/y
        //                vPpIbJ7DGKEV85hde4YqsBa32lAeLB0qfOvvxQmG+LNmAUKdHaMTq1H0iG2N6y7e
        //                c9D1vtrrfCCkCAab9fzUBafe5qYnmK0bX0vhSixs/ukCgYEA28RCuUlB9f3vd5Yt
        //                W6gRd02iUUsuxaTWl8weeNKJs4GLp3/hZKUzbc/RDgGutLcjJCdAxkJU/J3LAhNl
        //                qykwgYiQlIyJS4aPh6KcszXD95382S3Wr6MA5mWwS/zo9qaWCwLX6I9n/fVRKQdK
        //                2QrmuGHCXXaMXD1EZJndbC1SkZsCgYEAyyufAAieXTa/C3nMNtdNSvUbAIUmzHp+
        //                HhkCF8sHLc7XlOS/k4mHzZioV/0+UddfJgt/zS/fHfVSoBR1DfVLpDgIsjxzoPeC
        //                gXZChpQFO8vAXCqWSneG4kWLlbetSvD74j1Eh9Qygzk6QuZrAw5x5qH8rPNsKpLH
        //                +V9rw+Uzig8CgYBYDFSzSW988AVOQ0Pe8gI1a0w6B8Ywd29ml+gpfiifW6qpLCoQ
        //                mcN2Honic7gcPTd+F5/zDsZgA5Q/O6hDIBiH/T/31Cp5sOq2a+ceQc9G2Oxh0uSt
        //                r7//jwRIHYb0sx9wP+5jBXmjnPKsXniVZrGzc69cpM9tcTqCl8bHvYzUOwKBgCZ/
        //                M3eMKoW7E+QWxg02Kp0jaGRm1n00UKVfU7gybj/Ny6eY2HwaOTNJ08woXiCf0JWi
        //                5Cp7AanpjChs9+kXK6gIPg2XyskbXQ0u3Vgmv/8ekmpkX2no0BQb3WEXFqz2kKPD
        //                vDKIkLGwrEt04Z4IpKhw1THoRfyjJ2UnIYJS8bsdAoGAe//Lw8vFcV4BVPvwsAVX
        //                dBS6/MyFA3Iw6mqoUOxM64rhoEJDRD9I1NYMAOPtnvA4TvQuADyFvPB5+wL15UOj
        //                yqwdlStpfO/AfyU+8y2yWfy2k5INFYSbkZVjFoP+ux6AePsCrMozMlJXcmuxn0bv
        //                LrisF6E5WzKSm4FclokJaIk=";


        //                //امضای صورتحساب
        //                string SignData = ApiClass.SignData(normal, privatekey);
        //                //تولید کلید تصادفی متقارن 
        //                Random rnd = new Random();
        //                byte[] iv = ApiClass.getRandomNonce(32);
        //                byte[] askey = ApiClass.getRandomNonce(32);

        //                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(SignData);


        //                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();


        //                Byte[] bytes_normal = new Byte[normal.Length];
        //                bytes_normal = encoding.GetBytes(normal);
        //                Byte[] bytes_xor_ed = new Byte[normal.Length];

        //                for (int x = 0; x < normal.Length; x += 32)
        //                {
        //                    int len = 32;
        //                    if (x + 32 > normal.Length)
        //                    {
        //                        len = normal.Length - x;
        //                    }
        //                    Byte[] bytes_temp = new Byte[len];
        //                    Byte[] bytes_xor_temp = new Byte[len];
        //                    for (int t = 0; t < len; t++)
        //                    {
        //                        bytes_temp[t] = bytes_normal[x + t];
        //                    }

        //                    bytes_xor_temp = ApiClass.Xor(bytes_temp, askey);

        //                    for (int t = 0; t < len; t++)
        //                    {
        //                        bytes_xor_ed[x + t] = bytes_xor_temp[t];
        //                    }

        //                }

        //                string AesEncrypt = ApiClass.AesEncrypt(bytes_xor_ed, askey, iv);
        //                // askey

        //                string EncryptData = ApiClass.EncryptData(Convert.ToBase64String(askey), publikkey);

        //                InvoiceDto invoiceDto = new InvoiceDto() {  };
        //                var invoices = new List<InvoiceDto>
        //                      {
        //                        invoiceDto
        //                       };
        //               // TaxApiService.Instance.TaxApis.SendInvoices(invoices)
        //                //رمزنگاری نامتقارن


        //            }


        //            return Json(output, JsonRequestBehavior.AllowGet);
        //        }
        #endregion
        public ActionResult SendInvoice()
        {

            return View();
        }

        MyValidation getValidation(PropertyInfo propertyInfo, int x, int? _Inty, int? _Inp, object obj, string _ClientID, string sbc)
        {

            MyValidation MyValidation = new MyValidation();
            //var ex = db.InvoiceFields.Where(r => r.Field == propertyInfo.Name);

            //if (ex.Count() == 0)
            //{

            //    MyValidation.InvoiceField = new InvoiceField();
            //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
            //    {
            //        row = -1
            //    };
            //    //MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
            //    //{
            //    //    row = x,
            //    //    title = propertyInfo.Name,
            //    //    message = "فیلد نا معتبر"
            //    //};

            //}
            //else
            {
                //var _InvoiceField = ex.FirstOrDefault();
                //MyValidation.InvoiceField = ex.FirstOrDefault();
                MyValidation.InvoiceMyValidation = new InvoiceMyValidation() { row = -1 };

                var val = propertyInfo.GetValue(obj, null);

                if (propertyInfo.PropertyType.Name == typeof(string).Name && val == "")
                {
                    val = null;
                }
                if (propertyInfo.Name == "Inp")
                {
                    int a = 0;
                }
                //if ((_Inty == 1 && _Inp == 1 && _InvoiceField.Inp11 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 1 && _Inp == 2 && _InvoiceField.Inp12 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 1 && _Inp == 3 && _InvoiceField.Inp13 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 1 && _Inp == 4 && _InvoiceField.Inp14 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 1 && _Inp == 5 && _InvoiceField.Inp15 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 1 && _Inp == 6 && _InvoiceField.Inp16 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 1 && _Inp == 7 && _InvoiceField.Inp17 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 2 && _Inp == 1 && _InvoiceField.inp21 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 2 && _Inp == 2 && _InvoiceField.inp22 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else if ((_Inty == 3 && _Inp == 1 && _InvoiceField.inp31 == 1 && val == null) || _Inty == null || _Inp == null)
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = x,
                //        title = _InvoiceField.InvoiceTitle,
                //        message = "فیلد  نباید خالی بماند"
                //    };
                //}
                //else
                //{
                //    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                //    {
                //        row = -1
                //    };
                //}

                if (memory_id != _ClientID)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = " مغایرت حافظه مالیاتی",
                        message = "شما مجاز به آپلود اطلاعات چندین حافظه مالیاتی در یک فایل نیستید"
                    };
                }

                long sbc_sbc = 0;
                try
                {
                    sbc_sbc = long.Parse(sbc);
                }
                catch
                {

                }
                var az = db.UserBranches.Where(r => r.UserID == CurrentUser.UserID && r.Branch.BranchNum == sbc_sbc && r.Branch.Client.ClientID == _ClientID);
                if (az != null)
                {
                    if (az.ToList().Count() == 0)
                    {
                        MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                        {
                            row = x,
                            title = "عدم دسترسی",
                            message = "محدودیت دسترسی برای این شعبه یا حافظه مالیاتی"
                        };
                    }
                }


            }
            return MyValidation;

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

    public class ObjectUser
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    [Serializable]
    public class ApiObject
    {
        public tb_send tb_send { get; set; }
        public List<tb_check_send> tb_check_send { get; set; }

        public string json_result { get; set; }
        public string check_result { get; set; }
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
        public string PKEY { get; set; }
    }

    public class Users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}