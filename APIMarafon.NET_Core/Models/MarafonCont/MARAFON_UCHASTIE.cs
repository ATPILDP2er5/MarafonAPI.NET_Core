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

    public partial class MARAFON_UCHASTIE
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> MUID { get; set; }
        public Nullable<int> SUID { get; set; }
    }
}
