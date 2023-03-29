//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiTax.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceHeader()
        {
            this.InvoiceBodies = new HashSet<InvoiceBody>();
            this.Payments = new HashSet<Payment>();
        }
    
        public long InvoiceID { get; set; }
        public string Taxid { get; set; }
        public long Inno { get; set; }
        public string Irtaxid { get; set; }
        public long Indatim { get; set; }
        public Nullable<long> Indati2m { get; set; }
        public int Inp { get; set; }
        public string Tins { get; set; }
        public int Ins { get; set; }
        public string Sbc { get; set; }
        public Nullable<int> Crn { get; set; }
        public Nullable<long> Scln { get; set; }
        public Nullable<int> Scc { get; set; }
        public Nullable<int> spc { get; set; }
        public string Tinb { get; set; }
        public string Bid { get; set; }
        public string Bbc { get; set; }
        public string Billid { get; set; }
        public string Bpc { get; set; }
        public Nullable<int> ft { get; set; }
        public string Bpn { get; set; }
        public int Setm { get; set; }
        public Nullable<decimal> Cap { get; set; }
        public Nullable<decimal> Insp { get; set; }
        public Nullable<decimal> tax17 { get; set; }
        public Nullable<decimal> Tvam { get; set; }
        public Nullable<decimal> Tprdis { get; set; }
        public Nullable<decimal> Tdis { get; set; }
        public Nullable<decimal> Tadis { get; set; }
        public Nullable<decimal> Todam { get; set; }
        public Nullable<decimal> Tbill { get; set; }
        public Nullable<decimal> tcpbs { get; set; }
        public Nullable<decimal> tonw { get; set; }
        public Nullable<decimal> torv { get; set; }
        public Nullable<decimal> tocv { get; set; }
        public Nullable<int> Tob { get; set; }
        public string uuid { get; set; }
        public Nullable<int> Inty { get; set; }
        public Nullable<decimal> Tvop { get; set; }
        public Nullable<int> Dpvb { get; set; }
        public Nullable<int> InpID { get; set; }
    
        public virtual ftType ftType { get; set; }
        public virtual InPatern InPatern { get; set; }
        public virtual InsType InsType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceBody> InvoiceBodies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
