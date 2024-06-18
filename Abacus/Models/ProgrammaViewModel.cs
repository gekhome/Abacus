using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Abacus.Models;
using Abacus.DAL;
using System.Web.Mvc;

namespace Abacus.Models
{
    public class ProgrammaDayViewModel
    {
        public int PROGRAMMA_ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> PROGRAMMA_DATE { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> PROGRAMMA_MONTH { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Προσέλευση")]
        public Nullable<int> HOUR_START { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Αποχώρηση")]
        public Nullable<int> HOUR_END { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Εργαζόμενος")]
        public Nullable<int> PERSON_ID { get; set; }

        [Display(Name = "Βρεφονηπιακός σταθμός")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> SCHOOLYEARID { get; set; }

        public virtual ΠΡΟΣΩΠΙΚΟ ΠΡΟΣΩΠΙΚΟ { get; set; }
    }

    public class ProgrammaParameters
    {
        public int stationId { get; set; }

        public DateTime theDate { get; set; }

        public int syearId { get; set; }
    }

    public class sqlTmimaChildViewModel
    {
        public int CHILD_ID { get; set; }

        public Nullable<int> ΤΜΗΜΑ { get; set; }

        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΑΜ { get; set; }

        [Display(Name = "Ονοματεπώνυμο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        [Display(Name = "Φύλο")]
        public string ΦΥΛΟ { get; set; }

        [Display(Name = "Ηλικία")]
        public string ΗΛΙΚΙΑ { get; set; }

        [Display(Name = "Πατρώνυμο")]
        public string ΠΑΤΡΩΝΥΜΟ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Εγγραφή")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΕΓΓΡΑΦΗ { get; set; }
    }

    public class ChildParousiaViewModel
    {
        public int PAROUSIA_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Βρεφονήπιο")]
        public Nullable<int> CHILD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> SCHOOLYEARID { get; set; }

        [Display(Name = "Τμήμα")]
        public Nullable<int> TMIMA_ID { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> PAROUSIA_MONTH { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> PAROUSIA_DATE { get; set; }

        [Display(Name = "Παρών")]
        public bool PRESENCE { get; set; }

        public virtual ΠΑΙΔΙΑ ΠΑΙΔΙΑ { get; set; }
    }

    public class ChildSelectorViewModel
    {
        public int CHILD_ID { get; set; }

        [Display(Name = "Βρεφονήπιο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }
    }

    public class ChildTmimaSelectorViewModel
    {
        public int CHILD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Βρεφονήπιο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        public Nullable<int> ΤΜΗΜΑ { get; set; }
    }

    public class ParousiesParameters
    {
        public int tmimaId { get; set; }

        public DateTime theDate { get; set; }
    }

    public class ParousiesMonthViewModel
    {
        public int ROW_ID { get; set; }
        public Nullable<int> CHILD_ID { get; set; }
        public Nullable<int> ΒΝΣ { get; set; }
        public Nullable<int> TMIMA_ID { get; set; }
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }

        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΑΜ { get; set; }

        [Display(Name = "Ονοματεπώνυμο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        [Display(Name = "Παρουσίες")]
        public Nullable<int> ΠΛΗΘΟΣ { get; set; }
        public Nullable<int> PAROUSIA_MONTH { get; set; }
        public Nullable<int> SCHOOLYEAR_ID { get; set; }
        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Display(Name = "Παρατηρήσεις")]
        public string ΠΑΡΑΤΗΡΗΣΕΙΣ { get; set; }
    }

    public class repParousiesMonthViewModel
    {
        public int CHILD_ID { get; set; }
        public Nullable<int> ΒΝΣ { get; set; }
        public Nullable<int> TMIMA_ID { get; set; }
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }
        public Nullable<int> ΑΜ { get; set; }
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }
        public Nullable<int> ΠΛΗΘΟΣ { get; set; }
        public Nullable<int> PAROUSIA_MONTH { get; set; }
        public int SCHOOLYEAR_ID { get; set; }
        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }
    }

    #region ΑΠΟΥΣΙΕΣ ΠΑΙΔΙΩΝ

    public class ApousiesDetailViewModel
    {
        public int PAROUSIA_ID { get; set; }
        public Nullable<int> CHILD_ID { get; set; }
        public Nullable<int> STATION_ID { get; set; }
        public Nullable<int> TMIMA_ID { get; set; }
        public Nullable<int> PAROUSIA_MONTH { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> PAROUSIA_DATE { get; set; }

        [Display(Name = "Βρεφονήπιο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        [Display(Name = "Τμήμα")]
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }

        public int SCHOOLYEAR_ID { get; set; }

        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }
    }

    public class ApousiesSumViewModel
    {
        public Nullable<int> CHILD_ID { get; set; }
        public Nullable<int> STATION_ID { get; set; }
        public Nullable<int> TMIMA_ID { get; set; }
        public Nullable<int> PAROUSIA_MONTH { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [Display(Name = "Απουσίες")]
        public Nullable<int> ΑPOUSIES { get; set; }

        [Display(Name = "Βρεφονήπιο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        [Display(Name = "Τμήμα")]
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }
        public int SCHOOLYEAR_ID { get; set; }
        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }
    }

    #endregion
}