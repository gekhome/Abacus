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
    public class SysGenderViewModel
    {
        public int ΦΥΛΟ_ΚΩΔ { get; set; }
        public string ΦΥΛΟ { get; set; }

    }

    public class SysMonthViewModel
    {
        public int ΜΗΝΑΣ_ΚΩΔ { get; set; }
        public string ΜΗΝΑΣ { get; set; }
    }

    public class SysYearViewModel
    {
        public int ΕΤΟΣ_ΚΩΔ { get; set; }
        public Nullable<int> ΕΤΟΣ { get; set; }
    }

    public class HoursViewModel
    {
        public int HOUR_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(5, ErrorMessage = "Πρέπει να είναι μέχρι 5 χαρακτήρες (π.χ.13:45)")]
        [Display(Name = "Ωρα προσέλευσης/αποχώρησης")]
        public string HOUR_TEXT { get; set; }

    }

    public class BathmosViewModel
    {
        public int ΒΑΘΜΟΣ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Βαθμός")]
        public string ΒΑΘΜΟΣ_ΛΕΚΤΙΚΟ { get; set; }
    }

    public class PersonnelTypeViewModel
    {
        public int PROSOPIKO_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-Ω]+[ Α-Ω-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά.")]
        [Display(Name = "Κατηγορία προσωπικού")]
        public string PROSOPIKO_TEXT { get; set; }
    }

    public class SysApoxorisiViewModel
    {
        public int ΑΠΟΧΩΡΗΣΗ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-Ω]+[ Α-Ω-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά.")]
        [Display(Name = "Αιτία αποχώρησης")]
        public string ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ { get; set; }
    }

    public class KladosViewModel
    {
        public int ΚΛΑΔΟΣ_ΚΩΔ { get; set; }

        [StringLength(10, ErrorMessage = "Πρέπει να είναι μέχρι 10 χαρακτήρες.")]
        [Display(Name = "Κλάδος")]
        public string ΚΛΑΔΟΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Βασικός μισθός")]
        public Nullable<decimal> ΜΙΣΘΟΣ { get; set; }
    }

    public class EidikotitesViewModel
    {
        public int EIDIKOTITA_ID { get; set; }

        [StringLength(10, ErrorMessage = "Πρέπει να είναι μέχρι 10 χαρακτήρες.")]
        [RegularExpression(@"^[Α-Ω]+[ Α-Ω-_ΪΫ0-9]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά και αριθμοί.")]
        [Display(Name = "Κωδικός")]
        public string EIDIKOTITA_CODE { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [RegularExpression(@"^[Α-Ω]+[ Α-Ω-_ΪΫ0-9]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά και αριθμοί.")]
        [Display(Name = "Κλαδος-Ειδικότητα")]
        public string EIDIKOTITA_TEXT { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κλαδος")]
        public int KLADOS { get; set; }

        public virtual ΣΥΣ_ΚΛΑΔΟΙ ΣΥΣ_ΚΛΑΔΟΙ { get; set; }
    }

    public class TmimaCategoryViewModel
    {
        public int CATEGORY_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-Ω]+[ Α-Ω-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά.")]
        [Display(Name = "Κατηγορία τμήματος")]
        public string CATEGORY_TEXT { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Από ετών")]
        public Nullable<decimal> AGE_START { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Έως ετών")]
        public Nullable<decimal> AGE_END { get; set; }

    }

    public class TmimaViewModel
    {
        public int ΤΜΗΜΑ_ΚΩΔ { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Σχολικό έτος")]
        public Nullable<int> ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Α/Α")]
        public Nullable<int> ΑΥΞΩΝ_ΑΡΙΘΜΟΣ { get; set; }

        [StringLength(10, ErrorMessage = "Πρέπει να είναι μέχρι 10 χαρακτήρες.")]
        [RegularExpression(@"^[Α-Ω]+[ Α-Ω-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, κενά και παύλες.")]
        [Display(Name = "Χαρακτηρισμός")]
        public string ΧΑΡΑΚΤΗΡΙΣΜΟΣ { get; set; }

        [Display(Name = "Ονομασία τμήματος")]
        public string ΟΝΟΜΑΣΙΑ { get; set; }

        public virtual ΣΥΣ_ΣΤΑΘΜΟΙ ΣΥΣ_ΣΤΑΘΜΟΙ { get; set; }
        public virtual ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ { get; set; }
    }

    public class TmimaSelectorViewModel
    {
        public int ΤΜΗΜΑ_ΚΩΔ { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολικό έτος")]
        public Nullable<int> ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Display(Name = "Ονομασία τμήματος")]
        public string ΟΝΟΜΑΣΙΑ { get; set; }
    }

    public class sqlTmimaInfoViewModel
    {
        public int ΤΜΗΜΑ_ΚΩΔ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public int SCHOOLYEAR_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Display(Name = "Τμήμα")]
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }

        [Display(Name = "Βρεφονηπιακός σταθμός")]
        public string ΒΝΣ_ΟΝΟΜΑ { get; set; }

        [Display(Name = "Περιφερειακή")]
        public string ΠΕΡΙΦΕΡΕΙΑΚΗ { get; set; }
    }

    public class SysSchoolYearViewModel
    {
        public int SCHOOLYEAR_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(9, ErrorMessage = "Πρέπει να είναι μέχρι 9 χαρακτήρες (π.χ.2015-2016).")]
        [Display(Name = "Σχολικό Έτος")]
        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία Έναρξης")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΕΝΑΡΞΗ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία Λήξης")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΛΗΞΗ { get; set; }

        [Display(Name = "Τρέχον")]
        public bool ΤΡΕΧΟΝ_ΕΤΟΣ { get; set; }

    }

    public class SysPeriferiakiViewModel
    {
        public int ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Περιφερειακή επωνυμία")]
        public string ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Ταχ. Διεύθυνση")]
        public string ΤΑΧ_ΔΙΕΥΘΥΝΣΗ { get; set; }


        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Πόλη έδρα")]
        public string ΕΔΡΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Τμήμα")]
        public string ΤΜΗΜΑ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Τηλέφωνα")]
        public string ΤΗΛΕΦΩΝΑ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Φαξ")]
        public string FAX { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "E-Mail")]
        public string EMAIL { get; set; }

    }

    public class SysEgrafoTypeViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }
        public string ΕΓΓΡΑΦΟ_ΕΙΔΟΣ { get; set; }

    }

    public class SysReportViewModel
    {
        public int DOC_ID { get; set; }

        [Display(Name = "Ονομασία")]
        public string DOC_NAME { get; set; }

        [Display(Name = "Περιγραφή")]
        public string DOC_DESCRIPTION { get; set; }

        [Display(Name = "Κατηγορία")]
        public string DOC_CLASS { get; set; }

    }

    public class SysMetabolesViewModel
    {
        public int METABOLI_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Είδος μεταβολής")]
        public string METABOLI_TEXT { get; set; }
    }


    #region TOOLS

    //----------------------------------------------------
    // new addition 16-07-2019 for MasterChild grids
    //----------------------------------------------------
    public class PeriferiaViewModel
    {
        public int PERIFERIA_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [Display(Name = "Περιφερειακή Ενότητα")]
        public string PERIFERIA_NAME { get; set; }
    }

    public class DimosViewModel
    {
        public int DIMOS_ID { get; set; }

        [Display(Name = "Δήμος")]
        public string DIMOS { get; set; }
        public Nullable<int> DIMOS_PERIFERIA { get; set; }
    }

    public class DimoiParameters
    {
        public int PERIFERIA_ID { get; set; }
    }

    #endregion

    public class StationLoginsViewModel
    {
        public int LOGIN_ID { get; set; }

        [Display(Name = "Βρεφονηπιακός σταθμός")]
        public string STATION_NAME { get; set; }

        [Display(Name = "Τελευταία είσοδος")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public Nullable<System.DateTime> LOGIN_DATETIME { get; set; }

    }

    public class AdminLoginsViewModel
    {
        public int LOGIN_ID { get; set; }

        [Display(Name = "Διαχειριστής")]
        public string ADMIN_NAME { get; set; }

        [Display(Name = "Τελευταία είσοδος")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public Nullable<System.DateTime> LOGIN_DATETIME { get; set; }
    }

    public class AgeYearMonth
    {
        public int Years { get; set; }

        public int Months { get; set; }
    }

    public class GeneralReportParameters
    {
        public int? PERIFERIA_ID { get; set; }

        public int? PERIFERIAKI_ID { get; set; }

        public int? STATION_ID { get; set; }

        public int? SYEAR_ID { get; set; }
    }

}

