//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleNote.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Trash
    {
        public int ID { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public string tags { get; set; }
        public Nullable<bool> isPinned { get; set; }
    }
}
