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
    
    public partial class tb_contact
    {
        public long ContactID { get; set; }
        public string NationalCode { get; set; }
        public string Fullname { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public Nullable<int> degreeID { get; set; }
        public Nullable<int> magorID { get; set; }
    
        public virtual ContactInPerson ContactInPerson { get; set; }
        public virtual tb_degree tb_degree { get; set; }
        public virtual tb_magor tb_magor { get; set; }
    }
}