using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ApiTax.Models
{
    public static class utility
    {
        public static string ToJson()
        {

            string json = "";



            return json;

        }

        public static string ToPersian(string date)
        {
            try
            {
                DateTime d = DateTime.Parse(date);
                PersianCalendar pc = new PersianCalendar();
                return string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
            }
            catch {
                return "";
            }


        }

        public static DateTime ToMiladi(string date)
        {
            try
            {
                return DateTime.Parse(date, new System.Globalization.CultureInfo("fa-IR"));

            }
            catch
            {
                return DateTime.Now;
            }
        }
    }

    class ToJson<T>
    {
        private T genericMemberVariable;

        public ToJson(T value)
        {
            genericMemberVariable = value;
        }

        public  string get(T genericParameter)
        {

            string json = JsonConvert.SerializeObject(genericParameter);


            return json;

        }

        public T genericProperty { get; set; }
    }

    public class DataKeyVal
    {
        public string key { get; set; }

        public string value { get; set; }

    }

    public class ListData
    {
        public Boolean  IsArray { get; set; }
        public List<DataKeyVal> DataKeyValList { get; set; }

    }

}