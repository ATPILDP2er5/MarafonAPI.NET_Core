//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiMarafons.Table
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class SPORTMENS
    {
        [Key]
        public int UID { get; set; }
        public string fam { get; set; }
        public string name { get; set; }
        public string otch { get; set; }
        public Nullable<bool> pol { get; set; }
        public Nullable<System.DateTime> bday { get; set; }
        public string strana { get; set; }
    }
}
