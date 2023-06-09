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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.tb_marketing = new HashSet<tb_marketing>();
            this.tb_send = new HashSet<tb_send>();
            this.UserBranches = new HashSet<UserBranch>();
        }
    
        public int UserID { get; set; }
        public string NationalCode { get; set; }
        public string Fname { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string StartActive { get; set; }
        public string EndActive { get; set; }
        public Nullable<bool> isAdmin { get; set; }
        public Nullable<int> user_type { get; set; }
        public Nullable<int> BankID { get; set; }
        public string Shaba { get; set; }
    
        public virtual tb_bank tb_bank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_marketing> tb_marketing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_send> tb_send { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBranch> UserBranches { get; set; }
    }
}
