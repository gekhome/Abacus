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
    public class BudgetDataViewModel
    {
        public int BUDGET_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Σταθμός")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Σχολικό έτος")]
        public Nullable<int> SCHOOLYEAR_ID { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> BUDGET_MONTH { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ποσό τροφείου")]
        public Nullable<decimal> BUDGET_FOOD { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ποσό καθαριότητας")]
        public Nullable<decimal> BUDGET_CLEAN { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ποσό γενικών δαπανών")]
        public Nullable<decimal> BUDGET_OTHER { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Παιδιά")]
        public int CHILDREN_NUM { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Υπαλληλοι")]
        public int PERSONNEL_NUM { get; set; }

        public virtual ΣΥΣ_ΣΤΑΘΜΟΙ ΣΥΣ_ΣΤΑΘΜΟΙ { get; set; }

    }

    #region ΠΡΟΪΟΝΤΑ

    public class PersonCostDayViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Σχολικό έτος")]
        public Nullable<int> SCHOOLYEARID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Σταθμός")]
        public Nullable<int> STATION_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N4} €")]
        [Display(Name = "Κόστος ανά άτομο")]
        public Nullable<decimal> COST_PERSON { get; set; }

        public virtual ΣΥΣ_ΣΤΑΘΜΟΙ ΣΥΣ_ΣΤΑΘΜΟΙ { get; set; }
    }

    public class ProductCategoryViewModel
    {
        public int ΚΑΤΗΓΟΡΙΑ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [Display(Name = "Κατηγορία προϊόντος")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Δαπάνη είδος")]
        public Nullable<int> ΔΑΠΑΝΗ_ΚΩΔ { get; set; }

        public virtual ΔΑΠΑΝΕΣ_ΕΙΔΗ ΔΑΠΑΝΕΣ_ΕΙΔΗ { get; set; }
    }

    public class ExtraCategoryViewModel
    {
        public int ΚΑΤΗΓΟΡΙΑ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(150, ErrorMessage = "Πρέπει να είναι μέχρι 150 χαρακτήρες.")]
        [Display(Name = "Κατηγορία άλλη")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Είδος δαπάνης")]
        public Nullable<int> ΔΑΠΑΝΗ_ΚΩΔ { get; set; }
    }

    public class ProductUnitViewModel
    {
        public int ΜΟΝΑΔΑ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(50, ErrorMessage = "Πρέπει να είναι μέχρι 50 χαρακτήρες.")]
        [Display(Name = "Μονάδα κόστους")]
        public string ΜΟΝΑΔΑ { get; set; }
    }

    public class ProductViewModel
    {
        public int ΠΡΟΙΟΝ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Προϊόν διατροφής")]
        public string ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Μονάδα μέτρησης")]
        public Nullable<int> ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "ΦΠΑ %")]
        public int ΠΡΟΙΟΝ_ΦΠΑ { get; set; }
    }

    public class ProductStationViewModel
    {
        public int ΠΡΟΙΟΝ_ΚΩΔ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [RegularExpression(@"^[Α-ΩA-Z]+[ Α-ΩA-Z-_ΪΫ.,&\(\)]*$", ErrorMessage = "Μόνο κεφαλαία ελληνικά, λατινικά, στίξη, παρενθέσεις")]
        [Display(Name = "Προϊόν διατροφής")]
        public string ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Μονάδα μέτρησης")]
        public Nullable<int> ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "ΦΠΑ %")]
        public int ΠΡΟΙΟΝ_ΦΠΑ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }
    }

    public class VATScaleViewModel
    {
        public int FPA_ID { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "ΦΠΑ κλίμακα")]
        public Nullable<decimal> FPA_VALUE { get; set; }
    }

    public class sqlProductListViewModel
    {
        public int ΠΡΟΙΟΝ_ΚΩΔ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Προϊόν")]
        public string ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ { get; set; }

        [Display(Name = "Μονάδα μέτρησης")]
        public string ΜΟΝΑΔΑ { get; set; }

        public int ΜΟΝΑΔΑ_ΚΩΔ { get; set; }

        public int ΚΑΤΗΓΟΡΙΑ_ΚΩΔ { get; set; }
        public Nullable<int> ΔΑΠΑΝΗ_ΚΩΔ { get; set; }

        [Display(Name = "Δαπάνη είδος")]
        public string ΕΙΔΟΣ_ΛΕΚΤΙΚΟ { get; set; }

        [Display(Name = "ΦΠΑ %")]
        public Nullable<int> ΠΡΟΙΟΝ_ΦΠΑ { get; set; }

        public Nullable<decimal> FPA_VALUE { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }
    }

    public partial class sqlProductSelectorViewModel
    {
        public int ΠΡΟΙΟΝ_ΚΩΔ { get; set; }

        [Display(Name = "Προϊόν και μονάδα μέτρησης")]
        public string ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }
        public int ΚΑΤΗΓΟΡΙΑ_ΚΩΔ { get; set; }
    }

    #endregion

    #region ΕΙΔΗ ΔΑΠΑΝΩΝ

    public class CostFeedingViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Προϊόν, μονάδα μέτρησης")]
        public Nullable<int> ΠΡΟΙΟΝ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N3} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }

        public virtual ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ { get; set; }
        public virtual ΠΡΟΙΟΝΤΑ ΠΡΟΙΟΝΤΑ { get; set; }
    }

    public class CostCleaningViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Προϊόν, μονάδα μέτρησης")]
        public Nullable<int> ΠΡΟΙΟΝ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N3} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }

        public virtual ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ { get; set; }
        public virtual ΠΡΟΙΟΝΤΑ ΠΡΟΙΟΝΤΑ { get; set; }
    }

    public class CostOtherViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Προϊόν, μονάδα μέτρησης")]
        public Nullable<int> ΠΡΟΙΟΝ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [DisplayFormat(DataFormatString = "{0:N3} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }

        public virtual ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ { get; set; }
        public virtual ΠΡΟΙΟΝΤΑ ΠΡΟΙΟΝΤΑ { get; set; }
    }

    public class CostGeneralViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [Display(Name = "Κατηγορία")]
        public Nullable<int> ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Required(ErrorMessage = "Υποχρεωτική συμπλήρωση")]
        [StringLength(255, ErrorMessage = "Πρέπει να είναι μέχρι 255 χαρακτήρες.")]
        [Display(Name = "Περιγραφή")]
        public string ΠΕΡΙΓΡΑΦΗ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }
        public virtual ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ { get; set; }
    }

    #endregion

    #region ΕΥΡΕΤΗΡΙΑ ΔΑΠΑΝΩΝ

    public class sqlCostFoodViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Προϊόν, μονάδα μέτρησης")]
        public string ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }
    }

    public class sqlCostCleaningViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Προϊόν, μονάδα μέτρησης")]
        public string ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }
    }

    public class sqlCostOtherViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Προϊόν, μονάδα μέτρησης")]
        public string ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }
    }

    public class sqlCostGeneralViewModel
    {
        public int ΚΩΔΙΚΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Περιγραφή")]
        public string ΠΕΡΙΓΡΑΦΗ { get; set; }

        [Display(Name = "Ποσότητα")]
        public int ΠΟΣΟΤΗΤΑ { get; set; }

        [Display(Name = "Τιμή μονάδας")]
        public int ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }
    }

    public class sqlCostVariousViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Περιγραφή")]
        public string ΠΡΟΙΟΝ_ΜΟΝΑΔΑ { get; set; }

        [Display(Name = "Ποσότητα")]
        public Nullable<decimal> ΠΟΣΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Τιμή μονάδας")]
        public Nullable<decimal> ΤΙΜΗ_ΜΟΝΑΔΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }
    }

    // ΕΝΟΠΟΙΗΜΕΝΕΣ ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ ΔΑΠΑΝΕΣ
    public class sqlCostAllOtherViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Κατηγορία")]
        public string ΚΑΤΗΓΟΡΙΑ { get; set; }

        [Display(Name = "Περιγραφή υλικών")]
        public string ΠΕΡΙΓΡΑΦΗ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> ΣΥΝΟΛΟ { get; set; }

        [Display(Name = "Είδος δαπάνης")]
        public string ΕΙΔΟΣ { get; set; }
    }

    #endregion

    #region ΣΥΓΚΕΝΤΡΩΤΙΚΑ ΣΤΟΙΧΕΙΑ ΔΑΠΑΝΩΝ

    public class SumPersonsTrofeioViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> SCHOOLYEARID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> ΜΗΝΑΣ { get; set; }

        [Display(Name = "Παιδιά")]
        public Nullable<int> ΠΑΙΔΙΑ { get; set; }

        [Display(Name = "Προσωπικό")]
        public Nullable<int> ΠΡΟΣΩΠΙΚΟ { get; set; }

        [Display(Name = "Άτομα")]
        public Nullable<int> ΑΤΟΜΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ημερ. κόστος")]
        public Nullable<decimal> ΚΟΣΤΟΣ_ΗΜΕΡΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ημερ. δαπάνη")]
        public Nullable<decimal> ΔΑΠΑΝΗ_ΗΜΕΡΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ημερ. υπόλοιπο")]
        public Nullable<decimal> ΥΠΟΛΟΙΠΟ { get; set; }
    }

    public class SumCleaningDayViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> TOTAL_DAY { get; set; }

        public Nullable<int> ΜΗΝΑΣ_ΑΡΙΘΜΟΣ { get; set; }
    }

    public class SumOtherExpenseDayViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> TOTAL_DAY { get; set; }

        public Nullable<int> ΜΗΝΑΣ_ΑΡΙΘΜΟΣ { get; set; }
    }

    public class SumExtraExpenseDayViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public Nullable<decimal> TOTAL_DAY { get; set; }

        public Nullable<int> ΜΗΝΑΣ_ΑΡΙΘΜΟΣ { get; set; }
    }
    
    // ΕΝΟΠΟΙΗΜΕΝΕΣ ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ
    public class SumAllOtherExpenseDayViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σχολ. έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ημερομηνία")]
        public Nullable<System.DateTime> ΗΜΕΡΟΜΗΝΙΑ { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> ΜΗΝΑΣ_ΑΡΙΘΜΟΣ { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο")]
        public decimal TOTAL_DAY { get; set; }

        [Display(Name = "Είδος δαπάνης")]
        public string ΕΙΔΟΣ { get; set; }
    }

    #endregion

    // ΠΑΡΑΜΕΤΡΟΙ ΕΚΘΕΣΕΩΝ ΔΑΠΑΝΩΝ
    public class ExpenseReportParameters
    {
        public int? STATION_ID { get; set; }

        public string DATE_EXPENSE { get; set; }

        public int? SYEAR_ID { get; set; }
    }

}