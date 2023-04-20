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

namespace ApiTax.Models
{
    public static class GlobalUser
    {
        public static User CurrentUser { get; set; }
        public static Boolean isAdmin { get; set; }
        public static Boolean isLogin { get; set; }
        public static List<UserBranch> UserBranches { get; set; }
        public static int? user_type { get; set; }
    }

    public class InitRequest
    {
        public void init(System.Security.Principal.IPrincipal User)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
                    var username = User.Identity.Name;
                    var CurrentUser = db.Users.Where(r => r.NationalCode == username).FirstOrDefault();
                    GlobalUser.isLogin = true;
                    GlobalUser.UserBranches = CurrentUser.UserBranches.ToList();

                    GlobalUser.user_type = CurrentUser.user_type;

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

    }
    public class MyUidAndFiscalId
    {
        public string Uid { get; set; }
        public string FiscalId { get; set; }
    }
}