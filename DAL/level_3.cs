//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eyemusic45.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class level_3
    {
        public int user_ID { get; set; }
        public Nullable<int> level_unity { get; set; }
        public Nullable<int> frame { get; set; }
        public Nullable<int> x { get; set; }
        public Nullable<int> y { get; set; }
        public Nullable<int> distance { get; set; }
        public Nullable<int> angle { get; set; }
        public Nullable<int> success { get; set; }
        public Nullable<int> time_out { get; set; }
        public System.DateTime datetime { get; set; }
    
        public virtual userID userID { get; set; }
    }
}
