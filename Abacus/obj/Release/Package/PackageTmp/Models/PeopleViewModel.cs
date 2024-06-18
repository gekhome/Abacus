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
    public class ChildViewModel
    {
        public int CHILD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΑΜ { get; set; }

        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [Display(Name = "ΑΜΚΑ")]
        public string ΑΜΚΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Επώνυμο")]
        public string ΕΠΩΝΥΜΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Όνομα")]
        public string ΟΝΟΜΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Φύλο")]
        public int ΦΥΛΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία γέννησης")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΓΕΝΝΗΣΗ { get; set; }

        [Display(Name = "Ηλικία")]
        public string ΗΛΙΚΙΑ { get; set; }

        [Display(Name = "Ηλικία")]
        public decimal AGE { get; set; }

        [StringLength(10, ErrorMessage = "Πρέπει να είναι μέχρι 10 χαρακτήρες.")]
        [Display(Name = "ΑΦΜ κηδεμόνα")]
        public string ΑΦΜ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(150, ErrorMessage = "Πρέπει να είναι μέχρι 150 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Πατρώνυμο")]
        public string ΠΑΤΡΩΝΥΜΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(150, ErrorMessage = "Πρέπει να είναι μέχρι 150 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Μητρώνυμο")]
        public string ΜΗΤΡΩΝΥΜΟ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Διεύθυνση")]
        public string ΔΙΕΥΘΥΝΣΗ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Τηλέφωνα")]
        public string ΤΗΛΕΦΩΝΑ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "E-mail")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία εισόδου")]
        public Nullable<System.DateTime> ΕΙΣΟΔΟΣ_ΗΜΝΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία εξόδου")]
        public Nullable<System.DateTime> ΕΞΟΔΟΣ_ΗΜΝΙΑ { get; set; }

        [Display(Name = "Ενεργό")]
        public bool ΕΝΕΡΓΟΣ { get; set; }

        [Display(Name = "Παρατηρήσεις")]
        public string ΠΑΡΑΤΗΡΗΣΕΙΣ { get; set; }

    }

    public class ChildGridViewModel
    {
        public int CHILD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΑΜ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Επώνυμο")]
        public string ΕΠΩΝΥΜΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Όνομα")]
        public string ΟΝΟΜΑ { get; set; }

    }

    public class ChildTmimaViewModel
    {
        public int ΕΓΓΡΑΦΗ_ΚΩΔ { get; set; }

        [Display(Name = "Παιδί")]
        public Nullable<int> ΠΑΙΔΙ_ΚΩΔ { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Τμήμα")]
        public Nullable<int> ΤΜΗΜΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία εγγραφής")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΕΓΓΡΑΦΗ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία περάτωσης")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΠΕΡΑΣ { get; set; }
   
        public virtual ΠΑΙΔΙΑ ΠΑΙΔΙΑ { get; set; }
    }

    public class PersonnelViewModel
    {
        public int PERSONNEL_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΜΗΤΡΩΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "ΑΦΜ")]
        public string ΑΦΜ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Επώνυμο")]
        public string ΕΠΩΝΥΜΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Όνομα")]
        public string ΟΝΟΜΑ { get; set; }

        [StringLength(150, ErrorMessage = "Πρέπει να είναι μέχρι 150 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Πατρώνυμο")]
        public string ΠΑΤΡΩΝΥΜΟ { get; set; }

        [StringLength(150, ErrorMessage = "Πρέπει να είναι μέχρι 150 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Μητρώνυμο")]
        public string ΜΗΤΡΩΝΥΜΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Φύλο")]
        public Nullable<int> ΦΥΛΟ_ΚΩΔ { get; set; }

        [Range(1940, 2030, ErrorMessage = "Η τιμή πρέπει να είναι μεταξύ 1940 και 2030")]
        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Ετος γέννησης")]
        public Nullable<int> ΕΤΟΣ_ΓΕΝΝΗΣΗ { get; set; }

        [Display(Name = "Ηλικία")]
        public Nullable<int> ΗΛΙΚΙΑ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Διεύθυνση")]
        public string ΔΙΕΥΘΥΝΣΗ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Τηλέφωνα")]
        public string ΤΗΛΕΦΩΝΑ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "E-mail")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κλάδος")]
        public Nullable<int> ΚΛΑΔΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Ειδικότητα")]
        public Nullable<int> ΕΙΔΙΚΟΤΗΤΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Βαθμός")]
        public Nullable<int> ΒΑΘΜΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Απόφαση/ΦΕΚ τοποθέτησης")]
        public string ΑΠΟΦΑΣΗ_ΦΕΚ { get; set; }

        [Display(Name = "Παρατηρήσεις")]
        public string ΠΑΡΑΤΗΡΗΣΕΙΣ { get; set; }

        [Display(Name = "Αποχώρησε")]
        public bool ΑΠΟΧΩΡΗΣΕ { get; set; }

        [Display(Name = "Αιτία αποχώρησης")]
        public Nullable<int> ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία αποχώρησης")]
        public Nullable<System.DateTime> ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ { get; set; }

    }

    public class PersonnelGridViewModel
    {
        public int PERSONNEL_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΜΗΤΡΩΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Επώνυμο")]
        public string ΕΠΩΝΥΜΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά")]
        [Display(Name = "Όνομα")]
        public string ΟΝΟΜΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public int ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ { get; set; }
    }

    public class EducatorTmimaViewModel
    {
        public int RECORD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Display(Name = "Τμήμα")]
        public Nullable<int> ΤΜΗΜΑ_ΚΩΔ { get; set; }

        [Display(Name = "Παιδαγωγός")]
        public Nullable<int> ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ { get; set; }

        public virtual ΠΡΟΣΩΠΙΚΟ ΠΡΟΣΩΠΙΚΟ { get; set; }
    }


    #region ΜΗΤΡΩΑ ΠΑΙΔΙΩΝ, ΠΡΟΣΩΠΙΚΟΥ

    public class sqlChildInfoViewModel
    {
        public int CHILD_ID { get; set; }

        [Display(Name = "Βρεφονηπιακός σταθμός")]
        public string ΒΝΣ { get; set; }

        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΑΜ { get; set; }

        [Display(Name = "Ονοματεπώνυμο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        [Display(Name = "Φύλο")]
        public string ΦΥΛΟ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία γέννησης")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΓΕΝΝΗΣΗ { get; set; }

        [Display(Name = "Ηλικία")]
        public string ΗΛΙΚΙΑ { get; set; }

        [Display(Name = "Πατρώνυμο")]
        public string ΠΑΤΡΩΝΥΜΟ { get; set; }

        [Display(Name = "Μητρώνυμο")]
        public string ΜΗΤΡΩΝΥΜΟ { get; set; }

        [Display(Name = "Διεύθυνση")]
        public string ΔΙΕΥΘΥΝΣΗ { get; set; }

        [Display(Name = "Τηλέφωνα")]
        public string ΤΗΛΕΦΩΝΑ { get; set; }

        [Display(Name = "E-Mail")]
        public string EMAIL { get; set; }

        [Display(Name = "Περιφερειακή Δ/νση")]
        public string ΠΕΡΙΦΕΡΕΙΑΚΗ { get; set; }

    }

    public class sqlEgrafesInfoViewModel
    {
        public int ΕΓΓΡΑΦΗ_ΚΩΔ { get; set; }

        public Nullable<int> ΠΑΙΔΙ_ΚΩΔ { get; set; }

        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Τμήμα")]
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία εγγραφής")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΕΓΓΡΑΦΗ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία περάτωσης")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΠΕΡΑΣ { get; set; }
    }

    public class sqlPersonInfoViewModel
    {
        public int PERSONNEL_ID { get; set; }
        public int ΣΤΑΘΜΟΣ_ΚΩΔ { get; set; }

        [Display(Name = "ΒΝΣ")]
        public string ΒΝΣ_ΕΠΩΝΥΜΙΑ { get; set; }

        [Display(Name = "Α.Μ.")]
        public Nullable<int> ΜΗΤΡΩΟ { get; set; }

        [Display(Name = "Α.Φ.Μ.")]
        public string ΑΦΜ { get; set; }

        [Display(Name = "Ονοματεπώνυμο")]
        public string ΟΝΟΜΑΤΕΠΩΝΥΜΟ { get; set; }

        [Display(Name = "Πατρώνυμο")]
        public string ΠΑΤΡΩΝΥΜΟ { get; set; }

        [Display(Name = "Μητρώνυμο")]
        public string ΜΗΤΡΩΝΥΜΟ { get; set; }

        [Display(Name = "Φύλο")]
        public string ΦΥΛΟ { get; set; }

        [Display(Name = "Έτος γέννησης")]
        public Nullable<int> ΕΤΟΣ_ΓΕΝΝΗΣΗ { get; set; }

        [Display(Name = "Ηλικία")]
        public Nullable<int> ΗΛΙΚΙΑ { get; set; }

        [Display(Name = "Διεύθυνση")]
        public string ΔΙΕΥΘΥΝΣΗ { get; set; }

        [Display(Name = "Τηλέφωνα")]
        public string ΤΗΛΕΦΩΝΑ { get; set; }

        [Display(Name = "E-Mail")]
        public string EMAIL { get; set; }

        [Display(Name = "Κλάδος")]
        public string EIDIKOTITA_CODE { get; set; }

        [Display(Name = "Ειδικότητα")]
        public string EIDIKOTITA_TEXT { get; set; }

        [Display(Name = "Κατηγορία")]
        public string PROSOPIKO_TEXT { get; set; }

        [Display(Name = "Περιφερειακή")]
        public string ΠΕΡΙΦΕΡΕΙΑΚΗ { get; set; }

    }

    public partial class sqlEducatorTmimaInfoViewModel
    {
        public int RECORD_ID { get; set; }
        public Nullable<int> ΒΝΣ { get; set; }
        public Nullable<int> ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public string ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Display(Name = "Τμήμα")]
        public string ΤΜΗΜΑ_ΟΝΟΜΑ { get; set; }
    }

    #endregion

}