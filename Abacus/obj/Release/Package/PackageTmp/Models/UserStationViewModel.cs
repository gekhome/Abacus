using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Abacus.Models
{
    public class UserStationViewModel
    {
        public int USER_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση ονόματος χρήστη")]
        [StringLength(20, ErrorMessage = "Πρέπει να είναι μέχρι 20 χαρακτήρες.")]
        [Display(Name = "Όνομα χρήστη")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση κωδικού ασφαλείας")]
        [StringLength(20, ErrorMessage = "Πρέπει να είναι μέχρι 20 χαρακτήρες.")]
        [DataType(DataType.Password)]
        [Display(Name = "Κωδικός πρόσβασης")]
        public string PASSWORD { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Σταθμός")]
        public int STATION_ID { get; set; }

        [Display(Name = "Ενεργός")]
        public bool ISACTIVE { get; set; }
    }
}