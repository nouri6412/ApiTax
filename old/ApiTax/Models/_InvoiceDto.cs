using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTax.Models
{

    public class InvoiceDto
    {
        public List<ApiTax.Models.InvoiceBodyDto> Body { get; set; }
        public ApiTax.Models.InvoiceHeaderDto Header { get; set; }
        public List<ApiTax.Models.PaymentDto> Payments { get; set; }
        public List<ApiTax.Models.InvoiceExtension> Extension { get; set; }

    }
    public class InvoiceBodyDto
    {

        public string Sstt { get; set; }
        public int? Mu { get; set; }
        public double? Am { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Cfee { get; set; }
        public string Cut { get; set; }
        public string Exr { get; set; }
        public decimal? Prdis { get; set; }
        public decimal? Dis { get; set; }
        public decimal? Adis { get; set; }
        public decimal? Vra { get; set; }
        public string Sstid { get; set; }
        public decimal? Vam { get; set; }
        public decimal? Odr { get; set; }
        public decimal? Odam { get; set; }
        public string Olt { get; set; }
        public decimal? Olr { get; set; }
        public decimal? Olam { get; set; }
        public decimal? Consfee { get; set; }
        public decimal? Spro { get; set; }
        public decimal? Bros { get; set; }
        public decimal? Tcpbs { get; set; }
        public decimal? Cop { get; set; }
        public string Bsrn { get; set; }
        public string Odt { get; set; }
        public string Vop { get; set; }
        public decimal? Tsstam { get; set; }

    }

    public class InvoiceHeaderDto 
    {

        public decimal Tocv { get; set; }
        public string Bpc { get; set; }
        public string Sbc { get; set; }
        public string Tinb { get; set; }
        public string Bid { get; set; }
        public int Tob { get; set; }
        public string Tins { get; set; }
        public int Ins { get; set; }
        public int Inp { get; set; }
        public string Irtaxid { get; set; }
        public string Inno { get; set; }
        public int Inty { get; set; }
        public long Indati2m { get; set; }
        public long Indatim { get; set; }
        public string Taxid { get; set; }
        public string Bbc { get; set; }
        public string Bpn { get; set; }
        public int Ft { get; set; }
        public int Setm { get; set; }
        public decimal Tonw { get; set; }
        public int Cdcd { get; set; }
        public string Cdcn { get; set; }
        public decimal Tax17 { get; set; }
        public decimal Tvop { get; set; }
        public decimal Insp { get; set; }
        public decimal Cap { get; set; }
        public string Scln { get; set; }
        public decimal Torv { get; set; }
        public decimal Todam { get; set; }
        public decimal Tvam { get; set; }
        public decimal Tadis { get; set; }
        public decimal Tdis { get; set; }
        public decimal Tprdis { get; set; }
        public string Billid { get; set; }
        public string Crn { get; set; }
        public string Scc { get; set; }
        public decimal Tbill { get; set; }
    }

    public class PaymentDto 
    {

        public long? Pdt { get; set; }
        public string Pcn { get; set; }
        public string Trn { get; set; }
        public string Trmn { get; set; }
        public string Acn { get; set; }
        public string Iinn { get; set; }
        public string Pid { get; set; }
    }

    public class InvoiceExtension 
    {
  
    }

    [Serializable]
    public class InvoiceMyValidation
    {
        public int row { get; set; }
        public string title { get; set; }
        public string message { get; set; }
    }
}