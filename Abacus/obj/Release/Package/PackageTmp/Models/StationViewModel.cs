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
    public class StationsViewModel
    {
        public int ΣΤΑΘΜΟΣ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Επωνυμία")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Ταχ. Διεύθυνση")]
        public string ΤΑΧ_ΔΙΕΥΘΥΝΣΗ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Τηλ. Υπεύθυνου")]
        public string ΤΗΛΕΦΩΝΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Τηλ. Γραμματείας")]
        public string ΓΡΑΜΜΑΤΕΙΑ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Φαξ")]
        public string ΦΑΞ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "E-Mail")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Υπεύθυνος")]
        public string ΥΠΕΥΘΥΝΟΣ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Φύλο Υπεύθυνου")]
        public Nullable<int> ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Κινητό Υπεύθυνου")]
        public string ΚΙΝΗΤΟ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Αναπληρωτής")]
        public string ΑΝΑΠΛΗΡΩΤΗΣ { get; set; }

        [Display(Name = "Φύλο Αναπληρωτή")]
        public Nullable<int> ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Διαχειριστής")]
        public string ΔΙΑΧΕΙΡΙΣΤΗΣ { get; set; }

        [Display(Name = "Φύλο Διαχειριστή")]
        public Nullable<int> ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Περιφερειακή Δ/νση")]
        public Nullable<int> ΠΕΡΙΦΕΡΕΙΑΚΗ { get; set; }

        [Display(Name = "Περιφερειακή Ενότητα")]
        public Nullable<int> ΠΕΡΙΦΕΡΕΙΑ { get; set; }

        public virtual ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ { get; set; }

    }

    public class StationsGridViewModel
    {
        public int ΣΤΑΘΜΟΣ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Επωνυμία")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Περιφερειακή Δ/νση")]
        public Nullable<int> ΠΕΡΙΦΕΡΕΙΑΚΗ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Υπεύθυνος")]
        public string ΥΠΕΥΘΥΝΟΣ { get; set; }
    }

    public class sqlStationSelectorViewModel
    {
        public int ΣΤΑΘΜΟΣ_ΚΩΔ { get; set; }

        [Display(Name = "Επωνυμία")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [Display(Name = "Περιφερειακή Δ/νση")]
        public string ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ { get; set; }

        [Display(Name = "Υπεύθυνος")]
        public string ΥΠΕΥΘΥΝΟΣ { get; set; }

        public Nullable<int> ΠΕΡΙΦΕΡΕΙΑΚΗ { get; set; }
    }

}