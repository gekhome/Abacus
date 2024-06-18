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
    public class MealBabyViewModel
    {
        public int ΒΡΕΦΙΚΟ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ.,&\(\)]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά, στίξη, παρενθέσεις")]
        [Display(Name = "Βρεφικό")]
        public string ΒΡΕΦΙΚΟ { get; set; }

        [Display(Name = "Παρατηρήσεις")]
        public string ΣΧΟΛΙΟ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }
    }

    public class MealNoonViewModel
    {
        public int ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ.,&\(\)]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά, στίξη, παρενθέσεις")]
        [Display(Name = "Μεσημεριανό")]
        public string ΜΕΣΗΜΕΡΙΑΝΟ { get; set; }

        [Display(Name = "Παρατηρήσεις")]
        public string ΣΧΟΛΙΟ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }
    }

    public class MealMorningViewModel
    {
        public int ΠΡΩΙΝΟ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ.,&\(\)]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά, στίξη, παρενθέσεις")]
        [Display(Name = "Πρωινό")]
        public string ΠΡΩΙΝΟ { get; set; }

        [Display(Name = "Παρατηρήσεις")]
        public string ΣΧΟΛΙΟ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }
    }

    public class DiaitologioViewModel
    {
        public int ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΣΤΑΘΜΟΣ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Πρωινό")]
        public Nullable<int> ΓΕΥΜΑ_ΠΡΩΙ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Μεσημεριανό")]
        public Nullable<int> ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ { get; set; }

        //[Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Βρεφικό")]
        public Nullable<int> ΓΕΥΜΑ_ΒΡΕΦΗ { get; set; }

        [Display(Name = "Τρόφιμοι")]
        public Nullable<int> ΤΡΟΦΙΜΟΙ { get; set; }

        [Display(Name = "Προσωπικό")]
        public Nullable<int> ΠΡΟΣΩΠΙΚΟ { get; set; }

        [Display(Name = "Άτομα")]
        public Nullable<int> ΠΛΗΘΟΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος")]
        public Nullable<decimal> ΚΟΣΤΟΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη")]
        public Nullable<decimal> ΔΑΠΑΝΗ { get; set; }
    }

    public class MealPlanData
    {
        public int ΤΡΟΦΙΜΟΙ { get; set; }

        public int ΠΡΟΣΩΠΙΚΟ { get; set; }

        public int ΠΛΗΘΟΣ { get; set; }

        public decimal ΚΟΣΤΟΣ { get; set; }

        public decimal ΔΑΠΑΝΗ { get; set; }
    }

    public class sqlMealPlanViewModel
    {
        public int ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΣΤΑΘΜΟΣ_ΚΩΔ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Ημέρα")]
        public Nullable<int> DAY_NUM { get; set; }

        [Display(Name = "Πρωινό")]
        public string ΠΡΩΙΝΟ { get; set; }

        [Display(Name = "Μεσημεριανό")]
        public string ΜΕΣΗΜΕΡΙΑΝΟ { get; set; }

        [Display(Name = "Βρεφικό")]
        public string ΒΡΕΦΙΚΟ { get; set; }

        [Display(Name = "Τρόφιμοι")]
        public Nullable<int> ΤΡΟΦΙΜΟΙ { get; set; }

        [Display(Name = "Προσωπικό")]
        public Nullable<int> ΠΡΟΣΩΠΙΚΟ { get; set; }

        [Display(Name = "Άτομα")]
        public Nullable<int> ΠΛΗΘΟΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος")]
        public Nullable<decimal> ΚΟΣΤΟΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη")]
        public Nullable<decimal> ΔΑΠΑΝΗ { get; set; }
    }

    public class MealPlanParameters
    {
        public int STATION_ID { get; set; }

        public string DATE_MEALPLAN { get; set; }
    }

    public class DiaitologioParameters
    {
        public int STATION_ID { get; set; }

        public string DATE_START { get; set; }

        public string DATE_END { get; set; }
    }

}