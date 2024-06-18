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
    public class DocOperatorViewModel
    {
        public int DOCADMIN_ID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> DOCSTATION_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Ονοματεπώνυμο (πεζά)")]
        public string DOCADMIN_NAME { get; set; }
    }

    public class DocMetabolesViewModel
    {
        public int DOC_ID { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> SCHOOLYEAR_ID { get; set; }

        [Display(Name = "Β.Ν.Σ.")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Περιφερειακή")]
        public Nullable<int> PERIFERIAKI_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Πληροφορίες")]
        public Nullable<int> ADMIN_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Μήνας")]
        public Nullable<int> DOC_MONTH { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Έτος")]
        public Nullable<int> DOC_YEAR { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> DOC_DATE { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Αρ. Πρωτ.")]
        public string DOC_PROTOCOL { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Σχετικά")]
        public string DOC_SXETIKA { get; set; }

        [Display(Name = "Ορθή Επανάληψη")]
        public bool CORRECTION { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία ορθού")]
        public Nullable<System.DateTime> CORRECTION_DATE { get; set; }
    }

    public class DocProgrammaViewModel
    {
        public int DOC_ID { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> SCHOOLYEAR_ID { get; set; }

        [Display(Name = "Β.Ν.Σ.")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Περιφερειακή")]
        public Nullable<int> PERIFERIAKI_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Πληροφορίες")]
        public Nullable<int> ADMIN_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Μήνας")]
        public Nullable<int> DOC_MONTH { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Έτος")]
        public Nullable<int> DOC_YEAR { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> DOC_DATE { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Αρ. Πρωτ.")]
        public string DOC_PROTOCOL { get; set; }

        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Σχετικά")]
        public string DOC_SXETIKA { get; set; }

        [Display(Name = "Ορθή Επανάληψη")]
        public bool CORRECTION { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημ/νία ορθού")]
        public Nullable<System.DateTime> CORRECTION_DATE { get; set; }
    }

    public class DocumentParameters
    {
        public int documentId { get; set; }
        public int schoolyearId { get; set; }
        public int stationId { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }

    }

    public class ApofasiParameters
    {
        public int apofasiId { get; set; }
        public int schoolyearId { get; set; }
        public int stationId { get; set; }
        public string apofasiType { get; set; }

    }

    public class DocUploadsViewModel
    {
        public int UPLOAD_ID { get; set; }

        [Display(Name = "ΒΝΣ")]
        public Nullable<int> STATION_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> SCHOOLYEAR_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> UPLOAD_DATE { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [Display(Name = "Χειριστής")]
        public string UPLOAD_NAME { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(500, ErrorMessage = "Πρέπει να είναι μέχρι 500 χαρακτήρες.")]
        [Display(Name = "Περιγραφή αρχείων")]
        public string UPLOAD_SUMMARY { get; set; }
    }

    public class DocUploadsFilesViewModel
    {
        [Display(Name = "Κωδικός")]
        public System.Guid ID { get; set; }

        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [Display(Name = "Όνομα χρήστη")]
        public string STATION_USER { get; set; }

        [StringLength(10, ErrorMessage = "Πρέπει να είναι μέχρι 10 χαρακτήρες.")]
        [Display(Name = "Σχολ. έτος")]
        public string SCHOOLYEAR_TEXT { get; set; }

        [StringLength(120, ErrorMessage = "Πρέπει να είναι μέχρι 120 χαρακτήρες.")]
        [Display(Name = "Όνομα αρχείου")]
        public string FILENAME { get; set; }

        [StringLength(10, ErrorMessage = "Πρέπει να είναι μέχρι 10 χαρακτήρες.")]
        [Display(Name = "Επέκταση")]
        public string EXTENSION { get; set; }

        public Nullable<int> UPLOAD_ID { get; set; }

        public virtual DOCUPLOADS DOCUPLOADS { get; set; }
    }

}