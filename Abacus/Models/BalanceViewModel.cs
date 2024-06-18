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
    public class BalanceMonthViewModel
    {
        public int BALANCE_ID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> STATION_ID { get; set; }

        [Display(Name = "Σχολικό έτος")]
        public Nullable<int> SCHOOLYEAR_ID { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> MONTH_ID { get; set; }

        [Display(Name = "Τρόφιμοι")]
        public int CHILDREN_NUMBER { get; set; }

        [Display(Name = "Προσωπικό")]
        public int PERSONNEL_NUMBER { get; set; }

        [Display(Name = "Άτομα")]
        public int PERSONS_NUMBER { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος τροφείου")]
        public decimal COST_FEED { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος καθαριότητας")]
        public decimal COST_CLEAN { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος γενικών")]
        public decimal COST_OTHER { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη τροφείου")]
        public decimal EXPENSE_FEED { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη καθαριότητας")]
        public decimal EXPENSE_CLEAN { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη γενικών")]
        public decimal EXPENSE_OTHER { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Υπόλοιπο τροφείου")]
        public decimal BALANCE_FEED { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Υπόλοιπο καθαριότητας")]
        public decimal BALANCE_CLEAN { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Υπόλοιπο γενικών")]
        public decimal BALANCE_OTHER { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο κόστους")]
        public decimal COST_TOTAL { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο δαπανών")]
        public decimal EXPENSE_TOTAL { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ολικό υπόλοιπο")]
        public decimal BALANCE_TOTAL { get; set; }

    }

    public class expBalanceMonthViewModel
    {
        public int ROWID { get; set; }

        [Display(Name = "Σταθμός")]
        public Nullable<int> ΒΝΣ { get; set; }

        [Display(Name = "Σταθμός")]
        public string ΕΠΩΝΥΜΙΑ { get; set; }

        [Display(Name = "Σχολικό έτος")]
        public Nullable<int> ΣΧΟΛ_ΕΤΟΣ { get; set; }

        [Display(Name = "Μήνας")]
        public Nullable<int> ΜΗΝΑΣ_ΑΡΙΘΜΟΣ { get; set; }

        [Display(Name = "Μήνας")]
        public string ΜΗΝΑΣ { get; set; }

        [Display(Name = "Τρόφιμοι")]
        public int ΠΑΙΔΙΑ_ΔΥΝΑΜΗ { get; set; }

        [Display(Name = "Προσωπικό")]
        public int ΠΡΟΣΩΠΙΚΟ_ΔΥΝΑΜΗ { get; set; }

        [Display(Name = "Άτομα")]
        public int ΠΛΗΘΟΣ_ΑΤΟΜΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος τροφείου")]
        public decimal ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος καθαριότητας")]
        public decimal ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Κόστος γενικών")]
        public decimal ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη τροφείου")]
        public decimal ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Δαπάνη καθαριότητας")]
        public decimal ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Γενικές δαπάνες")]
        public decimal ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Υπόλοιπο τροφείου")]
        public decimal ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Υπόλοιπο καθαριότητας")]
        public decimal ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Υπόλοιπο γενικών")]
        public decimal ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο κόστους")]
        public decimal ΚΟΣΤΟΣ_ΣΥΝΟΛΟ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Σύνολο δαπανών")]
        public decimal ΔΑΠΑΝΗ_ΣΥΝΟΛΟ { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2} €")]
        [Display(Name = "Ολικό υπόλοιπο")]
        public decimal ΥΠΟΛΟΙΠΟ_ΣΥΝΟΛΟ { get; set; }
    }

}