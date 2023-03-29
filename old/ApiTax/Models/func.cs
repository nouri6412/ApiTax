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

namespace ApiTax.Models
{
    public class func
    {
      public  ITaxApis _api;
        public ITaxApis initApi(string memory_id, string key)
        {
            TaxApiService.Instance.Init(memory_id, new SignatoryConfig(key, null), "https://tp.tax.gov.ir/req/api/self-tsp");
            var info = TaxApiService.Instance.TaxApis.GetServerInformation();
            var token = TaxApiService.Instance.TaxApis.RequestToken();

             _api = TaxApiService.Instance.TaxApis;
            return _api;
        }

        public void check_send(List<tb_check_send> list_send, StoreTerminalSystemEntities db)
        {


            var list_check = new List<UidAndFiscalId>();

            foreach (var item in list_send)
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

            var inquiryResultModels =
_api.InquiryByUidAndFiscalId(list_check);
            foreach (var it in inquiryResultModels)
            {
                var items = list_send.Where(r=>r.UID==it.Uid);
                if(items !=null && items.Count()>0)
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

    }
    public class MyUidAndFiscalId
    {
        public string Uid { get; set; }
        public string FiscalId { get; set; }
    }
}