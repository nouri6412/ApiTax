using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTax.Models
{
    public class utility
    {
        public static string ToJson()
        {

            string json = "";



            return json;

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