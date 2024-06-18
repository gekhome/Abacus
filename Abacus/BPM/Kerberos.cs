using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abacus.DAL;
using Abacus.Models;
using Abacus.BPM;

namespace Abacus.BPM
{
    public static class Kerberos
    {
        public const int TICKET_TIMEOUT_MINUTES = 280;

        /// <summary>
        /// Υπολογίζει τις εργάσιμες ημέρες μεταξύ δύο ημερομηνιών,
        /// δηλ. χωρίς τα Σαββατοκύριακα.
        /// </summary>
        /// <param name="initial_date"></param>
        /// <param name="final_date"></param>
        /// <returns name="daycount"></returns>
        public static int WorkingDays(DateTime initial_date, DateTime final_date)
        {
            int daycount = 0;

            DateTime date1 = initial_date;
            DateTime date2 = final_date;

            while (date1 <= date2)
            {
                switch (date1.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        break;
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        daycount++;
                        break;
                    default:
                        break;
                }
                date1 = date1.AddDays(1);
            }
            return daycount;
        }


        #region ΚΑΝΟΝΕΣ ΔΙΑΓΡΑΦΗΣ ΠΑΡΑΜΕΤΡΙΚΩΝ ΣΤΟΙΧΕΙΩΝ

        public static bool CanDeleteMealMorning(int recordId)
        {
            using (var db = new AbacusDBEntities())
            {
                var count = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ where d.ΓΕΥΜΑ_ΠΡΩΙ == recordId select d).Count();
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteMealNoon(int recordId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ where d.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ == recordId select d).Count();
                if (data > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteMealBaby(int recordId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ where d.ΓΕΥΜΑ_ΒΡΕΦΗ == recordId select d).Count();
                if (data > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteApoxorisiType(int apoxorisiId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ == apoxorisiId select d).Count();
                if (data > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteProduct(int productId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data1 = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ where d.ΠΡΟΙΟΝ == productId select d).Count();
                var data2 = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ where d.ΠΡΟΙΟΝ == productId select d).Count();
                var data3 = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ where d.ΠΡΟΙΟΝ == productId select d).Count();

                if (data1 > 0 || data2 > 0 || data3 > 0) return false;
                else return true;
            }
        }

        public static bool CanDeleteVATvalue(int vatId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΠΡΟΙΟΝΤΑ where d.ΠΡΟΙΟΝ_ΦΠΑ == vatId select d).ToList();
                if (data.Count > 0) return false;
                else return true;
            }
        }

        public static bool CanDeleteExtraCategory(int categoryId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ where d.ΚΑΤΗΓΟΡΙΑ == categoryId select d).Count();
                if (data > 0) return false;
                else return true;
            }
        }

        public static bool CanDeleteCategory(int categoryId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΠΡΟΙΟΝΤΑ where d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ == categoryId select d).Count();
                if (data > 0) return false;
                else return true;
            }
        }

        public static bool CanDeleteSchoolYear(int schoolyearId)
        {
            return false;
        }

        public static bool CanDeleteHour(int hourId)
        {
            using (var db = new AbacusDBEntities())
            {
                var count = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ where d.HOUR_START == hourId || d.HOUR_END == hourId select d).Count();
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeletePeriferiaki(int periferiakiId)
        {
            using (var db = new AbacusDBEntities())
            {
                int count = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ where d.ΠΕΡΙΦΕΡΕΙΑΚΗ == periferiakiId select d).Count();
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteEidikotita(int eidikotitaId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΕΙΔΙΚΟΤΗΤΑ == eidikotitaId select d).Count();
                if (data > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteTmima(int tmimaId)
        {
            using (var db = new AbacusDBEntities())
            {
                int count1 = (from d in db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ where d.ΤΜΗΜΑ == tmimaId select d).Count();
                int count2 = (from d in db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ where d.TMIMA_ID == tmimaId select d).Count();
                int count3 = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ where d.TMIMA_ID == tmimaId select d).Count();
                if (count1 > 0 || count2 > 0 || count3 > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteTmimaCategory(int categoryId)
        {
            using (var db = new AbacusDBEntities())
            {
                var count = (from d in db.ΤΜΗΜΑ where d.ΚΑΤΗΓΟΡΙΑ == categoryId select d).Count();
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeletePersonnelType(int prosopikoTypeId)
        {
            using (var db = new AbacusDBEntities())
            {
                int count = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == prosopikoTypeId select d).Count();
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteMetaboliType(int metaboliId)
        {
            using (var db = new AbacusDBEntities())
            {
                int count = (from d in db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ where d.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ == metaboliId select d).Count();
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }

        public static bool CanDeleteChild(int childId)
        {
            using (var db = new AbacusDBEntities())
            {
                var count1 = (from d in db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ where d.ΠΑΙΔΙ_ΚΩΔ == childId select d).Count();
                int count2 = (from d in db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ where d.CHILD_ID == childId select d).Count();
                int count3 = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ where d.CHILD_ID == childId select d).Count();

                if (count1 > 0 || count2 > 0 || count3 > 0) return false;
                else return true;
            }
        }

        public static bool CanDeletePerson(int personId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data1 = (from d in db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ where d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId select d).Count();
                var data2 = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ where d.PERSON_ID == personId select d).Count();

                if (data1 > 0 || data2 > 0) return false;
                else return true;
            }
        }

        public static bool CanDeleteDiaxiristis(int adminId)
        {
            return false;
        }

        public static bool CanDeleteOperator(int operatorId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data1 = (from d in db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ where d.ADMIN_ID == operatorId select d).Count();
                var data2 = (from d in db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ where d.ADMIN_ID == operatorId select d).Count();

                if (data1 == 0 && data2 == 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool CanDeleteUpload(int uploadId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.DOCUPLOADS_FILES where d.UPLOAD_ID == uploadId select d).Count();
                if (data > 0) return false;
                else return true;
            }
        }

        #endregion  ΚΑΝΟΝΕΣ ΔΙΑΓΡΑΦΗΣ ΠΑΡΑΜΕΤΡΙΚΩΝ ΣΤΟΙΧΕΙΩΝ


        #region ΓΕΝΙΚΟΙ ΕΛΕΓΧΟΙ ΕΠΙΚΥΡΩΣΗΣ ΔΕΔΟΜΕΝΩΝ

        public static string ValidateChildFields(ΠΑΙΔΙΑ s)
        {
            string errMsg = "";

            //if (!Common.ValidateBirthdate(s)) errMsg += "-> Η ημ/νια γέννησης είναι εκτός λογικών ορίων. ";
            if (!Common.CheckAFM(s.ΑΦΜ)) errMsg += "-> Το ΑΦΜ δεν είναι έγκυρο. ";
            return (errMsg);
        }

        public static bool ValidateCurrentSchoolYear()
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.ΤΡΕΧΟΝ_ΕΤΟΣ == true select d).Count();
                if (data > 1 || data == 0) 
                    return false;
                else 
                    return true;
            }
        }

        public static bool ExistsUsername(string username)
        {
            using (var db = new AbacusDBEntities())
            {
                var userAdmins = db.USER_ADMINS.Where(u => u.USERNAME == username).FirstOrDefault(); ;
                var userStations = db.USER_STATIONS.Where(u => u.USERNAME == username).FirstOrDefault();

                return (userAdmins != null || userStations != null);
            }
        }

        public static bool ValidatePrimaryKeyChild(int am, int stationId)
        {
            using (var db = new AbacusDBEntities())
            {
                var existingdata = db.ΠΑΙΔΙΑ.Where(s => s.ΑΜ == am && s.ΒΝΣ == stationId).Count();
                if (existingdata == 0) return true;
                else return false;
            }
        }

        public static bool ValidatePrimaryKeyPerson(int am, int stationId)
        {
            if (am == 0)
                return true;

            using (var db = new AbacusDBEntities())
            {
                var existingdata = db.ΠΡΟΣΩΠΙΚΟ.Where(s => s.ΜΗΤΡΩΟ == am && s.ΒΝΣ == stationId).Count();
                if (existingdata == 0) return true;
                else return false;
            }
        }

        public static bool IsValidAM(PersonnelGridViewModel p)
        {
            if (p.ΜΗΤΡΩΟ == 0 && p.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ != 5)
                return false;
            return true;
        }

        public static string ValidatePersonFields(ΠΡΟΣΩΠΙΚΟ p)
        {
            string errMsg = "";

            bool apoxorisiError1 = p.ΑΠΟΧΩΡΗΣΕ == true && (p.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ == null || p.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ == null);
            bool apoxorisiError2 = p.ΑΠΟΧΩΡΗΣΕ == false && (p.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ > 0 || p.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ != null);

            if (!Common.CheckAFM(p.ΑΦΜ)) 
                errMsg += "-> Το ΑΦΜ δεν είναι έγκυρο. ";
            if (p.ΜΗΤΡΩΟ == 0 && p.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ != 5)
                errMsg += "Ο Α.Μ. μπορεί να έχει μηδενική τιμή μόνο για ασκούμενους.";
            if (apoxorisiError1) 
                errMsg += "-> Έχει τσεκαριστεί αποχώρηση, οπότε πρέπει να συμπληρωθεί αιτία και ημ/νία αποχώρησης.";
            if (apoxorisiError2) 
                errMsg += "-> Δεν έχει τσεκαριστεί αποχώρηση, οπότε αιτία και ημ/νία αποχώρησης πρέπει να είναι κενά.";

            return (errMsg);
        }

        public static bool MealsMorningExist(int stationId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ where d.ΒΝΣ == stationId select d).Count();
                if (data > 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool MealsNoonExist(int stationId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ where d.ΒΝΣ == stationId select d).Count();
                if (data > 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool MealsBabyExist(int stationId)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ where d.ΒΝΣ == stationId select d).Count();
                if (data > 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool CanPopulateMeals()
        {
            using (var db = new AbacusDBEntities())
            {
                var data1 = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ select d).Count();
                var data2 = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ select d).Count();
                var data3 = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ select d).Count();

                if (data1 > 0 && data2 > 0 && data3 > 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool BudgetDataExists(BudgetDataViewModel data, int schoolyearId, int monthId)
        {
            using (var db = new AbacusDBEntities())
            {
                var budgetData = (from d in db.BUDGET_DATA
                                  where d.SCHOOLYEAR_ID == schoolyearId && d.BUDGET_MONTH == monthId && d.STATION_ID == data.STATION_ID
                                  select d).FirstOrDefault();
                if (budgetData != null) return true;
                else return false;
            }
        }

        public static bool CostFoodExists(CostFeedingViewModel item)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ
                            where d.ΒΝΣ == item.ΒΝΣ && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΡΟΙΟΝ == item.ΠΡΟΙΟΝ && d.ΠΟΣΟΤΗΤΑ == item.ΠΟΣΟΤΗΤΑ && d.ΤΙΜΗ_ΜΟΝΑΔΑ == item.ΤΙΜΗ_ΜΟΝΑΔΑ
                            select d).Count();
                if (data > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public static bool CostCleaningExists(CostCleaningViewModel item)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                            where d.ΒΝΣ == item.ΒΝΣ && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΡΟΙΟΝ == item.ΠΡΟΙΟΝ && d.ΠΟΣΟΤΗΤΑ == item.ΠΟΣΟΤΗΤΑ && d.ΤΙΜΗ_ΜΟΝΑΔΑ == item.ΤΙΜΗ_ΜΟΝΑΔΑ
                            select d).Count();
                if (data > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public static bool CostOtherExists(CostOtherViewModel item)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ
                            where d.ΒΝΣ == item.ΒΝΣ && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΡΟΙΟΝ == item.ΠΡΟΙΟΝ 
                            && d.ΠΟΣΟΤΗΤΑ == item.ΠΟΣΟΤΗΤΑ && d.ΤΙΜΗ_ΜΟΝΑΔΑ == item.ΤΙΜΗ_ΜΟΝΑΔΑ
                            select d).Count();
                if (data > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public static bool CostGeneralExists(CostGeneralViewModel item)
        {
            using (var db = new AbacusDBEntities())
            {
                var data = (from d in db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                            where d.ΒΝΣ == item.ΒΝΣ && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ
                                    && d.ΠΕΡΙΓΡΑΦΗ == item.ΠΕΡΙΓΡΑΦΗ && d.ΣΥΝΟΛΟ == item.ΣΥΝΟΛΟ
                            select d).Count();
                if (data > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public static string ValidateCostValue(decimal? quantity, decimal? unit_cost)
        {
            string errMsg = "";

            if (quantity <= 0 || unit_cost <= 0)
                errMsg = "Η ποσότητα και η τιμή μονάδας πρέπει να είναι αριθμοί μεγαλύτεροι του μηδενός.";

            return errMsg;
        }

        public static string ValidateCostGeneral(CostGeneralViewModel item)
        {
            string errMsg = "";

            if (item.ΣΥΝΟΛΟ <= 0)
                errMsg = "Το κόστος πρέπει να είναι αριθμός μεγαλύτερος του μηδενός.";

            return errMsg;
        }

        #endregion

    }
}