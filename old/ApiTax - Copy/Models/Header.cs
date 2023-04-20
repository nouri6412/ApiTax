using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace ApiTax.Models
{
    public class Header
    {
        [Required]
        [StringLength(22)]
        public string taxid { get; set; }
        public string indatim { get; set; }
        public string indati2m { get; set; }
        public string inty { get; set; }
 
        public string inno { get; set; }
        public string iryaxid { get; set; }
        public string inp { get; set; }

        public string ins { get; set; }
        public string tins { get; set; }
        public string tob { get; set; }
        public string bid { get; set; }
        public string tinb { get; set; }
        public string sbc { get; set; }
        public string pbc { get; set; }
        public string bbc { get; set; }
        public string ft { get; set; }
        public string bpn { get; set; }
        public string scln { get; set; }
        public string scc { get; set; }
        public string srn { get; set; }
        public string billid { get; set; }
        public string tprdis { get; set; }
        public string tdis { get; set; }
        public string tvam { get; set; }
        public string todam { get; set; }
        public string tbill { get; set; }
        public string setm { get; set; }
        public string cap { get; set; }
        public string inap { get; set; }
        public string tvop { get; set; }
        public string tax17 { get; set; }


    }
}