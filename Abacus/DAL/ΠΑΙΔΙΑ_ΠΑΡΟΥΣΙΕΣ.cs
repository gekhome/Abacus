//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Abacus.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ
    {
        public int PAROUSIA_ID { get; set; }
        public Nullable<int> CHILD_ID { get; set; }
        public Nullable<int> STATION_ID { get; set; }
        public Nullable<int> SCHOOLYEARID { get; set; }
        public Nullable<int> TMIMA_ID { get; set; }
        public Nullable<int> PAROUSIA_MONTH { get; set; }
        public Nullable<System.DateTime> PAROUSIA_DATE { get; set; }
        public Nullable<bool> PRESENCE { get; set; }
    
        public virtual ΠΑΙΔΙΑ ΠΑΙΔΙΑ { get; set; }
    }
}
