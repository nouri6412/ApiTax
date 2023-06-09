﻿using System;
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

namespace ApiTax.Controllers
{
    public class InvoiceApiController : Controller
    {
        private StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
        public User CurrentUser;
        // GET: InvoiceApi
        public ActionResult Index()
        {
            var x = TaxApiService.Instance.TaxApis.GetEconomicCodeInformation("10200382627");

            return View();
        }
        public ActionResult UploadInvoice()
        {

            return View();
        }

        [HttpPost]
        public JsonResult UploadInvoice(FormCollection formCollection)
        {
            var username = User.Identity.Name;
            CurrentUser = db.Users.Where(r=>r.NationalCode== username).FirstOrDefault();

            var json = formCollection["excel_data"];
            List<ApiTax.Models.InvoiceBodyDto> _body = JsonConvert.DeserializeObject<List<ApiTax.Models.InvoiceBodyDto>>(json);
            List< ApiTax.Models.InvoiceHeaderDto> _Header = JsonConvert.DeserializeObject<List<ApiTax.Models.InvoiceHeaderDto>>(json);
            List<ApiTax.Models.PaymentDto> _Payments = JsonConvert.DeserializeObject<List<ApiTax.Models.PaymentDto>>(json);
            List<ExtraJsonData> _ExtraJsonData = JsonConvert.DeserializeObject<List<ExtraJsonData>>(json);


            
            long? inno = 0;

            List<InvoiceMyValidation> _InvoiceMyValidation = new List<InvoiceMyValidation>();
            List<ApiTax.Models.InvoiceDto> list = new List<ApiTax.Models.InvoiceDto>();
            var list_detail = new List<ApiTax.Models.InvoiceBodyDto>();
            for (int x = 0; x < _Header.Count(); x++)
            {

                if ((inno != _Header[x].Inno && inno != 0) || x == _Header.Count() - 1)
                {
                    if (x == _Header.Count() - 1)
                    {
                        list_detail.Add(_body[x]);
                    }

                    ApiTax.Models.InvoiceDto _InvoiceDto = new ApiTax.Models.InvoiceDto() { };
                    if (x > 0)
                    {
                        _InvoiceDto.Header = _Header[x - 1];
                    }
                    else
                    {
                        _InvoiceDto.Header = _Header[x];
                    }
                   
                    _InvoiceDto.Body = list_detail;
                   // _InvoiceDto.extension = new Extension();
                   _InvoiceDto.Payments = _Payments;
                    list_detail = new List<ApiTax.Models.InvoiceBodyDto>();
                    list.Add(_InvoiceDto);
                }
                list_detail.Add(_body[x]);

                inno = _Header[x].Inno;

                int? type_1 = _Header[x].Inty;
                int? type_2 = _Header[x].Inp;
                int? _sbc= _Header[x].Sbc;

                string ClientID = _ExtraJsonData[x].ClientID;

                foreach (PropertyInfo propertyInfo in _Header[x].GetType().GetProperties())
                {
                    MyValidation MyValidation= getValidation(propertyInfo,x, type_1, type_2, _Header[x], ClientID, _sbc);
                    if(MyValidation.InvoiceMyValidation.row > -1)
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

            MyExportData MyExportData = new MyExportData() {  list= list , list_error= _InvoiceMyValidation };

            var output = JsonConvert.SerializeObject(MyExportData);



            return Json(output, JsonRequestBehavior.AllowGet);
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

        MyValidation getValidation(PropertyInfo propertyInfo, int x,int? _Inty,int? _Inp,object obj,string _ClientID,int? sbc)
        {
          
            MyValidation MyValidation = new MyValidation();
            var ex = db.InvoiceFields.Where(r => r.Field == propertyInfo.Name);

            if(ex.Count()==0)
            {

                MyValidation.InvoiceField = new InvoiceField();
                MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                {
                    row = x,
                    title = propertyInfo.Name,
                    message = "فیلد نا معتبر"
                };

            }
            else
                {
                var _InvoiceField = ex.FirstOrDefault();
                MyValidation.InvoiceField = ex.FirstOrDefault(); 
                MyValidation.InvoiceMyValidation = new InvoiceMyValidation() {  row=-1};

                var val = propertyInfo.GetValue(obj, null);

                if (propertyInfo.PropertyType.Name == typeof(string).Name && val=="")
                {
                    val = null;
                }
                if(propertyInfo.Name=="Inp")
                {
                    int a = 0;
                }
                if ((_Inty==1 && _Inp==1 && _InvoiceField.Inp11==1 && val == null) || _Inty ==null || _Inp==null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 1 && _Inp == 2 && _InvoiceField.Inp12 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 1 && _Inp == 3 && _InvoiceField.Inp13 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 1 && _Inp == 4 && _InvoiceField.Inp14 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 1 && _Inp == 5 && _InvoiceField.Inp15 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 1 && _Inp == 6 && _InvoiceField.Inp16 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 1 && _Inp == 7 && _InvoiceField.Inp17 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 2 && _Inp == 1 && _InvoiceField.inp21 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 2 && _Inp == 2 && _InvoiceField.inp22 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else if ((_Inty == 3 && _Inp == 1 && _InvoiceField.inp31 == 1 && val == null) || _Inty == null || _Inp == null)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = _InvoiceField.InvoiceTitle,
                        message = "فیلد  نباید خالی بماند"
                    };
                }
                else
                    {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = -1
                    };
                }


                var az = db.UserBranches.Where(r=>r.UserID== CurrentUser.UserID && r.Branch.BranchNum== sbc && r.Branch.Client.ClientID==_ClientID);
                if(az.Count()==0)
                {
                    MyValidation.InvoiceMyValidation = new InvoiceMyValidation()
                    {
                        row = x,
                        title = "عدم دسترسی",
                        message = "محدودیت دسترسی برای این شعبه یا حافظه مالیاتی"
                    };
                }
             

            }
            return MyValidation;

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
}