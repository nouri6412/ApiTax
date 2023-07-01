using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using System.Web.Routing;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;

namespace ApiTax.Models
{
    public static class GlobalUser
    {
        public static User CurrentUser { get; set; }
        public static Boolean isAdmin { get; set; }
        public static Boolean isLogin { get; set; }
        public static List<UserBranch> UserBranches { get; set; }
        public static int? user_type { get; set; }
        public static ObjectUser _ObjectUser { get; set; } 
    }

    public class InitRequest
    {
        public void init(System.Security.Principal.IPrincipal User,string _userjson=null)
        {
            try
            {
                ObjectUser _user=new ObjectUser() {  is_api=false};
                var username = "";
                if (_userjson != null)
                {

                     _user = JsonConvert.DeserializeObject<ObjectUser>(_userjson);
                    
                    func func = new func();
                    if (func.IsValid(_user.username, _user.password))
                    {
                        username = _user.username;
                        _user.is_api = true;
                    }
                }
                if (User.Identity.IsAuthenticated || username !="")
                {
                    StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
                 

                    if(User.Identity.IsAuthenticated)
                    {
                        username = User.Identity.Name;
                    }
                    
                    var CurrentUser = db.Users.Where(r => r.NationalCode == username).FirstOrDefault();

                 
                    GlobalUser.isLogin = true;
                    GlobalUser.UserBranches = CurrentUser.UserBranches.ToList();

                    GlobalUser.user_type = CurrentUser.user_type;
                    GlobalUser._ObjectUser = _user;

                    if (CurrentUser.isAdmin == true)
                    {
                        GlobalUser.isAdmin = true;
                    }
                    else
                    {
                        GlobalUser.isAdmin = false;
                    }
                    GlobalUser.CurrentUser = CurrentUser;
                }
                else
                {
                    GlobalUser.user_type = 0;
                    GlobalUser.isLogin = false;
                    GlobalUser.isAdmin = false;
                    GlobalUser.CurrentUser = new Models.User();
                    GlobalUser.UserBranches = new List<UserBranch>();
                    
                }

            }
            catch { }
        }
    }
    public class func
    {
      public  ITaxApis _api;
        public ITaxApis initApi(string memory_id, string key)
        {

            TaxApiService.Instance.Init(memory_id, new SignatoryConfig(key, null), new NormalProperties(ClientType.SELF_TSP), "https://tp.tax.gov.ir/req/api/self-tsp");


            var info = TaxApiService.Instance.TaxApis.GetServerInformation();
            var token = TaxApiService.Instance.TaxApis.RequestToken();

             _api = TaxApiService.Instance.TaxApis;
            return _api;
        }
        public bool IsValid(string email, string password)
        {
            StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
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
        public string check_send(List<tb_check_send> list_send, StoreTerminalSystemEntities db)
        {
            string json_result = "";

            var list_check = new List<UidAndFiscalId>();

            foreach (var item in list_send)
            {
                if(item.tb_send != null)
                {
                    MyUidAndFiscalId myuidAndFiscalId = new MyUidAndFiscalId()
                    {
                        Uid = item.UID,
                        FiscalId = item.tb_send.Client.ClientID
                    };
                    var json = JsonConvert.SerializeObject(myuidAndFiscalId);
                    UidAndFiscalId uidAndFiscalId = JsonConvert.DeserializeObject<UidAndFiscalId>(json);

                    list_check.Add(uidAndFiscalId);
                }
          
            }

            var inquiryResultModels =
_api.InquiryByUidAndFiscalId(list_check);

            if(GlobalUser._ObjectUser.is_api)
            {
                json_result = JsonConvert.SerializeObject(inquiryResultModels);
            }
            else
            {
                foreach (var it in inquiryResultModels)
                {
                    var items = list_send.Where(r => r.UID == it.Uid);
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

            return json_result;
        }

    }
    public class MyUidAndFiscalId
    {
        public string Uid { get; set; }
        public string FiscalId { get; set; }
    }
    public class ObjectUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string clientID { get; set; }
        public string key { get; set; }
        public  Boolean is_api { get; set; }
    }
}