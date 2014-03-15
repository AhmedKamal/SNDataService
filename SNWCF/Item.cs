//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SNWCF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public Item()
        {
            this.Preferences = new HashSet<Preference>();
        }
    
        public long ItemID { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public System.DateTime DateOfItem { get; set; }
        public string ImageUrl { get; set; }
        public long IDNewsSources { get; set; }
        public Nullable<long> ClusterID { get; set; }
        public Nullable<long> CategoryID { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual NewsSource NewsSource { get; set; }
        public virtual ICollection<Preference> Preferences { get; set; }
    }
}
