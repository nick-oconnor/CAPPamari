//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAPPamari.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            this.Advisors = new HashSet<Advisor>();
        }
    
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Major { get; set; }
    
        public virtual ICollection<Advisor> Advisors { get; set; }
    }
}
