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
    
    public partial class sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ
    {
        public int ROWID { get; set; }
        public Nullable<int> STATION_ID { get; set; }
        public string ΕΠΩΝΥΜΙΑ { get; set; }
        public Nullable<int> SCHOOLYEARID { get; set; }
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }
        public Nullable<int> ΠΑΙΔΙΑ { get; set; }
        public Nullable<int> ΠΡΟΣΩΠΙΚΟ { get; set; }
        public Nullable<int> ΑΤΟΜΑ { get; set; }
        public Nullable<decimal> ΚΟΣΤΟΣ_ΗΜΕΡΑ { get; set; }
        public Nullable<decimal> ΔΑΠΑΝΗ_ΗΜΕΡΑ { get; set; }
        public Nullable<decimal> ΥΠΟΛΟΙΠΟ { get; set; }
        public Nullable<int> ΜΗΝΑΣ { get; set; }
    }
}
