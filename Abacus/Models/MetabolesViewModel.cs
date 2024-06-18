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

    public class PersonnelMetaboliViewModel
    {
        public int ΜΕΤΑΒΟΛΗ_ΚΩΔ { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Εργαζόμενος")]
        public Nullable<int> ΥΠΑΛΛΗΛΟΣ_ΚΩΔ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Ημ/νία από")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΑΠΟ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Ημ/νία έως")]
        public Nullable<System.DateTime> ΗΜΝΙΑ_ΕΩΣ { get; set; }

        [Display(Name = "Μέρες")]
        public Nullable<int> ΗΜΕΡΕΣ { get; set; }

        [Display(Name = "Σχολ.έτος")]
        public Nullable<int> ΣΧΟΛΙΚΟ_ΕΤΟΣ { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ { get; set; }

        [Display(Name = "Είδος μεταβολής")]
        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        public Nullable<int> ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ { get; set; }

        public virtual ΠΡΟΣΩΠΙΚΟ ΠΡΟΣΩΠΙΚΟ { get; set; }
    }

    public class MetabolesReportViewModel
    {
        public int RECORD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> BNS { get; set; }

        [Display(Name = "Εργαζόμενος")]
        public Nullable<int> EMPLOYEE_ID { get; set; }

        [Display(Name = "Σχολ.έτος")]
        public Nullable<int> SCHOOL_YEAR { get; set; }

        [Display(Name = "Έτος")]
        public Nullable<int> METABOLI_YEAR { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> METABOLI_MONTH { get; set; }

        [Display(Name = "Ημέρες απουσίας")]
        public Nullable<int> METABOLI_DAYS { get; set; }

        [Display(Name = "Μεταβολές")]
        public string METABOLI_TEXT { get; set; }

        public virtual ΠΡΟΣΩΠΙΚΟ ΠΡΟΣΩΠΙΚΟ { get; set; }

    }


}